using AutoMapper;
using DataAcess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs.Food;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpPost("{recipeId}")]
    public async Task<IActionResult> AddToFavorites(int recipeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var exists = await _context.FavoriteRecipes
            .AnyAsync(f => f.UserId == user.Id && f.RecipeId == recipeId);
        if (exists) return BadRequest("Recipe already in favorites");

        var recipe = await _context.Recipe.FirstOrDefaultAsync(r => r.Recipe_Id == recipeId);
        if (recipe == null) return NotFound("Recipe not found");

        var favorite = new FavoriteRecipe
        {
            UserId = user.Id,
            RecipeId = recipeId
        };

        _context.FavoriteRecipes.Add(favorite);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Recipe '{recipe.Recipe_Name}' added to favorites successfully." });
    }

    [HttpGet]
    public async Task<IActionResult> GetFavorites()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var favorites = await _context.FavoriteRecipes
            .Where(f => f.UserId == user.Id)
            .Include(f => f.Recipe)
                .ThenInclude(r => r.Nutrition)
            .Include(f => f.Recipe)
                .ThenInclude(r => r.Recipe_Ingredient)
                    .ThenInclude(ri => ri.Ingredient)
            .AsNoTracking()
            .ToListAsync();

        // Get all recipe IDs that are in cart for this user
        var cartRecipeIds = await _context.CartItems
            .Where(c => c.UserId == user.Id)
            .Select(c => c.RecipeId)
            .ToListAsync();

        var dtos = favorites.Select(f =>
        {
            var dto = _mapper.Map<RecipeWithNutritionDTO>(f.Recipe);
            dto.AddedAt = f.AddedAt;
            dto.IsFavorite = true;
            dto.IsInCart = cartRecipeIds.Contains(f.RecipeId);
            return dto;
        }).ToList();

        return Ok(dtos);
    }

    [HttpDelete("{recipeId}")]
    public async Task<IActionResult> RemoveFromFavorites(int recipeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var favorite = await _context.FavoriteRecipes
            .FirstOrDefaultAsync(f => f.UserId == user.Id && f.RecipeId == recipeId);

        if (favorite == null) return NotFound();

        _context.FavoriteRecipes.Remove(favorite);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
