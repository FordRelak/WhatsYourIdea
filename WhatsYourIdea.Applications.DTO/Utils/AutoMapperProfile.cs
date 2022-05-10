using AutoMapper;
using Domain.Entities;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Applications.DTO.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserAuthDto, ApplicationUser>()
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Login));

            CreateMap<CreateIdeaDto, Idea>()
                .ForMember(dst => dst.ShortDescription, opt => opt.MapFrom(src => src.SubTitle))
                .ForMember(dst => dst.FullDesctiption, opt => opt.MapFrom(src => src.Text))
                .ReverseMap();

            CreateMap<Idea, IdeaDto>()
                .ForMember(dst => dst.CommentNumber, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dst => dst.TrackedNumber, opt => opt.MapFrom(src => src.TrackingUsers.Count))
                .ForMember(dst => dst.SubTitle, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dst => dst.CreateDate, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.AuthorName, opt => opt.MapFrom(src => src.Author.UserProfile.UserName));

            CreateMap<Idea, IdeaDetailedDto>()
                .ForMember(dst => dst.CommentNumber, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dst => dst.TrackedNumber, opt => opt.MapFrom(src => src.TrackingUsers.Count))
                .ForMember(dst => dst.SubTitle, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dst => dst.CreateDate, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Author, opt => opt.MapFrom(src => src.Author.UserProfile));

            CreateMap<Comment, CommentDto>();

            CreateMap<UserProfile, UserDto>()
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Author.UserProfile.UserName))
                .ForMember(dst => dst.AvatarFilePath, opt => opt.MapFrom(src => src.Author.UserProfile.AvatarFilePath));

            CreateMap<Tag, TagDto>();
        }
    }
}