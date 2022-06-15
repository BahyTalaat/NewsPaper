using Newspaper.Data.DataContext;
using Newspaper.Data.DbModels;
using Newspaper.Repositories.NewspaperRepositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.NewspaperRepositories.UserAccounts
{
    public class ReaderAccountRepository : NewspaperGenericRepository<Reader>, IReaderAccountRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ReaderAccountRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
