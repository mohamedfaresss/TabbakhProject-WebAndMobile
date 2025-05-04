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
public class CartController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpPost("{recipeId}")]
    public async Task<IActionResult> AddToCart(int recipeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var exists = await _context.CartItems
            .AnyAsync(c => c.UserId == user.Id && c.RecipeId == recipeId);
        if (exists) return BadRequest("Recipe already in cart");

        var recipe = await _context.Recipe.FirstOrDefaultAsync(r => r.Recipe_Id == recipeId);
        if (recipe == null) return NotFound("Recipe not found");

        var item = new CartItem
        {
            UserId = user.Id,
            RecipeId = recipeId
        };

        _context.CartItems.Add(item);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Recipe '{recipe.Recipe_Name}' added to cart successfully." });
    }

    [HttpGet]
    public async Task<IActionResult> GetCartItems()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var items = await _context.CartItems
            .Where(c => c.UserId == user.Id)
            .Include(c => c.Recipe)
                .ThenInclude(r => r.Nutrition)
            .Include(c => c.Recipe)
                .ThenInclude(r => r.Recipe_Ingredient)
                    .ThenInclude(ri => ri.Ingredient)
            .AsNoTracking()
            .ToListAsync();

        var dtos = items.Select(c =>
        {
            var dto = _mapper.Map<RecipeWithNutritionDTO>(c.Recipe);
            dto.AddedAt = c.AddedAt;
            return dto;
        }).ToList();

        return Ok(dtos);
    }

    [HttpDelete("{recipeId}")]
    public async Task<IActionResult> RemoveFromCart(int recipeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var item = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == user.Id && c.RecipeId == recipeId);

        if (item == null) return NotFound();

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
