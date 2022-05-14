using Microsoft.AspNetCore.Identity;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Applications.Auth
{
    public interface IUserService
    {
        Task<OperationResult<IdentityError>> CreateAsync(UserAuthDto userDto);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<ApplicationUser> GetUserAsync(string userName);
        Task DeleteUser(string username);
    }
}