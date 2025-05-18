using Models.DTOs.Food;

public class RecipeWithNutritionDTO
{
    public int RecipeId { get; set; }
    public string Recipe_Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Preparation_Method { get; set; }
    public int Time { get; set; }
    public double Calories_100g { get; set; }
    public double Fat_100g { get; set; }
    public double Sugar_100g { get; set; }
    public double Protein_100g { get; set; }
    public double Carb_100 { get; set; }

    public List<IngredientAmountDTO> Ingredients { get; set; } = new();
    public string Type { get; set; }
    public DateTime AddedAt { get; set; }
    public string? ImageUrl { get; set; }
    public List<string> IngredientNames { get; set; }

    public bool IsFavorite { get; set; } = false;
    public bool IsInCart { get; set; } = false;

}
