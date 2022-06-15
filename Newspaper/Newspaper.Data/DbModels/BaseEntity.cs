using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Data.DbModels
{
    public abstract class BaseEntity
    {
        public long UID { get; set; }
        public DateTime? LMD { get; set; }
        public bool IsDeleted { get; set; }
    }
}
