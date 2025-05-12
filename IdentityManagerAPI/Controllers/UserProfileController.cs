using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTOs.User;
using Microsoft.EntityFrameworkCore;
using DataAcess.DbContexts;

namespace Graduation_project_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<UserProfileDTO>> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var allergies = await _context.UserAllergies
                .Where(a => a.UserId == user.Id)
                .Select(a => a.Name)
                .ToListAsync();

            var diseases = await _context.UserDiseases
                .Where(d => d.UserId == user.Id)
                .Select(d => d.Name)
                .ToListAsync();

            return new UserProfileDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bio = user.Bio,
                ImageId = user.ImageId,
                Allergies = allergies,
                Diseases = diseases
            };
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.Bio = dto.Bio;
            user.ImageId = dto.ImageId;

            // Remove old allergies and diseases
            var oldAllergies = _context.UserAllergies.Where(a => a.UserId == user.Id);
            var oldDiseases = _context.UserDiseases.Where(d => d.UserId == user.Id);

            _context.UserAllergies.RemoveRange(oldAllergies);
            _context.UserDiseases.RemoveRange(oldDiseases);

            // Add new ones
            foreach (var allergy in dto.Allergies)
            {
                _context.UserAllergies.Add(new UserAllergy { Name = allergy, UserId = user.Id });
            }

            foreach (var disease in dto.Diseases)
            {
                _context.UserDiseases.Add(new UserDisease { Name = disease, UserId = user.Id });
            }

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
