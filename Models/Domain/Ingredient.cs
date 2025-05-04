using System;
using System.Collections.Generic;

namespace Models.Domain
{
    public partial class Ingredient
    {
        public int Ingredient_Id { get; set; }

        public string? Ingredient_Name { get; set; }
        public ICollection<Recipe_Ingredient> Recipe_Ingredient { get; set; }
    }
}