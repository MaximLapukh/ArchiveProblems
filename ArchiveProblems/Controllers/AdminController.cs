using ArchiveProblems.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Controllers
{
    [Authorize(Roles = AccountController.ADMIN_ROLE_NAME)]
    public class AdminController:Controller
    {
        private readonly ProblemsContext _db;
        public AdminController(ProblemsContext db)
        {
            _db = db;
        }    
        #region Problem
        [HttpGet]
        public IActionResult Problems()
        {            
            return View(_db.problems.ToList());        
        }
        [HttpPost]
        public IActionResult Problems(string action, int? problemId)
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
                }                   
            }
            return RedirectToAction("Problems");
        }
        [HttpGet]
        public IActionResult ProblemAdd()
        {
             return View();
        }
        [HttpPost]
        public IActionResult ProblemAdd(Problem problem)
        {
            problem.date = DateTime.Now;
            _db.problems.Add(problem);
            _db.SaveChanges();
            return RedirectToAction("Problems");
        }
        [HttpGet]
        public IActionResult ProblemEdit(int? id)
        {
            if ( id != null)
                    return View(_db.problems.FirstOrDefault(p => p.Id == id));
            else return RedirectToAction("Problems");
        }
        [HttpPost]
        public IActionResult ProblemEdit(Problem problem)
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
            return RedirectToAction("Problems");
        }
        #endregion

        #region News
        [HttpGet]
        public IActionResult News()
        {
           
             return View(_db.news.ToList());
           
        }
        [HttpPost]
        public IActionResult News(string action,int? newsId)
        {           
            if (action == "Add")
            {
                return RedirectToAction("NewsAdd");
            }
            else if (action == "Delete" && newsId != null)
            {
                var curNews = _db.news.FirstOrDefault(p => p.Id == newsId);
                if (curNews != null)
                {
                    _db.news.Remove(curNews);
                    _db.SaveChanges();                        
                }
            }
            return RedirectToAction("News");
        }
        [HttpGet]
        public IActionResult NewsAdd()
        {
             return View();
        }
        [HttpPost]
        public IActionResult NewsAdd(News news)
        {
            news.date = DateTime.Now;
            _db.news.Add(news);
            _db.SaveChanges();
            return RedirectToAction("News");
        }
        [HttpGet]
        public IActionResult NewsEdit(int? id)
        {
            if (id != null)
                return View(_db.news.FirstOrDefault(p => p.Id == id));
            else return RedirectToAction("News");
        }
        [HttpPost]
        public IActionResult NewsEdit(News news)
        {
            if (news != null)
            {
                var curNews = _db.news.FirstOrDefault(p => p.Id == news.Id);
                if (curNews != null)
                {
                    curNews.name = news.name;
                    curNews.description = news.description;
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("News");
        }
        #endregion
    }
}
