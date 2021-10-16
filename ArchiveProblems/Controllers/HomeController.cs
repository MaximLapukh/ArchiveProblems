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
        [HttpGet]
        public IActionResult Problems()
        {
            if (HttpContext.Request.Cookies.TryGetValue("User", out string username)) ViewBag["User"] = username; 
            return View(_db.problems.ToList());

        }
        [HttpGet]
        public IActionResult News()
        {
            return View(_db.news.ToList());
        }
    }
}
