using AutoMapper;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Applications.DTO.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDto, ApplicationUser>();
        }
    }
}