using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Applications.Auth
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<OperationResult<IdentityError>> CreateAsync(UserAuthDto userDto)
        {
            if(userDto is null)
                throw new ArgumentNullException(nameof(userDto));

            var appUser = _mapper.Map<ApplicationUser>(userDto);
            appUser.UserProfile = new UserProfile()
            {
                UserName = userDto.Login
            };
            var result = await _userManager.CreateAsync(appUser, userDto.Password);
            return CreateResult(result);
        }

        public async Task<ApplicationUser> GetUserAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        private static OperationResult<IdentityError> CreateResult(IdentityResult result)
        {
            return new OperationResult<IdentityError>()
            {
                IsSuccess = result.Succeeded,
                Errors = result.Errors
            };
        }
    }
}