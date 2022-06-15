using Autofac;
using Newspaper.Repositories.NewspaperRepositories.Article;
using Newspaper.Repositories.NewspaperRepositories.Category;
using Newspaper.Repositories.NewspaperRepositories.UserAccounts;
using Newspaper.Repositories.UOW;
using Newspaper.Services.Newspaper.Article;
using Newspaper.Services.Newspaper.Category;
using Newspaper.Services.Newspaper.Security.Account;
using Newspaper.Services.Newspaper.Security.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services
{
    public class AutoFacConfiguration :Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            #region UOW
            //Register Unit of work service
            builder.RegisterGeneric(typeof(UnitOfWork<>)).As(typeof(IUnitOfWork<>));
            #endregion

            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            
            
            builder.RegisterType<ReaderAccountRepository>().As<IReaderAccountRepository>();
            builder.RegisterType<WriterAccountRepository>().As<IWriterAccountRepository>();
            builder.RegisterType<UserService>().As<IUserService>();
            
            builder.RegisterType<AccountServices>().As<IAccountServices>();
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>();
            builder.RegisterType<ArticleServices>().As<IArticleServices>();
        }
    }
}
