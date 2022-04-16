using AutoMapper;
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

        public async Task<OperationResult<IdentityError>> CreateAsync(UserDto userDto)
        {
            if(userDto is null)
                throw new ArgumentNullException(nameof(userDto));

            var appUser = _mapper.Map<ApplicationUser>(userDto);
            var result = await _userManager.CreateAsync(appUser);
            return CreateResult(result);
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