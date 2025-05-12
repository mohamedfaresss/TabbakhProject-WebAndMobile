using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Food
{
    public class RecipeLocalizedDTO
    {
        public int RecipeId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string PreparationMethod { get; set; }

        public int Time { get; set; }

        public double Calories_100g { get; set; }
        public double Fat_100g { get; set; }
        public double Sugar_100g { get; set; }
        public double Protein_100g { get; set; }
        public double Carb_100 { get; set; }

        public string Type { get; set; }

        public List<IngredientLocalizedDTO> Ingredients { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsInCart { get; set; }
    }
}