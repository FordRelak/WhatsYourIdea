using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Applications.Auth
{
    public class AuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AuthService(SignInManager<ApplicationUser> signInManager, IMapper mapper, IUserService userService)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<OperationResult<string>> SignInAsync(UserAuthDto userDto)
        {
            var user = await _userService.GetUserAsync(userDto.Login);
            
            if(user is null)
            {
                return new()
                {
                    IsSuccess = false,
                    Errors = new string[] { "User not found" }
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, userDto.Password, userDto.IsRemember, false);
            return new()
            {
                IsSuccess = result.Succeeded
            };
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}