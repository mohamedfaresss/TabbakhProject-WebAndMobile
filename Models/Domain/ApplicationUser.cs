using Microsoft.AspNetCore.Identity;
using Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string FullName { get; set; }

    public int? ImageId { get; set; }

    [ForeignKey("ImageId")]
    public Image? Image { get; set; }

    public string? Bio { get; set; }

    public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; }

    public ICollection<UserAllergy> Allergies { get; set; }
    public ICollection<UserDisease> Diseases { get; set; }
}
