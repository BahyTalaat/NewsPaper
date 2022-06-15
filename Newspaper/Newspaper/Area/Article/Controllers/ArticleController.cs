using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.API.Controllers;
using Newspaper.API.Filters;
using Newspaper.Core.Common;
using Newspaper.Core.Enums;
using Newspaper.DTO.Article;
using Newspaper.DTO.Category;
using Newspaper.Services.Newspaper.Article;
using System;
using System.Threading.Tasks;

namespace Newspaper.API.Area.Article.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class ArticleController : BaseController
    {
        private readonly IArticleServices _articleServices;

        public ArticleController(IHttpContextAccessor httpContextAccessor,IArticleServices articleServices):base(httpContextAccessor)
        {
            _articleServices = articleServices;
        }

        [NPAuthorize(EnAppMainRoles.Admin, EnAppMainRoles.Writer)]
        [HttpGet]
        [Route("GetNeed")]
        public Task<AjaxResult> GetNeed()
        {
            try
            {
                var result = _articleServices.GetNeed(LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NPAuthorize(EnAppMainRoles.Admin, EnAppMainRoles.Writer)]
        [HttpPost]
        [Route("AddArticle")]
        public Task<AjaxResult> AddArticle([FromBody] PostArticleDto postArticleDto)
        {
            try
            {
                var result = _articleServices.add(postArticleDto,LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NPAuthorize(EnAppMainRoles.Admin, EnAppMainRoles.Writer)]
        [HttpPost]
        [Route("UpdateArticle")]
        public Task<AjaxResult> UpdateArticle([FromBody] UpdateArticleDto updateArticleDto)
        {
            try
            {
                var result = _articleServices.Update(updateArticleDto, LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NPAuthorize(EnAppMainRoles.Admin, EnAppMainRoles.Writer)]
        [HttpDelete]
        [Route("RemoveArticle")]
        public Task<AjaxResult> RemoveArticle(long Id)
        {
            try
            {
                var result = _articleServices.Remove(Id, LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetArticle")]
        public Task<AjaxResult> GetArticle(long Id)
        {
            try
            {
                var result = _articleServices.Get(Id,LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetAllArticle")]
        public Task<AjaxResult> GetAllArticles([FromBody]FilterArticles filterArticles)
        {
            try
            {
                var result = _articleServices.GetAllActiveArticle(filterArticles,LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NPAuthorize(EnAppMainRoles.Admin, EnAppMainRoles.Writer)]
        [HttpPost]
        [Route("GetMyArticles")]
        public Task<AjaxResult> GetMyArticles([FromBody] FilterArticles filterArticles)
        {
            try
            {
                var result = _articleServices.GetMyArticles(filterArticles,LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NPAuthorize(EnAppMainRoles.Admin)]
        [HttpPost]
        [Route("ChangeArticleStatus")]
        public Task<AjaxResult> ChangeArticleStatus(ChangeArticleStatusDto changeArticleStatusDto)
        {
            try
            {
                var result = _articleServices.ChangeArticleStatus(changeArticleStatusDto,LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [NPAuthorize(EnAppMainRoles.Admin)]
        [HttpPost]
        [Route("GetAllDeactiveArticle")]
        public Task<AjaxResult> GetAllDeactiveArticle([FromBody] FilterArticles filterArticles)
        {
            try
            {
                var result = _articleServices.GetAllDeactiveArticle(filterArticles,LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("Search")]
        public Task<AjaxResult> Search(string title)
        {
            try
            {
                var result = _articleServices.Search(title, LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
