using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newspaper.Core.Enums;
using Newspaper.Data.DbModels.SecuritySchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Data.DataContext
{
    public class SeedingIntialization
    {
        public static ApplicationDbContext _appDbContext;
        private static UserManager<ApplicationUser> _userManager;
        private static IServiceProvider _serviceProvider;

        public static string[] AppUserRolesNames = Enum.GetNames(typeof(EnAppMainRoles));
        public static EnAppMainRoles[] AppUserRolesvalues = (EnAppMainRoles[])Enum.GetValues(typeof(EnAppMainRoles));


        public static void SeedClientApp(ApplicationDbContext appDbContext, IServiceProvider serviceProvider)
        {
            _appDbContext = appDbContext;
            _appDbContext.Database.EnsureCreated();
            _serviceProvider = serviceProvider;

            var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            // call functions

            SeedApplicationRoles();
            SeedApplicationAdmin();

            // save to the database
            _appDbContext.SaveChanges();
        }

        public static long LongRandom(long min, long max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }
        private static void SeedApplicationRoles()
        {

            //Seeding client roles
            var items = _appDbContext.Roles.ToList();
            if (items == null || items.Count == 0)
            {
                //Client roles
                for (int i = 0; i < AppUserRolesNames.Length; i++)
                {
                    var EnumNumber = (int)AppUserRolesvalues[i];
                    _appDbContext.Roles.Add(new ApplicationRole()
                    {
                        Id = LongRandom(100000000000000000, 100000000000000050, new Random()),
                        Name = AppUserRolesNames[i],
                        NormalizedName = AppUserRolesNames[i].ToUpper()
                    });
                }

            }
            else
            {
                //Client roles
                for (int i = 0; i < AppUserRolesNames.Length; i++)
                {
                    var role = items.Where(e => e.Name == AppUserRolesNames[i]).FirstOrDefault();
                    if (role == null)
                    {
                        var EnumNumber = (int)AppUserRolesvalues[i];
                        _appDbContext.Roles.Add(new ApplicationRole()
                        {
                            Id = LongRandom(100000000000000000, 100000000000000050, new Random()),
                            Name = AppUserRolesNames[i],
                            NormalizedName = AppUserRolesNames[i].ToUpper()
                        });
                    }

                }
            }
            _appDbContext.SaveChanges();
        }
        private static async void SeedApplicationAdmin()
        {


            var admin = _appDbContext.Users.FirstOrDefault(u=>u.UserName == "Bahy@gmail.com");
            if (admin == null )
            {
               
                //create identity user
                var User = new ApplicationUser()
                {
                    FullName="Bahy Talaat",
                    Address="Assuit",
                    PersonalImagePath="1.png",
                    UserName = "Bahy@gmail.com",
                    Email = "Bahy@gmail.com",
                    PasswordHash="Bahy@123"
                };
                //_appDbContext.Add(User);
                //_appDbContext.SaveChanges();
                IdentityResult result = await _userManager.CreateAsync(User, "Bahy@123");
                //if (!result.Succeeded)
                //    return String.Join(",", result.Errors.Select(x => x.Description).ToList());

                // add user
                //if (!result.Succeeded)
                //{
                //    throw new Exception("Failed to save user");
                //}

                // add Role
                List<string> roles = new List<string>();
                roles.Add(EnAppMainRoles.Admin.ToString());
                roles.Add(EnAppMainRoles.Reader.ToString());
                roles.Add(EnAppMainRoles.Writer.ToString());

                var AddRoleResult = await _userManager.AddToRolesAsync(User, roles.ToArray());

                if (!AddRoleResult.Succeeded)
                {
                    throw new Exception("Failed to save role");
                }

            }
           
        }
    }
}
