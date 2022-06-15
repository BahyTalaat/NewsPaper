using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Newspaper.Core.Common;
using Newspaper.Data.DataContext;
using Newspaper.Data.DbModels.SecuritySchema;
using Newspaper.DTO.Category;
using Newspaper.Repositories.NewspaperRepositories.Category;
using Newspaper.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Newspaper.Category
{
    public class CategoryService:ICategoryService
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IUnitOfWork<ApplicationDbContext> unitOfWork,
            UserManager<ApplicationUser> userManager,
            IMapper mapper
            ,ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<AjaxResult> add(CreateCategoryDto categoryDto, string LoggedInUserEmail)

        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync(LoggedInUserEmail);
                if (AppUser == null)
                    return "User not exist";
                var res = new AjaxResult();
                //var IsCategoryExist = _categoryRepository.Any(c => c.Name.Equals(categoryDto.Name, StringComparison.InvariantCultureIgnoreCase));
                var IsCategoryExist = _categoryRepository.Any(c => c.Name == categoryDto.Name);
                if (IsCategoryExist)
                    return "Category Already Exist";
                await _categoryRepository.AddAsync(new Data.DbModels.Category
                {
                    Name = categoryDto.Name,
                    LMD=DateTime.UtcNow,
                    UID=AppUser.Id
                });
                await _unitOfWork.SaveAsync();
                res.AddParameter("Message", "Category added successfully");
                return res;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AjaxResult> Get(long id)
        {
            try
            {
                var res = new AjaxResult();
                var category = await _categoryRepository.GetFirstAsync(c => c.Id==id);
                if (category == null)
                    return "Category not exist";
                var categoryDto =_mapper.Map<GetCategory>(category);
                res.AddParameter("Category", categoryDto);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AjaxResult> GetAll()
        {
            try
            {
                var res = new AjaxResult();
                var categoryList = _mapper.Map<List<GetCategory>>(_categoryRepository.GetAll());
                res.AddParameter("CategoryList", categoryList);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AjaxResult> remove(long CategoryId, string LoggedInUserEmail)
        {
            try
            {
                var AppUser = await _userManager.FindByEmailAsync(LoggedInUserEmail);
                if (AppUser == null)
                    return "User not exist";

                var res = new AjaxResult();
                var category = await _categoryRepository.GetFirstAsync(c => c.Id == CategoryId);
                if (category == null)
                    return "Category Not exist";
                 _categoryRepository.Remove(category);
                await _unitOfWork.SaveAsync();
                res.AddParameter("Message", "Category has been deleted successfully");
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AjaxResult> Update(UpdateCategoryDto updateCategoryDto, string LoggedInUserEmail)
        {
            try
            {
                var res = new AjaxResult();

                var AppUser = await _userManager.FindByEmailAsync(LoggedInUserEmail);
                if (AppUser == null)
                    return "User not exist";

                var category = await _categoryRepository.GetFirstAsync(c => c.Id == updateCategoryDto.Id);
                if (category == null)
                    return "Category Not exist";

                category.Name = updateCategoryDto.Name;
                category.LMD = DateTime.UtcNow;
                category.UID = AppUser.Id;

                _categoryRepository.Update(category);
                await _unitOfWork.SaveAsync();
                res.AddParameter("Message", "Category has been Updated successfully");
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
