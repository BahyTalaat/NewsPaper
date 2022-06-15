using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newspaper.Core.Common;
using Newspaper.Core.Enums;
using Newspaper.Data.DbModels.SecuritySchema;
using Newspaper.DTO.Account;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Newspaper.Security.Account
{
    public class AccountServices: IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AccountServices(UserManager<ApplicationUser> userManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<AjaxResult> Login(LoginDTO loginParams)
        {
            try
            {
                var result = new AjaxResult();
                var appUser = await _userManager.FindByEmailAsync(loginParams.Email);
                if (appUser == null)
                    return "InValid Email Or Password";

                if (loginParams.IsWriter)
                {
                    if (await _userManager.IsInRoleAsync(appUser, EnAppMainRoles.Reader.ToString()) &&
                        !(await _userManager.IsInRoleAsync(appUser, EnAppMainRoles.Writer.ToString())))
                    {
                        return "InValid Email Or Password";
                    }
                }
                if (!loginParams.IsWriter)
                {
                    if (await _userManager.IsInRoleAsync(appUser, EnAppMainRoles.Writer.ToString())&&
                        !(await _userManager.IsInRoleAsync(appUser, EnAppMainRoles.Reader.ToString())))
                    {
                        return "InValid Email Or Password";
                    }
                }


                if (appUser != null &&
                 _passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, loginParams.Password) !=
                 PasswordVerificationResult.Success)
                {
                    return "InValid Email Or Password";
                }

                var token = GenerateJSONWebToken(appUser.Id, appUser.UserName);
                if (appUser.AccessFailedCount > 0)
                {
                    appUser.AccessFailedCount = 0;
                    await _userManager.UpdateAsync(appUser);
                }

                var roles = await _userManager.GetRolesAsync(appUser);
                result.AddParameter("AccessToken", token);
                result.AddParameter("FullName", $"{appUser.FullName}");
                result.AddParameter("Email", $"{appUser.Email}");
                result.AddParameter("EmailConfirmed", $"{appUser.EmailConfirmed}");
                result.AddParameter("Phone", $"{appUser.PhoneNumber}");
                result.AddParameter("PhoneConfirmed", $"{appUser.PhoneNumberConfirmed}");
                result.AddParameter("Roles", roles);
                result.AddParameter("UID", $"{appUser.Id}");

                return result;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GenerateJSONWebToken(long userId, string userName)
        {
            try
            {
                var user = _userManager.FindByIdAsync(userId.ToString()).Result;
                var signingKey = Convert.FromBase64String(_configuration["Jwt:Key"]);
                var claims = new List<Claim>
            {
                new Claim("userid", userId.ToString()),
                new Claim("FullName", user.FullName),
                new Claim("iUserID", user.Id.ToString()),
                new Claim("Email", user.Email.ToString()),
                new Claim("Phone", user.PhoneNumber != null? user.PhoneNumber.ToString() : "N/A"),
                new Claim(ClaimTypes.NameIdentifier, userName)
            };

                var roles = _userManager.GetRolesAsync(user).Result;
                foreach (var item in roles)
                {
                    var roleClaim = new Claim(ClaimTypes.Role, item);
                    claims.Add(roleClaim);
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = null,              // Not required as no third-party is involved
                    Audience = null,            // Not required as no third-party is involved
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(60000),
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                var token = jwtTokenHandler.WriteToken(jwtToken);
                return token;
            }
            catch
            {
                throw;
            }
        }

    }
}
