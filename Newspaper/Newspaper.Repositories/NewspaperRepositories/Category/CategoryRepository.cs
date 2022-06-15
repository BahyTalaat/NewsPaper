using Newspaper.Data.DataContext;
using Newspaper.Repositories.NewspaperRepositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.NewspaperRepositories.Category
{
    public class CategoryRepository: NewspaperGenericRepository<Data.DbModels.Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CategoryRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
