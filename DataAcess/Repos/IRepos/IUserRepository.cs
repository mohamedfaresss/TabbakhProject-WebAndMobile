using Models.DTOs.Auth;
using Models.DTOs.User;

namespace DataAcess.Repos.IRepos
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByID(string userID);
        Task<bool> IsUniqueUserName(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
        Task<string> ForgotPasswordAsync(ForgotPasswordRequestDTO forgotPasswordRequestDTO);
        Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<bool> UpdateAsync(ApplicationUser user);
        Task<ApplicationUser> FindByEmailAsync(string email);
    }
}