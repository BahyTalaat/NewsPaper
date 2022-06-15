using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newspaper.Data.ConnectionStrings;
using Newspaper.Data.DbModels;
using Newspaper.Data.DbModels.SecuritySchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Data.DataContext
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(NewspaperConnectionStrings.LocalNewspaperDbConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users", "Security");
            builder.Entity<ApplicationRole>().ToTable("Roles", "Security");

        }

        public virtual DbSet<Writer> Writers { get; set; }
        public virtual DbSet<Reader> Readers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
    }
}
