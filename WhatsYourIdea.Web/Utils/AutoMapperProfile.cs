using AutoMapper;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Web.ViewModels;

namespace WhatsYourIdea.Web.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterViewModel, UserAuthDto>();
            CreateMap<LoginViewModel, UserAuthDto>();

            CreateMap<EditorViewModel, CreateIdeaDto>();
        }
    }
}