using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Food;
using Models.DTOs.Food.Arabic;
using Models.Domain;
using DataAcess.DbContexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Models.ArabicDomain;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ArabicDbContext _arabicDb;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public FoodController(IMapper mapper, ApplicationDbContext db, ArabicDbContext arabicDb, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _arabicDb = arabicDb;
        _mapper = mapper;
        _userManager = userManager;
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
            dto.ImageUrl = imageUrls.GetValueOrDefault(dto.RecipeId);
            dto.IsFavorite = favIds.Contains(dto.RecipeId);
            dto.IsInCart = cartIds.Contains(dto.RecipeId);
        }

        return dtos;
    }

    [HttpGet("recipes")]
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
                recipe.IsFavorite = favIds.Contains(recipe.RecipeId);
                recipe.IsInCart = cartIds.Contains(recipe.RecipeId);
            }

            return Ok(result);
        }
    }

    [HttpGet("recipes/preview")]
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
                recipe.IsFavorite = favIds.Contains(recipe.RecipeId);
                recipe.IsInCart = cartIds.Contains(recipe.RecipeId);
            }

            return Ok(result);
        }
    }

    [HttpGet("recipes/search/by-name/{name}")]
    public async Task<IActionResult> SearchRecipesByName(string name)
    {
        if (IsArabicRequest())
        {
            var recipes = await _arabicDb.الوصفات
                .Where(x => EF.Functions.Like(x.اسم, $"%{name}%"))
                .Include(r => r.التغذية)
                .Include(r => r.وصفة_المكونات)
                    .ThenInclude(ri => ri.المكون)
                .AsNoTracking()
                .Take(50)
                .ToListAsync();

            var result = await MapArabicRecipesWithImageUrl(recipes);
            return Ok(result);
        }
        else
        {
            var recipes = await _db.Recipe
                .Where(x => EF.Functions.Like(x.Recipe_Name, $"%{name}%"))
                .Include(r => r.Nutrition)
                .Include(r => r.Recipe_Ingredient)
                    .ThenInclude(ri => ri.Ingredient)
                .AsNoTracking()
                .Take(50)
                .ToListAsync();

            var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            var (favIds, cartIds) = await GetUserFavoritesAndCartAsync();

            foreach (var recipe in result)
            {
                recipe.IsFavorite = favIds.Contains(recipe.RecipeId);
                recipe.IsInCart = cartIds.Contains(recipe.RecipeId);
            }

            return Ok(result);
        }
    }

    [HttpPost("recipes/search/by-ingredient-ids")]
    public async Task<ActionResult> SearchRecipesByIngredientIds([FromBody] List<int> ingredientIds)
    {
        if (IsArabicRequest())
        {
            var recipes = await _arabicDb.وصفة_المكونات
                .AsNoTracking()
                .Where(ri => ingredientIds.Contains(ri.بطاقة_تعريف_المكون))
                .Select(ri => ri.الوصفة)
                .Distinct()
                .Include(r => r.التغذية)
                .Include(r => r.وصفة_المكونات)
                    .ThenInclude(ri => ri.المكون)
                .Take(50)
                .ToListAsync();

            var result = await MapArabicRecipesWithImageUrl(recipes);
            return Ok(result);
        }
        else
        {
            var recipes = await _db.Recipe_Ingredient
                .AsNoTracking()
                .Where(ri => ingredientIds.Contains(ri.Ingredient_Id))
                .Select(ri => ri.Recipe)
                .Distinct()
                .Include(r => r.Nutrition)
                .Include(r => r.Recipe_Ingredient)
                    .ThenInclude(ri => ri.Ingredient)
                .Take(50)
                .ToListAsync();

            var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            var (favIds, cartIds) = await GetUserFavoritesAndCartAsync();

            foreach (var recipe in result)
            {
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