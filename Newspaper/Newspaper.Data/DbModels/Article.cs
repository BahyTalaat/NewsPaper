using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Data.DbModels
{
    [Table("Article", Schema = "Newspaper")]
    public class Article : BaseEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ArticleText { get; set; }
        public string WriterName { get; set; }
        public DateTime ArticleDate { get; set; }
        public int Status { get; set; }

        // relations
        public long WriterId { get; set; }

        [ForeignKey("category")]
        public long CategoryId { get; set; }
        public Category  category { get; set; }
    }
}
