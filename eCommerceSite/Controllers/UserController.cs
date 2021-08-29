using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductContext _context;

        public UserController(ProductContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Map Data to user account object !This step is for ViewModels
                UserAccount account = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    UserName = reg.UserName
                };
                // Add to database
                _context.UserAccounts.Add(account);
                await _context.SaveChangesAsync();
                // Say Hello!
                TempData["Message"] = $"Hello {reg.UserName}!";
                // Redirect to home
                return RedirectToAction("Index", "Home");
            }
            return View(reg);
        }
    }
}
