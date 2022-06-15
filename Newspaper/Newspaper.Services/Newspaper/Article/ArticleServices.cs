using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Newspaper.Core.Common;
using Newspaper.Core.Enums;
using Newspaper.Core.Helper;
using Newspaper.Data.DataContext;
using Newspaper.Data.DbModels.SecuritySchema;
using Newspaper.DTO.Article;
using Newspaper.Repositories.NewspaperRepositories.Article;
using Newspaper.Repositories.NewspaperRepositories.Category;
using Newspaper.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Newspaper.Article
{
    public class ArticleServices: IArticleServices
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ArticleServices(IUnitOfWork<ApplicationDbContext> unitOfWork,
            UserManager<ApplicationUser> userManager
            ,IArticleRepository articleRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<AjaxResult> GetNeed(string LoggedInUserEmail)
        {
            try
            {
                var res = new AjaxResult();
                var enums = new
                {
                    ArticleStatus = Helper.ConvertCustomEnumToList<EnArticleStatus>(false),
                };
                res.AddParameter("Enum", enums);
                return res;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> add(PostArticleDto postArticleDto, string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";

                var IsCategoryExist = _categoryRepository.Any(c => c.Id == postArticleDto.CategoryId);
                if (!IsCategoryExist)
                    return "Please select category";

                var res = new AjaxResult();
                Data.DbModels.Article article = new Data.DbModels.Article()
                {
                   Title = postArticleDto.Title,
                    ArticleText = postArticleDto.ArticleText,
                    WriterName=AppUser.FullName,
                    ArticleDate = DateTime.UtcNow,
                    WriterId=AppUser.Id,
                    Status = (int)EnArticleStatus.Active,
                    CategoryId = postArticleDto.CategoryId,
                    LMD=DateTime.UtcNow,
                    UID=AppUser.Id

                };
                _articleRepository.Add(article);
                await _unitOfWork.SaveAsync();
                res.AddParameter("Message", "Article has been added successfully");
                    return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> Get(long Id,string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";

                var res = new AjaxResult();
                Data.DbModels.Article article;
                if (await _userManager.IsInRoleAsync(AppUser,EnAppMainRoles.Admin.ToString()))
                    article = _articleRepository.GetFirst(x => x.Id == Id);
                else
                    article = _articleRepository.GetFirst(x => x.Id == Id && x.Status == (int)EnArticleStatus.Active);

                if (article == null)
                    return "Article not exist";

                var articleDto=_mapper.Map<GetArticle>(article);
                articleDto.StatusStr = ((EnArticleStatus)articleDto.Status).ToString();

                res.AddParameter("Article", articleDto);
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> GetAllActiveArticle(FilterArticles filterArticles,string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";

                var res = new AjaxResult();

                var articlelist = _articleRepository.GetAll(x=>x.Status == (int)EnArticleStatus.Active);
                
                int count = articlelist.Count();

                if (!filterArticles.WriterName.IsNullOrEmptyWithTrim())
                    articlelist=articlelist.Where(x => x.WriterName == filterArticles.WriterName);
                // sorting
                if (!(filterArticles.OrderByProp.IsNullOrEmptyWithTrim() || filterArticles.Direction.IsNullOrEmptyWithTrim()))
                {
                    if (filterArticles.Direction == "asc")
                        articlelist = articlelist.OrderBy<Data.DbModels.Article>(filterArticles.OrderByProp);
                    else if (filterArticles.Direction == "desc")
                        articlelist = articlelist.OrderByDescending<Data.DbModels.Article>(filterArticles.OrderByProp);

                }

                var articleDtolist = new List<GetArticle>();
                foreach (var article in articlelist)
                {
                    var articleDto = _mapper.Map<GetArticle>(article);
                    articleDto.StatusStr = ((EnArticleStatus)articleDto.Status).ToString();

                    articleDtolist.Add(articleDto);
                }

                res.AddParameter("count", count);
                res.AddParameter("Articles", articleDtolist);
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> GetMyArticles(FilterArticles filterArticles, string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";

                var res = new AjaxResult();

                var articlelist = _articleRepository.GetAll(x=>x.WriterId == AppUser.Id);

                int count=articlelist.Count();
                // sorting
                if (!(filterArticles.OrderByProp.IsNullOrEmptyWithTrim() || filterArticles.Direction.IsNullOrEmptyWithTrim()))
                {
                    if (filterArticles.Direction == "asc")
                        articlelist = articlelist.OrderBy<Data.DbModels.Article>(filterArticles.OrderByProp);
                    else if (filterArticles.Direction == "desc")
                        articlelist = articlelist.OrderByDescending<Data.DbModels.Article>(filterArticles.OrderByProp);

                }
                var articleDtolist = new List<GetArticle>();
                foreach (var article in articlelist)
                {
                    var articleDto = _mapper.Map<GetArticle>(article);
                    articleDto.StatusStr = ((EnArticleStatus)articleDto.Status).ToString();

                    articleDtolist.Add(articleDto);
                }

                res.AddParameter("count", count);
                res.AddParameter("Article", articleDtolist);
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> Remove(long Id, string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";

                var res = new AjaxResult();

                var article = _articleRepository.GetFirst(x => x.Id == Id);
                if (article == null)
                    return "Article not exist";

                if(!await _userManager.IsInRoleAsync(AppUser, EnAppMainRoles.Admin.ToString()))
                {
                    if (article.WriterId != AppUser.Id)
                    {
                        return "Can not delete this article";
                    }
                }
                

                _articleRepository.Remove(article);
                await _unitOfWork.SaveAsync();
                res.AddParameter("Message", "Article has been removed successfully");
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> Update(UpdateArticleDto updateArticleDto, string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";
                var article = await _articleRepository.GetFirstAsync(x => x.Id == updateArticleDto.Id);
                if (article == null)
                    return "Article not exist";

                var IsCategoryExist = _categoryRepository.Any(c => c.Id == updateArticleDto.CategoryId);
                if (!IsCategoryExist)
                    return "Please select category";

                if (!await _userManager.IsInRoleAsync(AppUser, EnAppMainRoles.Admin.ToString()))
                {
                    if (article.WriterId != AppUser.Id)
                    {
                        return "Can not update this article";
                    }
                }

                var res = new AjaxResult();
                article = _mapper.Map<Data.DbModels.Article>(updateArticleDto);


                article.WriterName = AppUser.FullName;
                article.ArticleDate = DateTime.UtcNow;
                article.Status = (int)EnArticleStatus.Active;
                article.WriterId = AppUser.Id;
                article.UID = AppUser.Id;
                article.LMD = DateTime.UtcNow;


                _articleRepository.Update(article);
                await _unitOfWork.SaveAsync();
                res.AddParameter("Message", "Article has been Updated successfully");
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> ChangeArticleStatus(ChangeArticleStatusDto changeArticleStatusDto, string LoggedInUserEmail)
        {
            try
            {
                var res = new AjaxResult();
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";
                var article = await _articleRepository.GetFirstAsync(x => x.Id == changeArticleStatusDto.ArticleId);
                if (article == null)
                    return "Article not exist";

                if (changeArticleStatusDto.Status != (int)EnArticleStatus.Active &&
                    changeArticleStatusDto.Status != (int)EnArticleStatus.Deactive)
                    return "Wrong Status";

                article.Status=changeArticleStatusDto.Status;

                _articleRepository.Update(article);
                await _unitOfWork.SaveAsync();
                res.AddParameter("Message", "Article status has been Updated successfully");
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> GetAllDeactiveArticle(FilterArticles filterArticles, string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";

                var res = new AjaxResult();

                var articlelist = _articleRepository.GetAll(x => x.Status == (int)EnArticleStatus.Deactive);
                int count = articlelist.Count();

                if (!filterArticles.WriterName.IsNullOrEmptyWithTrim())
                    articlelist = articlelist.Where(x => x.WriterName == filterArticles.WriterName);
                // sorting
                if (!(filterArticles.OrderByProp.IsNullOrEmptyWithTrim() || filterArticles.Direction.IsNullOrEmptyWithTrim()))
                {
                    if (filterArticles.Direction == "asc")
                        articlelist = articlelist.OrderBy<Data.DbModels.Article>(filterArticles.OrderByProp);
                    else if (filterArticles.Direction == "desc")
                        articlelist = articlelist.OrderByDescending<Data.DbModels.Article>(filterArticles.OrderByProp);

                }

                var articleDtolist = new List<GetArticle>();
                foreach (var article in articlelist)
                {
                    var articleDto = _mapper.Map<GetArticle>(article);
                    articleDto.StatusStr = ((EnArticleStatus)articleDto.Status).ToString();

                    articleDtolist.Add(articleDto);
                }

                res.AddParameter("count", count);
                res.AddParameter("Articles", articleDtolist);
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> Search(string title, string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync((LoggedInUserEmail));
                if (AppUser == null)
                    return "User not exist";

                var res = new AjaxResult();

                var articlelist = _articleRepository.GetAll(x => (
                x.Title.ToLower().Contains(title.ToLower()) || title.ToLower().Contains(x.Title.ToLower()) 
                || x.Title.ToLower().StartsWith(title.ToLower()) || title.ToLower().StartsWith(x.Title.ToLower()) 
                ||x.Title.ToLower().EndsWith(title.ToLower()) || title.ToLower().EndsWith(x.Title.ToLower()))
                && x.Status == (int)EnArticleStatus.Active
                );
                

                var articleDtolist = new List<GetArticle>();
                foreach (var article in articlelist)
                {
                    var articleDto = _mapper.Map<GetArticle>(article);
                    articleDto.StatusStr = ((EnArticleStatus)articleDto.Status).ToString();

                    articleDtolist.Add(articleDto);
                }

                res.AddParameter("Articles", articleDtolist);
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
