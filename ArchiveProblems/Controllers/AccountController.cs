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
    public class AccountController:Controller
    {
        public readonly ProblemsContext _db;
        public AccountController(ProblemsContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Account()
        {
            if (HttpContext.Request.Cookies.TryGetValue(helper.USERID_KEY, out string userid))
            {
                return View(_db.users.Include(u => u.solvedProblems).FirstOrDefault(u => u.Id == int.Parse(userid)));
            }
            else
            {
                return View("Null",null);
            }
        }
        [HttpPost]
        public IActionResult Account(User user, string action)
        {
            if (HttpContext.Request.Cookies.TryGetValue(helper.USERID_KEY, out string userid))
                if (action == "Sign out")
                {
                    HttpContext.Response.Cookies.Delete(helper.USERID_KEY);                    
                }
                else if (action == "Save")
                {                      
                    if (helper.isCorrectUser(user) && _db.users.FirstOrDefault(u => u.name == user.name && u.Id != int.Parse(userid)) == null)
                    {
                        User curUser = _db.users.FirstOrDefault(u => u.Id == int.Parse(userid));
                        curUser.name = user.name;
                        curUser.password = user.password;
                        _db.SaveChanges();
                        return RedirectToAction("Account");
                    }
                    return Redirect("~/Home/Result/" + (int)result.unsuccesSignUp);
                }
                else if (action == "Delete")
                {
                    _db.users.Remove(_db.users.FirstOrDefault(u => u.Id == int.Parse(userid)));
                    _db.SaveChanges();
                    HttpContext.Response.Cookies.Delete(helper.USERID_KEY);
                }
            return RedirectToAction("Account");
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            if (!HttpContext.Request.Cookies.ContainsKey(helper.USERID_KEY))
                return View();
            else return RedirectToAction("Account");
        }
        [HttpPost]
        public IActionResult SignIn(User user)
        {
            if (helper.isCorrectUser(user) && !HttpContext.Request.Cookies.ContainsKey(helper.USERID_KEY))
            {
                var curUser = _db.users.FirstOrDefault(u => u.name == user.name);
                if (curUser != null)
                {
                    if (curUser.password == user.password)
                    {
                        HttpContext.Response.Cookies.Append(helper.USERID_KEY, curUser.Id.ToString());
                        return RedirectToAction("Account");
                    }
                }
            }
            return Redirect("~/Home/Result/" + (int)result.unsuccesSignIn);
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            if (!HttpContext.Request.Cookies.ContainsKey(helper.USERID_KEY)) return View();
            else return RedirectToAction("Account");
        }
        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (helper.isCorrectUser(user) && !HttpContext.Request.Cookies.ContainsKey(helper.USERID_KEY))
            {
                if (_db.users.FirstOrDefault(u => u.name == user.name) == null)
                {
                    _db.users.Add(user);
                    _db.SaveChanges();
                    return Redirect("~/Home/Result/" + (int)result.successSignUp);
                }
            }
            return Redirect("~/Home/Result/" + (int)result.unsuccesSignUp);
        }
    }
}
