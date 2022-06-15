using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.API.Controllers;
using Newspaper.API.Filters;
using Newspaper.Core.Common;
using Newspaper.Core.Enums;
using Newspaper.DTO.Category;
using Newspaper.Services.Newspaper.Category;
using System;
using System.Threading.Tasks;

namespace Newspaper.API.Area.Category.Controllers
{
    [Route("api/[controller]")]
    [NPAuthorize(EnAppMainRoles.Admin)]
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
        public Task<AjaxResult> Add([FromBody] CreateCategoryDto categoryDto)
        {
            try
            {
                var result = _categoryService.add(categoryDto, LoggedInUserEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("UpdateCategory")]
        public Task<AjaxResult> UpdateCategory([FromBody] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                var result = _categoryService.Update(updateCategoryDto, LoggedInUserEmail);
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
                var result = _categoryService.remove(Id, LoggedInUserEmail);
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
        
        [AllowAnonymous]
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
