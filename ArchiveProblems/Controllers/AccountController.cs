using ArchiveProblems.Models;
using ArchiveProblems.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Controllers
{
    public class AccountController:Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private const string ADMIN_KEY = "123";
        internal const string ADMIN_ROLE_NAME = "admin";

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Account()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                return View(roles.Contains(ADMIN_ROLE_NAME));
            }
            return View();
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Incorrect password or name!");
                }
            }
            return RedirectToAction("Account");
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User() { UserName = model.Name, Email = model.Email };
                var register = await _userManager.CreateAsync(user, model.Password);
                if (register.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                }
            }
            return RedirectToAction("Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Account");
        }
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserAsync(User).Id.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Account");
        }
        [HttpPost]
        public async Task<IActionResult> GetAdmin(string adminKey)
        {
            if(adminKey == ADMIN_KEY)
            {
                var user = await _userManager.GetUserAsync(User);
                await _userManager.AddToRoleAsync(user, ADMIN_ROLE_NAME);
            }
            return RedirectToAction("Account");
        }
        [HttpPost]
        public async Task<IActionResult> ExitAdmin()
        {
           
            var user = await _userManager.GetUserAsync(User);
            await _userManager.RemoveFromRoleAsync(user, ADMIN_ROLE_NAME);
            return RedirectToAction("Account");
        }
    }
}
