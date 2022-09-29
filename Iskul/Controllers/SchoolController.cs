using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Iskul_DataAccess;
using Iskul_DataAccess.Repository.IRepository;
using Iskul_Models;
using Iskul_Models.ViewModels;
using Iskul_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Iskul.Controllers
{
    [Authorize(Roles = WC.AdminRole)]

    public class SchoolController : Controller
    {
        private readonly ISchoolRepository _schoolRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SchoolController(ISchoolRepository schoolRepo, IWebHostEnvironment webHostEnvironment)
        {
            _schoolRepo = schoolRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            //old code for IEnumerable with include
            // IEnumerable<Product> objlist = _db.Product.Include(u=>u.Category).Include(u=>u.ApplicationType);
            IEnumerable<School> objlist = _schoolRepo.GetAll(includeProperties: "Category");

            //foreach(var obj in objlist)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
            //};

            return View(objlist);
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //// using ViewBag

            //// ViewBag.CategoryDropDown = CategoryDropDown;

            //// example below is when using ViewData instead of ViewBag

            //ViewData["CategoryDropDown"] = CategoryDropDown;

            // Product product = new Product();

            SchoolVM schoolVM = new SchoolVM()
            {
                School = new School(),
                CategorySelectList = _schoolRepo.GetAllDropdownList(WC.CategoryName),
            };

            if (id == null)
            {
                // this is for create
                return View(schoolVM);
            }
            else
            {
                schoolVM.School = _schoolRepo.Find(id.GetValueOrDefault());
                if (schoolVM == null)
                {
                    return NotFound();
                }
                return View(schoolVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SchoolVM schoolVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (schoolVM.School.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    schoolVM.School.Image = fileName + extension;

                    _schoolRepo.Add(schoolVM.School);

                }
                else
                {
                    //Updating
                    // old code **** var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    // no tracking should be set for this object so only one instance of the object will be updated which is ProductVM.Product
                    var objFromDb = _schoolRepo.FirstOrDefault(u => u.Id == schoolVM.School.Id, isTracking: false);

                    //check here if new file has been updated for an existing product
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        //delete old file if it exists in the image folder
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        schoolVM.School.Image = fileName + extension;
                    }
                    else
                    {
                        schoolVM.School.Image = objFromDb.Image;
                    }
                    _schoolRepo.Update(schoolVM.School);
                }

                _schoolRepo.Save();
                TempData[WC.Success] = "Action completed successfully";
                return RedirectToAction("Index");
            }

            // if model state is not valid, the view model should be populated again
            schoolVM.CategorySelectList = _schoolRepo.GetAllDropdownList(WC.CategoryName);

            return View(schoolVM);
        }





        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // old code ---> Product product = _db.Product.Include(u=>u.Category).Include(u=>u.ApplicationType).FirstOrDefault(u=>u.Id==id);

            // Eager loading here...
            School school = _schoolRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Category");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _schoolRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _schoolRepo.Remove(obj);
            _schoolRepo.Save();
            TempData[WC.Success] = "School removed successfully";
            return RedirectToAction("Index");

        }
    }
}
