﻿using AutoMapper;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Web.ViewModels;

namespace WhatsYourIdea.Web.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterViewModel, UserDto>();
            CreateMap<LoginViewModel, UserDto>();

            CreateMap<EditorViewModel, CreateIdeaDto>();
        }
    }
}