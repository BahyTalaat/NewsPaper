using Newspaper.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.NewspaperRepositories.Category
{
    public interface ICategoryRepository: IGRepository<Data.DbModels.Category>
    {
    }
}
