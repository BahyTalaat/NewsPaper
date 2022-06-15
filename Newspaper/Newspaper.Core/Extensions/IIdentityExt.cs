
using Newspaper.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Core.Extensions
{
    public static class IIdentityExt
    {
        public static string GetFullName(this IIdentity identity)
        {
            var Claims = ((ClaimsIdentity)identity).Claims;
            var fullNameClaim = Claims.FirstOrDefault(x => x.Type == "FullName");
            if (fullNameClaim != null)
                return fullNameClaim.Value;

            return "";
        }
        public static long GetUserID(this IIdentity identity)
        {
            var Claims = ((ClaimsIdentity)identity).Claims;
            var UserIDClaim = Claims.FirstOrDefault(x => x.Type == "Id");
            if (UserIDClaim != null)
                return long.Parse(UserIDClaim.Value);

            return 0;
        }
        public static string GetUserEmail(this IIdentity identity)
        {
            var Claims = ((ClaimsIdentity)identity).Claims;
            var iUserIDClaim = Claims.FirstOrDefault(x => x.Type == "Email");
            if (iUserIDClaim != null)
                return iUserIDClaim.Value;

            return "";
        }
        public static List<EnAppMainRoles> GetUserRoles(this IIdentity identity)
        {
            List<string> agzRoles = new List<string>();

            List<EnAppMainRoles> roles = ((ClaimsIdentity)identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => (EnAppMainRoles)Enum.Parse(typeof(EnAppMainRoles), c.Value)).ToList();
            return roles;
        }

    }
}
