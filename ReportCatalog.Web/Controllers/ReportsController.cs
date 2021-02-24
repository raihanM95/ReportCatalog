using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReportCatalog.Application;
using ReportCatalog.Domain.Entities;
using ReportCatalog.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportsController(IRepositoryWrapper repository, IWebHostEnvironment hostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: Reports/Reports
        public async Task<IActionResult> Reports()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Reports/SubCategoryWiseReports
        public async Task<IActionResult> SubCategoryWiseReports(int id)
        {
            var reports = await _repository.Reports.GetBySubCategoryIdAsync(id);
            return Json(reports);
        }

        // GET: Reports/View/5
        public async Task<IActionResult> View(int id)
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var report = await _repository.Reports.GetByIdAsync(id);
                if (report == null)
                {
                    return NotFound();
                }

                return View(report);
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var reports = new ReportViewModel
                {
                    Reports = await _repository.Reports.GetAllAsync()
                };

                //ViewData["Category"] = new SelectList(await _repository.Categories.GetAllAsync(), "Id", "Name");

                return View(reports);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var report = await _repository.Reports.GetByIdAsync(id);
                if (report == null)
                {
                    return NotFound();
                }

                return View(report);
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // GET: Reports/Create
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,FileName,Description,InputImage,OutputImage,SubCategoryId,Id")] ReportViewModel model)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                if (ModelState.IsValid && !ReportExists(model.Name))
                {
                    var report = new Report
                    {
                        Name = model.Name,
                        FileName = model.FileName,
                        Description = model.Description,
                        InputImage = InputImageUploadedFile(model),
                        OutputImage = OutputImageUploadedFile(model),
                        SubCategoryId = model.SubCategoryId,
                        CreatedBy = HttpContext.Session.GetString("Admin"),
                        Created = DateTime.Now
                    };

                    ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");

                    await _repository.Reports.AddAsync(report);
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

        private bool ReportExists(int id)
        {
            var category = _repository.Categories.GetByIdAsync(id);
            if (category.Result != null)
                return true;
            else
                return false;
        }

        private bool ReportExists(string name)
        {
            var category = _repository.Categories.GetByNameAsync(name);
            if (category.Result != null)
                return true;
            else
                return false;
        }

        // POST: Reports/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var report = await _repository.Reports.DeleteAsync(id);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        // Images upload
        private string InputImageUploadedFile(ReportViewModel model)
        {
            string inputImagePath = null;

            if (model.InputImage != null)
            {
                //Save image to wwwroot/images
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.InputImage.FileName);
                string extension = Path.GetExtension(model.InputImage.FileName);
                inputImagePath = fileName = "MSDSL" + "_" + fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    model.InputImage.CopyTo(fileStream);
                }

                //string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                //inputImagePath = Guid.NewGuid().ToString() + "_" + model.InputImage.FileName;
                //string filePath = Path.Combine(uploadsFolder, inputImagePath);
                //using (var fileStream = new FileStream(filePath, FileMode.Create))
                //{
                //    model.InputImage.CopyTo(fileStream);
                //}
            }
            return inputImagePath;
        }

        private string OutputImageUploadedFile(ReportViewModel model)
        {
            string outputImagePath = null;

            if (model.OutputImage != null)
            {
                //Save image to wwwroot/images
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.OutputImage.FileName);
                string extension = Path.GetExtension(model.OutputImage.FileName);
                outputImagePath = fileName = "MSDSL" + "_" + fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    model.InputImage.CopyTo(fileStream);
                }
            }
            return outputImagePath;
        }
    }
}
