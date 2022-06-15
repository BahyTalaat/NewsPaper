using Newspaper.Core.Common;
using Newspaper.DTO.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Newspaper.Article
{
    public interface IArticleServices
    {
        Task<AjaxResult> GetNeed(string LoggedInUserEmail);
        Task<AjaxResult> add(PostArticleDto postArticleDto, string LoggedInUserEmail);
        Task<AjaxResult> Update(UpdateArticleDto updateArticleDto, string LoggedInUserEmail);
        Task<AjaxResult> Remove(long Id,string LoggedInUserEmail);
        Task<AjaxResult> Get(long Id, string LoggedInUserEmail);
        Task<AjaxResult> GetAllActiveArticle(FilterArticles filterArticles,string LoggedInUserEmail);
        Task<AjaxResult> GetAllDeactiveArticle(FilterArticles filterArticles, string LoggedInUserEmail);
        Task<AjaxResult> GetMyArticles(FilterArticles filterArticles, string LoggedInUserEmail);
        Task<AjaxResult> ChangeArticleStatus(ChangeArticleStatusDto changeArticleStatusDto, string LoggedInUserEmail);
        Task<AjaxResult> Search(string title, string LoggedInUserEmail);
    }
}
