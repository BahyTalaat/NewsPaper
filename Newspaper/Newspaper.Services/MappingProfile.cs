using AutoMapper;
using Newspaper.Data.DbModels;
using Newspaper.DTO.Article;
using Newspaper.DTO.Category;
using Newspaper.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Reader, RegisterReaderDto>().ReverseMap();
            CreateMap<Writer, RegisterWriterDto>().ReverseMap();
            CreateMap<Article, PostArticleDto>().ReverseMap();
            CreateMap<Article, UpdateArticleDto>().ReverseMap();
            CreateMap<Article, GetArticle>().ReverseMap();
            CreateMap<Category, GetCategory>().ReverseMap();
        }
    }
}
