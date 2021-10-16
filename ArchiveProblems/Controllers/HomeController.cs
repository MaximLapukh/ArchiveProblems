using ArchiveProblems.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Controllers
{
    public class HomeController:Controller
    {
        public ProblemsContext _db { get; }
        public HomeController(ProblemsContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Problems()
        {
            if (HttpContext.Request.Cookies.TryGetValue("User", out string username)) ViewBag["User"] = username; 
            return View(_db.problems.ToList());
        }
    }
}
