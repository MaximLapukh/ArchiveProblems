using ArchiveProblems.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Controllers
{
    public class AdminController:Controller
    {
        private readonly ProblemsContext _db;

        public AdminController(ProblemsContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            if (!HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY))
                return View();
            else return RedirectToAction("Problems");
        }

        [HttpPost]
        public IActionResult SignIn(Admin admin)
        {
            if (admin != null)
            {
                var curAdmin = _db.admins.FirstOrDefault(a => a.name == admin.name);
                if (curAdmin != null && curAdmin.password == admin.password)
                {
                    HttpContext.Response.Cookies.Append(helper.ADMIN_KEY, "1");
                    return RedirectToAction("Problems");
                }
            }
            return RedirectToAction("SignIn");
        }
        [HttpPost]
        public IActionResult Exit()
        {
            if(HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY))
            {
                HttpContext.Response.Cookies.Delete(helper.ADMIN_KEY);
            }
            return RedirectToAction("SignIn");
        }
        [HttpGet]
        public IActionResult Problems()
        {
            if (HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY))
            {
                return View(_db.problems.ToList());
            }
            return RedirectToAction("SignIn");
        
        }
        [HttpPost]
        public IActionResult Problems(string action,int? problemId)
        {
            if (HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY))
            {
                if (action == "Add")
                {
                    return RedirectToAction("ProblemAdd");
                }
                else if (action == "Delete" && problemId != null)
                {
                    var curProblem = _db.problems.FirstOrDefault(p => p.Id == problemId);
                    if (curProblem != null)
                    {
                        _db.problems.Remove(curProblem);
                        _db.SaveChanges();
                        return RedirectToAction("Problems");
                    }                   
                }
            }
            return RedirectToAction("SignIn");
        }
        [HttpGet]
        public IActionResult ProblemAdd()
        {
            if (HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY))
            {
                return View();
            }
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public IActionResult ProblemAdd(Problem problem)
        {
            if (HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY) && problem != null)
            {
                problem.date = DateTime.Now;
                _db.problems.Add(problem);
                _db.SaveChanges();
                return RedirectToAction("Problems");
            }
            return RedirectToAction("SignIn");
        }
        [HttpGet]
        public IActionResult ProblemEdit(int? id)
        {
            if (HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY))
            {
                if ( id != null)
                     return View(_db.problems.FirstOrDefault(p => p.Id == id));
                else return RedirectToAction("Problems");
            }
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public IActionResult ProblemEdit(Problem problem)
        {
            if (HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY))
            {
                if (problem != null) {
                    var curProblem = _db.problems.FirstOrDefault(p => p.Id == problem.Id);
                    if (curProblem != null)
                    {
                        curProblem.name = problem.name;
                        curProblem.description = problem.description;
                        curProblem.answer = problem.answer;
                        _db.SaveChanges();
                    }
                }
                else return RedirectToAction("Problems");
            }
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult News()
        {
            if (HttpContext.Request.Cookies.ContainsKey(helper.ADMIN_KEY))
            {
                return View(_db.news.ToList());
            }
            return RedirectToAction("SignIn");       
        }

    }
}
