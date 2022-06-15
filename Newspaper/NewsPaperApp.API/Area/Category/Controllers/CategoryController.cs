using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.API.Filters;
using Newspaper.Core.Common;
using Newspaper.Core.Enums;
using Newspaper.DTO.Category;
using Newspaper.Services.Newspaper.Category;
using NewsPaperApp.API.Controllers;
using System;
using System.Threading.Tasks;

namespace NewsPaperApp.API.Area.Category.Controllers
{
    [Route("api/[controller]")]
    [NPAuthorize(EnAppMainRoles.Admin, EnAppMainRoles.Reader, EnAppMainRoles.Writer)]
    [ApiController]

    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _categoryService = categoryService;
        }
        [HttpPost]
        [Route("AddCategory")]
        public Task<AjaxResult> Add([FromBody] CategoryDto categoryDto)
        {
            try
            {
                var result = _categoryService.add(categoryDto);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("UpdateCategory")]
        public Task<AjaxResult> UpdateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                var result = _categoryService.Update(categoryDto);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("RemoveCategory")]
        public Task<AjaxResult> RemoveCategory(long Id)
        {
            try
            {
                var result = _categoryService.remove(Id);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetCategory")]
        public Task<AjaxResult> GetCategory(long Id)
        {
            try
            {
                var result = _categoryService.Get(Id);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetAllCategories")]
        public Task<AjaxResult> GetAllCategories()
        {
            try
            {
                var result = _categoryService.GetAll();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
