﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportCatalog.Application;
using ReportCatalog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Controllers
{
    public class UserLogsController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public UserLogsController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: UserLogs
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var userLogs = new UserLogViewModel
                {
                    UserLogs = await _repository.UserLogs.GetAllAsync()
                };

                return View(userLogs);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }
    }
}
