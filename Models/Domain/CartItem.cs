using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
