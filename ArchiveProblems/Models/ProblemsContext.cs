using ArchiveProblems.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Models
{
    public class ProblemsContext: IdentityDbContext<User>
    {
        public DbSet<Admin> admins { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<Problem> problems { get; set; }
        public ProblemsContext(DbContextOptions<ProblemsContext> options) : base(options)
        {
            Database.EnsureCreated();
            if(Roles.FirstOrDefault(r=>r.Name == AccountController.ADMIN_ROLE_NAME) == null)
            {
                Roles.Add(new IdentityRole() { Name = AccountController.ADMIN_ROLE_NAME, NormalizedName = AccountController.ADMIN_ROLE_NAME.ToUpper() });
                SaveChanges();
            }
        }
    }
}
