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
    public class WriterAccountRepository: NewspaperGenericRepository<Writer>, IWriterAccountRepository
    {
        private readonly ApplicationDbContext _applicationDbContext; 
        public WriterAccountRepository(ApplicationDbContext applicationDbContext):base(applicationDbContext)
        {
            _applicationDbContext=applicationDbContext;
        }
    }
}
