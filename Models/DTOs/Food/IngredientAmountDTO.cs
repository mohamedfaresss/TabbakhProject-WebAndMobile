using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Food
{
    public class IngredientAmountDTO
    {
        public string IngredientName { get; set; } = null!;
        public double? Amount { get; set; }
    }
}
