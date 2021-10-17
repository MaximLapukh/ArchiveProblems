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
        private const string USERNAME_KEY = "username";
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
            return View(_db.problems.ToList());
        }
        [HttpGet]
        public IActionResult News()
        {
            return View(_db.news.ToList());
        }
        [HttpGet]
        public IActionResult Account()
        {
            if (HttpContext.Request.Cookies.TryGetValue(USERNAME_KEY, out string username))
            {               
                return View(_db.users.FirstOrDefault(u => u.name == username));
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            if (!HttpContext.Request.Cookies.ContainsKey(USERNAME_KEY)) return View();            
            else return RedirectToAction("About");
        }
        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (isCorrectUser(user) && !HttpContext.Request.Cookies.ContainsKey(USERNAME_KEY))
            {
                if (_db.users.FirstOrDefault(u => u.name == user.name) == null)
                {
                    _db.users.Add(user);
                    _db.SaveChanges();
                    return Redirect("~/Home/Result/"+result.successSignUp);
                }              
            }
            return Redirect("~/Home/Result/"+result.unsuccesSingUp);
        }
        [HttpGet]
        public IActionResult Result(int? id)
        {
            if (id == null) return RedirectToAction("About");
            return View(id);
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            if(!HttpContext.Request.Cookies.ContainsKey(USERNAME_KEY))
                return View();
            else return RedirectToAction("About");
        }
        [HttpPost]
        public IActionResult SignIn(User user)
        {
            if(isCorrectUser(user) && !HttpContext.Request.Cookies.ContainsKey(USERNAME_KEY))
            {
                var curUser = _db.users.FirstOrDefault(u => u.name == user.name);
                if (curUser!= null)
                {
                    if(curUser.password == user.password)
                    {
                        HttpContext.Response.Cookies.Append(USERNAME_KEY, user.name);
                        return RedirectToAction("Account");
                    }                        
                }                
            }
            return Redirect("~/Home/Result/"+result.unsuccesSignIn);
        }
        private bool isCorrectUser(User user) => user != null && (user.name != null && user.password != null);
    }
    enum result
    {
        unsuccesSingUp,
        successSignUp,
        unsuccesSignIn
    }
}
