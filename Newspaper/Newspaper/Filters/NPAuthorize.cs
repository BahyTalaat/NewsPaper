
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newspaper.Core.Enums;
using Newspaper.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Newspaper.API.Filters
{
    public enum RolesProbability
    {
        Any = 1,
        All = 2
    }

    public class NPAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        readonly RolesProbability _Probability;
        readonly List<EnAppMainRoles> _Roles;

        private string _responseReason = "";
        private HttpStatusCode _responseCode = HttpStatusCode.Unauthorized;

        
        public NPAuthorizeAttribute(params EnAppMainRoles[] Roles)
        {
            _Roles = new List<EnAppMainRoles>(Roles);
            _Probability = RolesProbability.Any;
            this.Roles = string.Join(",", _Roles.Select(x => Enum.GetName(typeof(EnAppMainRoles), x)));
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //checking against our custom table goes here
            if (!HasWebServiceAccess(context))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        private bool HasWebServiceAccess(AuthorizationFilterContext context)
        {
            //HttpContext.Current.User.Identity                 
            var userRoles = context.HttpContext.User.Identity.GetUserRoles();
            if (!_Roles.Any())
                return true;
            else if (_Probability == RolesProbability.Any)
                return userRoles.Intersect(_Roles).Any();
            else
                return userRoles.Intersect(_Roles).Count() == _Roles.Count;
        }
    }
}
