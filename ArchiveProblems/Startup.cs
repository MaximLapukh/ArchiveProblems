using ArchiveProblems.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiveProblems
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
   
            services.AddMvc();
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ProblemsContext>(options => options.UseSqlServer(connection));

            var options = new DbContextOptionsBuilder<ProblemsContext>().UseSqlServer(connection).Options;
            DataInit(options);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=About}/{id?}");
            });
        }
        private void DataInit(DbContextOptions<ProblemsContext> options)
        {
            using (var db = new ProblemsContext(options))
            {
                if (!db.admins.Any()) db.admins.Add(new Admin() { name = "root", password = "123" });
                if (!db.problems.Any()) db.problems.Add(
                    new Problem()
                    {
                        name = "Amicable numbers",
                        description = "Let d(n) be defined as the sum of proper divisors of n (numbers less than n which divide evenly into n)." +
                         " If d(a) = b and d(b) = a, where a ≠ b, then a and b are an amicable pair and each of a and b are called amicable numbers." +
                         " For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110; therefore d(220) = 284. " +
                         "The proper divisors of 284 are 1, 2, 4, 71 and 142; so d(284) = 220." +
                         " Evaluate the sum of all the amicable numbers under 10000.",
                        answer = 31626,
                        date = DateTime.Now
                    });
                if (!db.news.Any()) db.news.Add(new News() { name = "Server had started!", description = "Server had started!!!", date = DateTime.Now });
                db.SaveChanges();
            }
        }
    }
}
