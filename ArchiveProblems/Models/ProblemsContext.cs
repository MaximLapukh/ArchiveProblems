using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Models
{
    public class ProblemsContext:DbContext
    {
        public DbSet<Admin> admins { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<Problem> problems { get; set; }
        public DbSet<User> users { get; set; }
        public ProblemsContext(DbContextOptions<ProblemsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
