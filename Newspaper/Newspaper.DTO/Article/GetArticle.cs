using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.DTO.Article
{
    public class GetArticle
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ArticleText { get; set; }
        public string WriterName { get; set; }
        public DateTime ArticleDate { get; set; }
        public int Status { get; set; }
        public string StatusStr { get; set; }
        public long WriterId { get; set; }

    }
}
