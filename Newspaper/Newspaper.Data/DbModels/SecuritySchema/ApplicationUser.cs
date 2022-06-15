using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Data.DbModels.SecuritySchema
{
    public class ApplicationUser:IdentityUser<long>
    {
        public override long Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PersonalImagePath { get; set; }
    }
}
