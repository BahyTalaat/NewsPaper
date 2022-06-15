using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.DTO.Article
{
    public class FilterArticles
    {
        public string WriterName { get; set; }
        public string OrderByProp { get; set; }
        public string Direction { get; set; }
    }
}
