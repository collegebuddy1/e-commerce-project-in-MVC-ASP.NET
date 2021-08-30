using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                // Can refactor this later MAKE A ISSUE! delete this if you made the issue ^.^
                // Check if Email is unique
                bool isEmailTaken = await (from acc in _context.UserAccounts
                                           where acc.Email == reg.Email
                                           select acc).AnyAsync();

                // If email is not Unique error and send abck to view
                if (isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "That email is already in use.");
                }

                // Check for Username
                bool isUsernameTaken = await (from acc in _context.UserAccounts
                                           where acc.UserName == reg.UserName
                                           select acc).AnyAsync();

                // If username is not Unique error and send abck to view
                if (isUsernameTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.UserName), "That username is already in use.");
                }
                if (isEmailTaken || isUsernameTaken)
                {
                    return View(reg);
                }

                // Map Data to user account object !!This step is for ViewModels!!
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

                // Log user in
                LogUserIn(account.UserID);

                // Say Hello!
                TempData["Message"] = $"Thank you for registering an account with us {reg.UserName}!";

                // Redirect to home
                return RedirectToAction("Index", "Home");
            }
            return View(reg);
        }

        public IActionResult Login()
        {
            // Check if user is already logged in
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogInViewModel logIn)
        {
            if (!ModelState.IsValid)
            {
                return View(logIn);
            }
            UserAccount account = await (from u in _context.UserAccounts
                                         where (u.UserName == logIn.UserNameOrEmail
                                             || u.Email == logIn.UserNameOrEmail)
                                         && u.Password == logIn.Password
                                         select u).SingleOrDefaultAsync();

            // Method syntax
            //UserAccount acc = await _context.UserAccounts
            //    .Where(userAcc => 
            //          (userAcc.UserName == logIn.UserNameOrEmail 
            //        || userAcc.Email == logIn.UserNameOrEmail)
            //        && userAcc.Password == logIn.Password).SingleOrDefaultAsync();

            if (account == null)
            {
                // Credentials did not match
                ModelState.AddModelError(nameof(LogInViewModel.UserNameOrEmail), "That account was not found please try again.");
                // Error Message
                TempData["Message"] = $"Im sorry your Login Information was not correct.";
                return View(logIn);
            }
            // Say Hello!
            TempData["Message"] = $"Welcome back {account.UserName}!";

            // Log User into website. Do this after adding sessions to start up.
            LogUserIn(account.UserID);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            // Destroy Session
            HttpContext.Session.Clear();

            // Redirect to Homepage
            return RedirectToAction("Index", "Home");
        }

        private void LogUserIn(int accountId)
        {
            HttpContext.Session.SetInt32("UserId", accountId);
        }
    }
}