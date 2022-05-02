using Microsoft.AspNetCore.Identity;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Applications.Auth
{
    public interface IUserService
    {
        Task<OperationResult<IdentityError>> CreateAsync(UserDto userDto);
        Task<ApplicationUser> GetUserAsync(string userName);
    }
}