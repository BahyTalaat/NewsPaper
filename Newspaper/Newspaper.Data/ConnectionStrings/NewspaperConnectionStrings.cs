using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Data.ConnectionStrings
{
    public static class NewspaperConnectionStrings
    {
        //public const string SchemaName = "dbo";

        public static string LocalNewspaperDbConnectionString = "data source=.;initial catalog=Newspaper ; integrated security =true;";
         public static string DevNewspaperDbConnectionString = "";
         public static string ProdcutionNewspaperDbConnectionString = "";

    }
}
