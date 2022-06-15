using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.DTO.Article
{
    public class PostArticleDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string ArticleText { get; set; }
        public long CategoryId { get; set; }
    }
}
