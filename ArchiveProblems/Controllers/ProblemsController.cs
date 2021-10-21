using ArchiveProblems.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        public ProblemsController(ProblemsContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> All()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null) 
                ViewBag.solvedProblems = _db.Users.Include(u=>u.solvedProblems)
                    .FirstOrDefault(u=>u.Id==user.Id).solvedProblems;
            return View(_db.problems.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> Problem(int? id)
        {           
            if (id != null)
            {
                var curProblem = _db.problems.FirstOrDefault(p => p.Id == id);
                if (curProblem != null)
                {                  
                    var user = await _userManager.GetUserAsync(User);
                    var solvedProblems = _db.Users.Include(u => u.solvedProblems)
                    .FirstOrDefault(u => u.Id == user.Id).solvedProblems;

                    ViewBag.solved = solvedProblems != null && solvedProblems.Contains(curProblem);
                    
                    return View(curProblem);
                }
            }
            return RedirectToAction("All");
        }
        [HttpPost]
        public async Task<IActionResult> CheckAsnwer(int problemId, int answer)
        {           
            var curProblem = _db.problems.FirstOrDefault(p => p.Id == problemId);
            if (curProblem !=null && curProblem.answer == answer)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user.solvedProblems == null) user.solvedProblems = new List<Problem>();
                user.solvedProblems.Add(curProblem);
                _db.SaveChanges();

                return Redirect("~/Home/Result/" + (int)result.rightAnswer);
            }
            return Redirect("~/Home/Result/" + (int)result.wrongAnswer);
        }       
    }
}
