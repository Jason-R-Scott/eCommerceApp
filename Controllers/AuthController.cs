using System;
using System.Linq;
using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers {
    public class AuthController : Controller {
        private readonly DataContext _context;
        // private readonly IAuthRepository _repo;

        public AuthController (
            DataContext context
            // IAuthRepository repo
        ) {
            this._context = context;
            // _repo = repo;
        }

        [HttpGet ("")]
        public IActionResult Index () {
            return View ();
        }

        // [HttpPost ("registertest")]
        // public IActionResult Rtest (RegistrationViewModel reg) {
        //     if (!ModelState.IsValid) {
        //         return View ("Index");
        //     }
        //     var registration = _repo.Register(reg);
        //     if (registration is null) {
        //         return RedirectToAction("Index");
        //     }
        //     return View("Index");
        // }

        [HttpPost ("register")]
        public IActionResult Create (RegistrationViewModel reg) {
            if (!ModelState.IsValid) {
                return View ("Index");
            }
            User userInDB = _context.Users.FirstOrDefault (u => u.Email == reg.Email);
            if (userInDB != null) {
                ModelState.AddModelError ("Email", "User already exists");
                return View ("Index");
            }

            PasswordHasher<RegistrationViewModel> hasher = new PasswordHasher<RegistrationViewModel> ();
            string hashedPW = hasher.HashPassword (reg, reg.Password);
            User newUser = new User {
                FirstName = reg.FirstName,
                LastName = reg.LastName,
                Email = reg.Email,
                Password = hashedPW,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Users.Add (newUser);
            _context.SaveChanges ();

            return RedirectToAction ("Index");
        }

        [HttpPost ("login")]
        public IActionResult Login (LoginViewModel login) {
            if (!ModelState.IsValid) {
                return View ("Index");
            } else {
                var userInDB = _context.Users.FirstOrDefault (u => u.Email == login.LoginEmail);
                if (userInDB is null) {
                    ModelState.AddModelError ("LoginEmail", "Invalid Login");
                    return View ("Index");
                } else {
                    var hasher = new PasswordHasher<LoginViewModel> ();
                    var result = hasher.VerifyHashedPassword (login, userInDB.Password, login.LoginPassword);
                    if (result == 0) {
                        ModelState.AddModelError ("LoginPassWord", "Invalid Password");
                        return View ("Index");
                    }
                }
            }
            User loggedIn = _context.Users.FirstOrDefault (u => u.Email == login.LoginEmail);
            System.Console.WriteLine ("#########################################");
            // System.Console.WriteLine(loggedIn.LoginEmail);
            System.Console.WriteLine (login.LoginEmail);
            var username = loggedIn.FirstName;
            int userid = loggedIn.UserID;
            System.Console.WriteLine (username);
            System.Console.WriteLine (userid);
            HttpContext.Session.SetInt32 ("ID", userid);
            HttpContext.Session.SetString ("Name", username);
            HttpContext.Session.SetString ("loggedin", "true");
            return RedirectToAction ("AllProducts", "Home");
        }

        [HttpGet ("logout")]
        public IActionResult Logout () {
            HttpContext.Session.SetString ("loggedin", "false");
            HttpContext.Session.Clear ();
            return RedirectToAction ("Index");
        }
    }
}