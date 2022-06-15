using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Data.DbModels
{
    [Table("Category", Schema = "Newspaper")]
    public class Category : BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual List<Article> articles { get; set; }
    }
}
