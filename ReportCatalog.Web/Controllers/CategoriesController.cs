using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]
    public class CategoriesController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public CategoriesController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var categories = new CategoryViewModel
                {
                    Categories = await _repository.Categories.GetAllAsync()
                };

                return View(categories);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        //GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
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
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // GET: Categories/Create
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

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] CategoryViewModel model)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (ModelState.IsValid && !CategoryExists(model.Name))
                {
                    var category = new Category
                    {
                        Name = model.Name,
                        CreatedBy = HttpContext.Session.GetString("User"),
                        Created = DateTime.Now
                    };

                    await _repository.Categories.AddAsync(category);
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

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
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
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] CategoryViewModel model)
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
                        var category = new Category
                        {
                            Id = model.Id,
                            Name = model.Name,
                            LastModifiedBy = HttpContext.Session.GetString("User"),
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
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // POST: Categories/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var category = await _repository.Categories.DeleteAsync(id);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
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
