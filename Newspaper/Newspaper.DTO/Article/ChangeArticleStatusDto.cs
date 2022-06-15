using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.DTO.Article
{
    public class ChangeArticleStatusDto
    {
        public long ArticleId { get; set; }
        public int Status { get; set; }
    }
}
