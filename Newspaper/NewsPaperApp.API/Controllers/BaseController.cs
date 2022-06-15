
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPaperApp.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string LoggedInUserEmail { get { return User.Identity.GetUserEmail(); } }
        
    }
}
