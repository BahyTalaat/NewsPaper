using Newspaper.Core.Common;
using Newspaper.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Newspaper.Security.Account
{
    public interface IAccountServices
    {
        Task<AjaxResult> Login(LoginDTO loginParams);
        string GenerateJSONWebToken(long userId, string userName);

    }
}
