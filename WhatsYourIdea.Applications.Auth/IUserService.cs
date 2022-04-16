using Microsoft.AspNetCore.Identity;
using WhatsYourIdea.Applications.DTO;

namespace WhatsYourIdea.Applications.Auth
{
    public interface IUserService
    {
        Task<OperationResult<IdentityError>> CreateAsync(UserDto userDto);
    }
}