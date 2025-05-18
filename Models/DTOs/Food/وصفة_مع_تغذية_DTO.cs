using System;
using System.Collections.Generic;
using Models.DTOs.Food; // لاستخدام IngredientAmountDTO

namespace Models.DTOs.Food.Arabic
{
    public class وصفة_مع_تغذية_DTO
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
        public string Type { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime AddedAt { get; set; }
        public List<IngredientAmountDTO> Ingredients { get; set; } = new(); // استخدام IngredientAmountDTO
        public List<string> IngredientNames { get; set; } = new();
        public bool IsFavorite { get; set; } = false;
        public bool IsInCart { get; set; } = false;
    }
}