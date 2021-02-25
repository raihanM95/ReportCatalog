using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReportCatalog.Application;
using ReportCatalog.Domain.Entities;
using ReportCatalog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public CategoriesController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: Categories
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var categories = new CategoryViewModel
            {
                Categories = await _repository.Categories.GetAllAsync()
            };

            ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");

            return View(categories);
        }

        //GET: Categories/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await _repository.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Json(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ProjectId,Id")] CategoryViewModel model)
        {
            if (ModelState.IsValid && !CategoryExists(model.Name))
            {
                var category = new Category
                {
                    Name = model.Name,
                    ProjectId = model.ProjectId,
                    CreatedBy = HttpContext.Session.GetString("Admin"),
                    Created = DateTime.Now
                };

                await _repository.Categories.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");
            ModelState.AddModelError("Name", "Name already exists!");

            //Thread.Sleep(10000);
            return View();
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await _repository.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            //return View(category);
            return Json(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] CategoryViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category = new Category
                    {
                        Id = model.Id,
                        Name = model.Name,
                        LastModifiedBy = HttpContext.Session.GetString("Admin"),
                        LastModified = DateTime.Now
                    };
                    await _repository.Categories.UpdateAsync(category);
                }
                catch (Exception ex)
                {
                    if (!CategoryExists(model.Id))
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

        // POST: Categories/Delete/
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _repository.Categories.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Category
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Category(int id)
        {
            var categories = await _repository.Categories.GetByProjectIdAsync(id);
            return Json(categories);
        }

        private bool CategoryExists(int id)
        {
            var category = _repository.Categories.GetByIdAsync(id);
            if (category.Result != null)
                return true;
            else
                return false;
        }

        private bool CategoryExists(string name)
        {
            var category = _repository.Categories.GetByNameAsync(name);
            if (category.Result != null)
                return true;
            else
                return false;
        }
    }
}
