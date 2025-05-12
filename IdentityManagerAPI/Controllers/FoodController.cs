using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Food;
using Models.Domain;
using System.Security.Claims;
using DataAcess.DbContexts;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public FoodController(IMapper mapper, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
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

    [HttpGet("recipes")]
    public async Task<IActionResult> GetAllRecipes()
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

    [HttpGet("recipes/preview")]
    public async Task<IActionResult> GetRecipePreview()
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

    [HttpGet("ingredients")]
    public IActionResult GetAllIngredients()
    {
        var ingredients = _db.Ingredient.AsNoTracking().ToList();
        return Ok(ingredients);
    }

    [HttpGet("recipes/search/by-name/{name:alpha}")]
    public async Task<IActionResult> SearchRecipesByName(string name)
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

    [HttpPost("recipes/search/by-ingredient-ids")]
    public async Task<ActionResult<List<RecipeWithNutritionDTO>>> SearchRecipesByIngredientIds([FromBody] List<int> ingredientIds)
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

    [HttpGet("ingredients/search/by-name/{ingredientName:alpha}")]
    public IActionResult SearchIngredientIdsByName(string ingredientName)
    {
        var ids = _db.Ingredient
            .AsNoTracking()
            .Where(ing => EF.Functions.Like(ing.Ingredient_Name.ToLower(), $"%{ingredientName.ToLower()}%"))
            .Select(i => i.Ingredient_Id)
            .ToList();

        return Ok(ids);
    }
}
