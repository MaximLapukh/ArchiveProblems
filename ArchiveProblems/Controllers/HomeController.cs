using ArchiveProblems.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Controllers
{
    public class HomeController:Controller
    {
       
        public readonly ProblemsContext _db;
        public HomeController(ProblemsContext db)
        {
            _db = db;
        }
        public IActionResult About()
        {
            return View();
        }                            
        public IActionResult Result(int? id)
        {
            if (id == null) return RedirectToAction("About");
            return View(id);
        }
    }
    enum result
    {
        unsuccesSignUp = 0,
        successSignUp = 1,
        unsuccesSignIn = 2,
        rightAnswer = 3,
        wrongAnswer = 4
    }
}
