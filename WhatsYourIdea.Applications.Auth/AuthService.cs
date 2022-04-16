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

        public AuthService(SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task SignInAsync(UserDto userDto)
        {
            var appUser = _mapper.Map<ApplicationUser>(userDto);
            await _signInManager.SignInAsync(appUser, userDto.IsRemember);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}