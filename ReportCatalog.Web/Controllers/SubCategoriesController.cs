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
    public class SubCategoriesController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public SubCategoriesController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: SubCategories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var subCategories = new SubCategoryViewModel
                {
                    SubCategories = await _repository.SubCategories.GetAllAsync()
                };

                ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");

                return View(subCategories);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        //GET: SubCategories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var subCategory = await _repository.SubCategories.GetByIdAsync(id);
                if (subCategory == null)
                {
                    return NotFound();
                }

                return Json(subCategory);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // GET: SubCategories/Create
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

        // POST: SubCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CategoryId,Id")] SubCategoryViewModel model)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (ModelState.IsValid && !SubCategoryExists(model.Name))
                {
                    var subCategory = new SubCategory
                    {
                        Name = model.Name,
                        CategoryId = model.CategoryId,
                        CreatedBy = HttpContext.Session.GetString("Admin"),
                        Created = DateTime.Now
                    };

                    await _repository.SubCategories.AddAsync(subCategory);
                    return RedirectToAction(nameof(Index));
                }

                ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");
                ModelState.AddModelError("Name", "Name already exists!");

                //Thread.Sleep(10000);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // GET: SubCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var subCategory = await _repository.SubCategories.GetByIdAsync(id);
                if (subCategory == null)
                {
                    return NotFound();
                }
                //return View(subCategory);
                return Json(subCategory);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // POST: SubCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] SubCategoryViewModel model)
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
                        var subCategory = new SubCategory
                        {
                            Id = model.Id,
                            Name = model.Name,
                            LastModifiedBy = HttpContext.Session.GetString("Admin"),
                            LastModified = DateTime.Now
                        };
                        await _repository.SubCategories.UpdateAsync(subCategory);
                    }
                    catch (Exception ex)
                    {
                        if (!SubCategoryExists(model.Id))
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

        // POST: SubCategories/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var subCategory = await _repository.SubCategories.DeleteAsync(id);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // GET: SubCategories/SubCategory
        public async Task<IActionResult> SubCategory(int id)
        {
            var subCategories = await _repository.SubCategories.GetByCategoryIdAsync(id);
            return Json(subCategories);
        }

        private bool SubCategoryExists(int id)
        {
            var subCategory = _repository.SubCategories.GetByIdAsync(id);
            if (subCategory.Result != null)
                return true;
            else
                return false;
        }

        private bool SubCategoryExists(string name)
        {
            var subCategory = _repository.SubCategories.GetByNameAsync(name);
            if (subCategory.Result != null)
                return true;
            else
                return false;
        }
    }
}
