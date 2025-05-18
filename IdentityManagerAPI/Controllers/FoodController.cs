using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Food;
using Models.DTOs.Food.Arabic;
using Models.Domain;
using DataAcess.DbContexts;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Models.ArabicDomain;

namespace Graduation_project_APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ArabicDbContext _arabicDb;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;

        public FoodController(
            ApplicationDbContext db,
            ArabicDbContext arabicDb,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache)
        {
            _db = db;
            _arabicDb = arabicDb;
            _mapper = mapper;
            _userManager = userManager;
            _cache = cache;
        }

        private bool IsArabicRequest()
        {
            var acceptLanguage = Request.Headers["Accept-Language"].ToString();
            return acceptLanguage.StartsWith("ar", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<(HashSet<int> favoriteIds, HashSet<int> cartIds)> GetUserFavoritesAndCartAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return (new HashSet<int>(), new HashSet<int>());

            var favIds = await _db.FavoriteRecipes
                .Where(f => f.UserId == user.Id)
                .Select(f => f.RecipeId)
                .ToHashSetAsync();

            var cartIds = await _db.CartItems
                .Where(c => c.UserId == user.Id)
                .Select(c => c.RecipeId)
                .ToHashSetAsync();

            return (favIds, cartIds);
        }

        private async Task<List<وصفة_مع_تغذية_DTO>> MapArabicRecipesWithImageUrl(List<الوصفات> recipes)
        {
            var dtos = _mapper.Map<List<وصفة_مع_تغذية_DTO>>(recipes);
            var recipeIds = dtos.Select(dto => dto.RecipeId).ToList();
            var imageUrls = await _db.Recipe
                .Where(r => recipeIds.Contains(r.Recipe_Id))
                .Select(r => new { r.Recipe_Id, r.ImageUrl })
                .ToDictionaryAsync(r => r.Recipe_Id, r => r.ImageUrl);

            var (favIds, cartIds) = await GetUserFavoritesAndCartAsync();

            foreach (var dto in dtos)
            {
                var originalUrl = imageUrls.GetValueOrDefault(dto.RecipeId);
                dto.ImageUrl = originalUrl != null ? $"https://your-api.com/api/Food/image-proxy/{dto.RecipeId}" : null;
                dto.IsFavorite = favIds.Contains(dto.RecipeId);
                dto.IsInCart = cartIds.Contains(dto.RecipeId);
            }

            return dtos;
        }

        [HttpGet("image-proxy/{recipeId}")]
        public async Task<IActionResult> GetImageProxy(int recipeId)
        {
            var cacheKey = $"image_{recipeId}";
            if (!_cache.TryGetValue(cacheKey, out byte[] content))
            {
                var imageUrl = await _db.Recipe
                    .Where(r => r.Recipe_Id == recipeId)
                    .Select(r => r.ImageUrl)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(imageUrl))
                    return NotFound();

                // تحويل رابط Google Drive إلى رابط مباشر
                if (imageUrl.Contains("drive.google.com/file/d"))
                {
                    var fileId = Regex.Match(imageUrl, @"file/d/([a-zA-Z0-9_-]+)/")?.Groups[1].Value;
                    if (!string.IsNullOrEmpty(fileId))
                        imageUrl = $"https://drive.google.com/uc?export=download&id={fileId}";
                }

                using var client = new HttpClient();
                try
                {
                    var response = await client.GetAsync(imageUrl);
                    if (!response.IsSuccessStatusCode)
                        return StatusCode((int)response.StatusCode);

                    content = await response.Content.ReadAsByteArrayAsync();
                    _cache.Set(cacheKey, content, TimeSpan.FromHours(24));
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to fetch image");
                }
            }

            var contentType = "image/jpeg"; // أو استخدم Content-Type من الـ response لو متاح
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return File(content, contentType);
        }

        [HttpGet("recipes")]
        [Authorize] // إضافة JWT Authentication
        public async Task<IActionResult> GetAllRecipes()
        {
            if (IsArabicRequest())
            {
                var recipes = await _arabicDb.الوصفات
                    .Include(r => r.التغذية)
                    .Include(r => r.وصفة_المكونات)
                        .ThenInclude(ri => ri.المكون)
                    .AsNoTracking()
                    .Take(3000)
                    .ToListAsync();

                var result = await MapArabicRecipesWithImageUrl(recipes);
                return Ok(result);
            }
            else
            {
                var recipes = await _db.Recipe
                    .Include(r => r.Nutrition)
                    .Include(r => r.Recipe_Ingredient)
                        .ThenInclude(ri => ri.Ingredient)
                    .AsNoTracking()
                    .Take(3000)
                    .ToListAsync();

                var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
                var (favIds, cartIds) = await GetUserFavoritesAndCartAsync();

                foreach (var recipe in result)
                {
                    recipe.ImageUrl = recipe.ImageUrl != null ? $"https://your-api.com/api/Food/image-proxy/{recipe.RecipeId}" : null;
                    recipe.IsFavorite = favIds.Contains(recipe.RecipeId);
                    recipe.IsInCart = cartIds.Contains(recipe.RecipeId);
                }

                return Ok(result);
            }
        }

        [HttpGet("recipes/preview")]
        [Authorize]
        public async Task<IActionResult> GetRecipePreview()
        {
            if (IsArabicRequest())
            {
                var recipes = await _arabicDb.الوصفات
                    .Include(r => r.التغذية)
                    .Include(r => r.وصفة_المكونات)
                        .ThenInclude(ri => ri.المكون)
                    .AsNoTracking()
                    .Take(500)
                    .ToListAsync();

                var result = await MapArabicRecipesWithImageUrl(recipes);
                return Ok(result);
            }
            else
            {
                var recipes = await _db.Recipe
                    .Include(r => r.Nutrition)
                    .Include(r => r.Recipe_Ingredient)
                        .ThenInclude(ri => ri.Ingredient)
                    .AsNoTracking()
                    .Take(500)
                    .ToListAsync();

                var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
                var (favIds, cartIds) = await GetUserFavoritesAndCartAsync();

                foreach (var recipe in result)
                {
                    recipe.ImageUrl = recipe.ImageUrl != null ? $"https://your-api.com/api/Food/image-proxy/{recipe.RecipeId}" : null;
                    recipe.IsFavorite = favIds.Contains(recipe.RecipeId);
                    recipe.IsInCart = cartIds.Contains(recipe.RecipeId);
                }

                return Ok(result);
            }
        }

        [HttpGet("recipes/search/by-name/{name}")]
        [Authorize]
        public async Task<IActionResult> SearchRecipesByName(string name)
        {
            if (IsArabicRequest())
            {
                var recipes = await _arabicDb.الوصفات
                    .Include(r => r.التغذية)
                    .Include(r => r.وصفة_المكونات)
                        .ThenInclude(ri => ri.المكون)
                    .Where(r => r.اسم.Contains(name))
                    .AsNoTracking()
                    .ToListAsync();

                var result = await MapArabicRecipesWithImageUrl(recipes);
                return Ok(result);
            }
            else
            {
                var recipes = await _db.Recipe
                    .Include(r => r.Nutrition)
                    .Include(r => r.Recipe_Ingredient)
                        .ThenInclude(ri => ri.Ingredient)
                    .Where(r => r.Recipe_Name.Contains(name))
                    .AsNoTracking()
                    .ToListAsync();

                var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
                var (favIds, cartIds) = await GetUserFavoritesAndCartAsync();

                foreach (var recipe in result)
                {
                    recipe.ImageUrl = recipe.ImageUrl != null ? $"https://your-api.com/api/Food/image-proxy/{recipe.RecipeId}" : null;
                    recipe.IsFavorite = favIds.Contains(recipe.RecipeId);
                    recipe.IsInCart = cartIds.Contains(recipe.RecipeId);
                }

                return Ok(result);
            }
        }

        [HttpPost("recipes/search/by-ingredient-ids")]
        [Authorize]
        public async Task<IActionResult> SearchRecipesByIngredientIds([FromBody] List<int> ingredientIds)
        {
            if (IsArabicRequest())
            {
                var recipes = await _arabicDb.الوصفات
                    .Include(r => r.التغذية)
                    .Include(r => r.وصفة_المكونات)
                        .ThenInclude(ri => ri.المكون)
                    .Where(r => r.وصفة_المكونات.Any(ri => ingredientIds.Contains(ri.بطاقة_تعريف_المكون)))
                    .AsNoTracking()
                    .ToListAsync();

                var result = await MapArabicRecipesWithImageUrl(recipes);
                return Ok(result);
            }
            else
            {
                var recipes = await _db.Recipe
                    .Include(r => r.Nutrition)
                    .Include(r => r.Recipe_Ingredient)
                        .ThenInclude(ri => ri.Ingredient)
                    .Where(r => r.Recipe_Ingredient.Any(ri => ingredientIds.Contains(ri.Ingredient_Id)))
                    .AsNoTracking()
                    .ToListAsync();

                var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
                var (favIds, cartIds) = await GetUserFavoritesAndCartAsync();

                foreach (var recipe in result)
                {
                    recipe.ImageUrl = recipe.ImageUrl != null ? $"https://your-api.com/api/Food/image-proxy/{recipe.RecipeId}" : null;
                    recipe.IsFavorite = favIds.Contains(recipe.RecipeId);
                    recipe.IsInCart = cartIds.Contains(recipe.RecipeId);
                }

                return Ok(result);
            }

        }
        [HttpGet("ingredients")]
        public IActionResult GetAllIngredients()
        {
            if (IsArabicRequest())
            {
                var ingredients = _arabicDb.المكونات.AsNoTracking().ToList();
                return Ok(ingredients);
            }
            else
            {
                var ingredients = _db.Ingredient.AsNoTracking().ToList();
                return Ok(ingredients);
            }
        }
        [HttpGet("ingredients/search/by-name/{ingredientName}")]
        public IActionResult SearchIngredientIdsByName(string ingredientName)
        {
            if (IsArabicRequest())
            {
                var ids = _arabicDb.المكونات
                    .AsNoTracking()
                    .Where(ing => EF.Functions.Like(ing.اسم_المكون.ToLower(), $"%{ingredientName.ToLower()}%"))
                    .Select(i => i.بطاقة_تعريف)
                    .ToList();
                return Ok(ids);
            }
            else
            {
                var ids = _db.Ingredient
                    .AsNoTracking()
                    .Where(ing => EF.Functions.Like(ing.Ingredient_Name.ToLower(), $"%{ingredientName.ToLower()}%"))
                    .Select(i => i.Ingredient_Id)
                    .ToList();
                return Ok(ids);
            }
        }
    }

}