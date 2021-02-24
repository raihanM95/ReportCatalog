using Encrypt.Pass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportCatalog.Application;
using ReportCatalog.Domain.Entities;
using ReportCatalog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public UsersController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: Users/Users
        public async Task<IActionResult> Users()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var users = new UserViewModel
                {
                    Users = await _repository.Users.GetAllAsync()
                };

                return View(users);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        //GET: Users/Register
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return RedirectToAction("Reports", "Reports");
            }
            else
            {
                return View();
            }
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password")] UserViewModel model)
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return RedirectToAction("Reports", "Reports");
            }
            else
            {
                if (ModelState.IsValid && !UserNameExists(model.Username))
                {
                    var user = new User
                    {
                        Username = model.Username,
                        Password = Crypto.Hash(model.Password),
                        UserType = "user",
                        CreatedBy = model.Username,
                        Created = DateTime.Now
                    };

                    await _repository.Users.RegisterUserAsync(user);
                    return RedirectToAction(nameof(Login));
                }

                ModelState.AddModelError("Username", "Username already exists!");

                //Thread.Sleep(10000);
                return View();
            }
        }

        //GET: Users/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return RedirectToAction("Reports", "Reports");
            }
            else
            {
                return View();
            }
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("Username,Password")] UserViewModel model)
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return RedirectToAction("Reports", "Reports");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = await _repository.Users.GetUserAsync(model.Username, Crypto.Hash(model.Password), "user");
                    if (user == null)
                    {
                        ModelState.AddModelError("UserType", "Invalid login attempt!");
                        return View();
                    }
                    else
                    {
                        var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
                        // add userlog
                        var userLog = new UserLog
                        {
                            Username = model.Username,
                            UserType = "user",
                            Time = DateTime.Now,
                            Type = "Login",
                            IP = feature?.RemoteIpAddress?.ToString()
                        };

                        await _repository.UserLogs.AddAsync(userLog);

                        HttpContext.Session.SetString("User", user.Username);
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View();
            }
        }

        //POST: Users/Logout
        public async Task<ActionResult> UserLogout()
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
            // add userlog
            var userLog = new UserLog
            {
                Username = HttpContext.Session.GetString("User"),
                UserType = "user",
                Time = DateTime.Now,
                Type = "Logout",
                IP = ip
            };

            await _repository.UserLogs.AddAsync(userLog);
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login", "Users");
        }

        //GET: Users
        [HttpGet]
        [Route("c/access")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return View();
            }
        }

        // POST: Users
        [HttpPost]
        [Route("c/access")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind("Username,Password")] UserViewModel model)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = await _repository.Users.GetUserAsync(model.Username, model.Password, "admin");
                    if (user == null)
                    {
                        ModelState.AddModelError("Password", "Invalid login attempt!");
                        return View();
                    }
                    else
                    {
                        var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
                        // add userlog
                        var userLog = new UserLog
                        {
                            Username = model.Username,
                            UserType = "admin",
                            Time = DateTime.Now,
                            Type = "Login",
                            IP = feature?.RemoteIpAddress?.ToString()
                        };

                        await _repository.UserLogs.AddAsync(userLog);

                        HttpContext.Session.SetString("Admin", user.Username);
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
                return View();
            }
        }

        //POST: Users/Logout
        public async Task<ActionResult> Logout()
        {
            var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
            // add userlog
            var userLog = new UserLog
            {
                Username = HttpContext.Session.GetString("Admin"),
                UserType = "admin",
                Time = DateTime.Now,
                Type = "Logout",
                IP = feature?.RemoteIpAddress?.ToString()
            };

            await _repository.UserLogs.AddAsync(userLog);
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index", "Users");
        }

        private bool UserNameExists(string userName)
        {
            var category = _repository.Users.GetByNameAsync(userName);
            if (category.Result != null)
                return true;
            else
                return false;
        }
    }
}
