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
    public class ProblemsController:Controller
    {
        public readonly ProblemsContext _db;
        public ProblemsController(ProblemsContext db)
        {
            _db = db;
        }
        public IActionResult All()
        {
            User user = null;

            if (HttpContext.Request.Cookies.TryGetValue(helper.USERID_KEY, out string userid))
                user = _db.users.Include(u => u.solvedProblems).FirstOrDefault(u => u.Id == int.Parse(userid));

            if (user != null) ViewBag.solvedProblems = user.solvedProblems;
            return View(_db.problems.ToList());
        }
        [HttpGet]
        public IActionResult Problem(int? id)
        {
            if (id != null)
            {
                var curProblem = _db.problems.FirstOrDefault(p => p.Id == id);
                if (curProblem != null)
                {
                    if (HttpContext.Request.Cookies.TryGetValue(helper.USERID_KEY, out string userid))
                    {
                        var user = _db.users.Include(u => u.solvedProblems).FirstOrDefault(u => u.Id == int.Parse(userid));
                        ViewBag.signIn = true;
                        ViewBag.userid = user.Id;
                        ViewBag.solved = user.solvedProblems.Contains(curProblem);
                    }
                    else
                    {
                        ViewBag.signIn = false;
                    }
                    return View(curProblem);
                }
            }
            return RedirectToAction("All");
        }
        [HttpPost]
        public IActionResult Problem(int userid, int problemid, int answer)
        {
            var curProblem = _db.problems.FirstOrDefault(p => p.Id == problemid);
            if (curProblem != null && curProblem.answer == answer)
            {
                var curUser = _db.users.Include(u => u.solvedProblems).FirstOrDefault(u => u.Id == userid);
                curUser.solvedProblems.Add(curProblem);
                _db.SaveChanges();

                return Redirect("~/Home/Result/" + (int)result.rightAnswer);
            }
            return Redirect("~/Home/Result/" + (int)result.wrongAnswer);
        }       
    }
}
