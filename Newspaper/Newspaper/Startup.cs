using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newspaper.Core.Extensions;
using Newspaper.Data.ConnectionStrings;
using Newspaper.Data.DataContext;
using Newspaper.Data.DbModels.SecuritySchema;
using Newspaper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper
{
    public class Startup
    {
        private string defaultNewspaperDbConnection { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
//#if LOCAL
//            defaultNewspaperDbConnection = NewspaperConnectionStrings.LocalNewspaperDbConnectionString;
//#elif DEBUG
            defaultNewspaperDbConnection = NewspaperConnectionStrings.LocalNewspaperDbConnectionString;

//#else
//            defaultNewspaperDbConnection = NewspaperConnectionStrings.ProdcutionNewspaperDbConnectionString;

//#endif
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddCors();
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
            services.AddIdentityCore<ApplicationUser>()
                                 .AddRoles<ApplicationRole>()
                                 .AddEntityFrameworkStores<ApplicationDbContext>()
                                 .AddDefaultTokenProviders()
                                 .AddTokenProvider("NewspaperApp", typeof(DataProtectorTokenProvider<ApplicationUser>));
           
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
                //options.SignIn.RequireConfirmedEmail = true;

            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion


            services.AddMvc(cfg =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                 .RequireAuthenticatedUser()
                 .Build();
                cfg.Filters.Add(new AuthorizeFilter(policy));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                //Custom bad request handler
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errorsAjaxResult = actionContext.ModelState.GetMessegesErrorsSummary();
                    var result = new JsonResult(errorsAjaxResult)
                    {
                        StatusCode = (int)HttpStatusCode.OK
                    };
                    return result;
                };
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest);
          
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                var signingKey = Convert.FromBase64String(Configuration["Jwt:Key"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (
                             context.Request.Query.TryGetValue("token", out StringValues token)
                        )
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var te = context.Exception;
                        return Task.CompletedTask;
                    }
                };
            });

            #region ConnectionString

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(defaultNewspaperDbConnection, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,

                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: new List<int>() { 19 });
            }));

            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(defaultNewspaperDbConnection));

            #endregion

            services.AddControllers();

            #region swagger configuration

            services.AddSwaggerGen(setup =>
            {
                setup.ResolveConflictingActions(x => x.First());
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Newspaper.API", Version = "v1" });
                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

            });
            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            #endregion


            #region for User Manager Dependancy Injection
            // reason for not found error when make authorize attribute
            //services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            //{
            //    options.User.RequireUniqueEmail = false;
            //})
            //  .AddEntityFrameworkStores<ApplicationDbContext>()
            //  .AddDefaultTokenProviders();
      

            services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                 .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            #endregion




            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Newspaper", Version = "v1" });
            //});




            #region Registe our services with Autofac container
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new AutoFacConfiguration());
            builder.Populate(services);
            IContainer container = builder.Build();

            #endregion

            return new AutofacServiceProvider(container);

        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ApplicationDbContext userDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IRecruiter.API v1"));
            }

            app.UseCors(builder =>
              builder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod());

            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Resources")),
            //    RequestPath = new PathString("/wwwroot/Resources")
            //});


            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            SeedingIntialization.SeedClientApp(userDbContext, serviceProvider);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStatusCodePages();
        }

    }
}
