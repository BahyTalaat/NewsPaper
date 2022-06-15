
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.API.Controllers;
using Newspaper.Core.Common;
using Newspaper.Core.Extensions;
using Newspaper.DTO.Account;
using Newspaper.Services.Newspaper.Security.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newspaper.API.Area.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountServices _accountServices;

        public AccountController(IHttpContextAccessor httpContextAccessor,
            IAccountServices accountServices) : base(httpContextAccessor)
        {
            _accountServices = accountServices;
        }

        [AllowAnonymous]
        [HttpPost, DisableRequestSizeLimit]
        [Route("Login")]
        public async Task<AjaxResult> login([FromBody] LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountServices.Login(loginDTO);
                return result;
            }
            else
            {
                return ModelState.GetMessegesErrorsSummary();
            }
        }





    }
}
