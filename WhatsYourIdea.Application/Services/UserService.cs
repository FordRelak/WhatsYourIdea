using Microsoft.AspNetCore.Identity;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Application
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userRepository;

        public UserService(UserManager<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
