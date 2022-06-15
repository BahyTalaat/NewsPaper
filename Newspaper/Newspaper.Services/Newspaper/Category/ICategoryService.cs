using Newspaper.Core.Common;
using Newspaper.DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Newspaper.Category
{
    public interface ICategoryService
    {
        Task<AjaxResult> add(CreateCategoryDto categoryDto,string LoggedInUserEmail);
        Task<AjaxResult> remove(long CategoryId, string LoggedInUserEmail);
        Task<AjaxResult> Update(UpdateCategoryDto updateCategoryDto, string LoggedInUserEmail);
        Task<AjaxResult> GetAll();
        Task<AjaxResult> Get(long id);
    }
}
