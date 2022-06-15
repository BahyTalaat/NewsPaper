using Newspaper.Data.DataContext;
using Newspaper.Repositories.NewspaperRepositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.NewspaperRepositories.Article
{
    public class ArticleRepository : NewspaperGenericRepository<Data.DbModels.Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ArticleRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
