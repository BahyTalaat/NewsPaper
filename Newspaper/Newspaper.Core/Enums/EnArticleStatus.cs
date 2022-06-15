using Newspaper.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Core.Enums
{
    public enum EnArticleStatus
    {
        [CustomEnum(DisplayText = "Active")]
        Active =1,
        [CustomEnum(DisplayText = "Deactive")]
        Deactive =2
    }
}
