﻿using AutoMapper;
using NewsWebAPI.Entities;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Article, ArticleModal>().ReverseMap();
            CreateMap<User, UserModal>().ReverseMap();
        }
    }
}