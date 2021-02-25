using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Reports()
        {
            ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");
            return View();
        }

        // GET: Reports/SubCategoryWiseReports
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> SubCategoryWiseReports(int id)
        {
            var reports = await _repository.Reports.GetBySubCategoryIdAsync(id);
            return Json(reports);
        }

        // GET: Reports/View/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> View(int id)
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

        // GET: Reports
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var reports = new ReportViewModel
            {
                Reports = await _repository.Reports.GetAllAsync()
            };

            return View(reports);
        }

        // GET: Reports/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
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

        // GET: Reports/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["Project"] = new SelectList(await _repository.Projects.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,FileName,Description,InputImage,OutputImage,SubCategoryId,Id")] ReportViewModel model)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var report = await _repository.Reports.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
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
