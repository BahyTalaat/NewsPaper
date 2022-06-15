using Newspaper.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.NewspaperRepositories.Article
{
    public interface IArticleRepository: IGRepository<Data.DbModels.Article>
    {
    }
}
