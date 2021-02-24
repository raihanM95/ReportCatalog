using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportCatalog.Application;
using ReportCatalog.Domain.Entities;
using ReportCatalog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public ProjectsController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var projects = new ProjectViewModel
                {
                    Projects = await _repository.Projects.GetAllAsync()
                };

                return View(projects);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        //GET: Projects/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var project = await _repository.Projects.GetByIdAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                return Json(project);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] ProjectViewModel model)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (ModelState.IsValid && !ProjectExists(model.Name))
                {
                    var project = new Project
                    {
                        Name = model.Name,
                        CreatedBy = HttpContext.Session.GetString("Admin"),
                        Created = DateTime.Now
                    };

                    await _repository.Projects.AddAsync(project);
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("Name", "Name already exists!");

                //Thread.Sleep(10000);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var project = await _repository.Projects.GetByIdAsync(id);
                if (project == null)
                {
                    return NotFound();
                }
                
                return Json(project);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] ProjectViewModel model)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (id != model.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var project = new Project
                        {
                            Id = model.Id,
                            Name = model.Name,
                            LastModifiedBy = HttpContext.Session.GetString("Admin"),
                            LastModified = DateTime.Now
                        };
                        await _repository.Projects.UpdateAsync(project);
                    }
                    catch (Exception ex)
                    {
                        if (!ProjectExists(model.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // POST: Projects/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var category = await _repository.Projects.DeleteAsync(id);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        private bool ProjectExists(int id)
        {
            var project = _repository.Projects.GetByIdAsync(id);
            if (project.Result != null)
                return true;
            else
                return false;
        }

        private bool ProjectExists(string name)
        {
            var project = _repository.Projects.GetByNameAsync(name);
            if (project.Result != null)
                return true;
            else
                return false;
        }
    }
}
