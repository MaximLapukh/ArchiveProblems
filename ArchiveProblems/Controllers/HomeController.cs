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
        private const string USERID_KEY = "userid";
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
            if (HttpContext.Request.Cookies.TryGetValue(USERID_KEY, out string userid))
            {               
                return View(_db.users.FirstOrDefault(u => u.Id == int.Parse(userid)));
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult Account(User user,string action)
        {
            if (HttpContext.Request.Cookies.TryGetValue(USERID_KEY, out string userid))
                if (action == "Sign out")
                {               
                    HttpContext.Response.Cookies.Delete(USERID_KEY);                
                }else if(action == "Save")
                {
                    //TODO: validate data of user                
                    if (_db.users.FirstOrDefault(u => u.name == user.name&&u.Id!=int.Parse(userid)) == null)
                    {                        
                        User curUser = _db.users.FirstOrDefault(u => u.Id == int.Parse(userid));
                        curUser.name = user.name;
                        curUser.password = user.password;
                        _db.SaveChanges();
                        return RedirectToAction("Account");
                    }                
                    return Redirect("~/Home/Result/" + (int)result.unsuccesSingUp);
                }else if(action == "Delete")
                {
                    _db.users.Remove(_db.users.FirstOrDefault(u => u.Id == int.Parse(userid)));
                    _db.SaveChanges();
                    HttpContext.Response.Cookies.Delete(USERID_KEY);
                }
            return RedirectToAction("Account");
        }      
        [HttpGet]
        public IActionResult SignUp()
        {
            if (!HttpContext.Request.Cookies.ContainsKey(USERID_KEY)) return View();            
            else return RedirectToAction("About");
        }
        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (isCorrectUser(user) && !HttpContext.Request.Cookies.ContainsKey(USERID_KEY))
            {
                if (_db.users.FirstOrDefault(u => u.name == user.name) == null)
                {
                    _db.users.Add(user);
                    _db.SaveChanges();
                    return Redirect("~/Home/Result/" + (int)result.successSignUp);
                }              
            }
            return Redirect("~/Home/Result/" + (int)result.unsuccesSingUp);
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
            if(!HttpContext.Request.Cookies.ContainsKey(USERID_KEY))
                return View();
            else return RedirectToAction("About");
        }
        [HttpPost]
        public IActionResult SignIn(User user)
        {
            if(isCorrectUser(user) && !HttpContext.Request.Cookies.ContainsKey(USERID_KEY))
            {
                var curUser = _db.users.FirstOrDefault(u => u.name == user.name);
                if (curUser!= null)
                {
                    if(curUser.password == user.password)
                    {
                        HttpContext.Response.Cookies.Append(USERID_KEY, curUser.Id.ToString());
                        return RedirectToAction("Account");
                    }                        
                }                
            }
            return Redirect("~/Home/Result/"+ (int)result.unsuccesSignIn);
        }
        private bool isCorrectUser(User user) => user != null && (user.name != null && user.password != null);
    }
    enum result
    {
        unsuccesSingUp = 0,
        successSignUp = 1,
        unsuccesSignIn = 2
    }
}
