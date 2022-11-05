using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Iskul_DataAccess;
using Iskul_DataAccess.Repository.IRepository;
using Iskul_Models;
using Iskul_Models.ViewModels;
using Iskul_Utility;
using Iskul_Utility.BrainTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;


namespace Iskul.Controllers
{
    [Authorize]
    public class EnrollController : Controller
    {
        private readonly IEnrollHeaderRepository _enrollHeaderRepo;
        private readonly IEnrollDetailRepository _enrollDetailRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IApplicationUserRepository _userRepo;
        private readonly ISchoolRepository _schoolRepo;

        

        [BindProperty]
        public EnrollVM EnrollVM { get; set; }

        public EnrollController(IEnrollHeaderRepository enrollHeaderRepo, 
            IEnrollDetailRepository enrollDetailRepo,
            IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender,
            IApplicationUserRepository userRepo,
            ISchoolRepository schoolRepo)
        {
            _enrollHeaderRepo = enrollHeaderRepo;
            _enrollDetailRepo = enrollDetailRepo;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _userRepo = userRepo;
            _schoolRepo = schoolRepo;
        }

        public IActionResult Index(int? id) // id is schoolId
        {
            // retrieve applicationuser data here and check for saved enrollment

           

            ApplicationUser applicationUser;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // var userId = User.FindFirstValue(ClaimTypes.Name);

            applicationUser = _userRepo.FirstOrDefault(u => u.Id == claim.Value);

            School school = _schoolRepo.FirstOrDefault(u => u.Id == id);

            EnrollHeader enrollHeader = _enrollHeaderRepo.FirstOrDefault(u => u.ApplicationUserId == applicationUser.Id);
            
            if (enrollHeader != null)  
                // enrollment record exists for this user,
                // now check if enrollment record exists for the chosen school
            {
                EnrollDetail enrollDetail = _enrollDetailRepo.FirstOrDefault(u => u.EnrollHeaderId == enrollHeader.Id && u.SchoolId == school.Id);

                if (enrollDetail != null && enrollDetail.EnrollStatus == WC.StatusSaved)
                {
                    EnrollVM  = new EnrollVM()
                    {
                        AppUserId = applicationUser.Id,
                        SchoolId = school.Id,
                        SchoolName = school.SchoolName,
                        SchoolImage = school.Image,
                        EnrollHeader = enrollHeader,  // Header and Detail exists
                        EnrollDetail = enrollDetail,
                        CivilStatusSelectList = WC.ListCivilStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Text = i,
                            Value = i
                        })
                    };
                    return View(EnrollVM);
                }
                else
                {
                    EnrollVM  = new EnrollVM()
                    {
                        AppUserId = applicationUser.Id,
                        SchoolId = school.Id,
                        SchoolName = school.SchoolName,
                        SchoolImage = school.Image,
                        EnrollHeader = enrollHeader,  // Header exists
                        EnrollDetail = new EnrollDetail()
                        {
                            SchoolId = school.Id,
                            LogDate = DateTime.Now,
                            DateofBirth = DateTime.Now,
                            SchoolPhoto = WC.NoImageAvailable,
                            EnrollStatus = WC.StatusPending
                        },
                        CivilStatusSelectList = WC.ListCivilStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Text = i,
                            Value = i
                        })
                    };
                    return View(EnrollVM);
                }

            };
            
            // Header is null

            EnrollVM  = new EnrollVM()
            {
                AppUserId = applicationUser.Id,
                SchoolId = school.Id,
                SchoolName = school.SchoolName,
                SchoolImage = school.Image,
            
                EnrollHeader = new EnrollHeader()
                {
                    ApplicationUserId = applicationUser.Id,
                    EnrollDate = DateTime.Now,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    PhoneNumber = applicationUser.PhoneNumber,
                    Email = applicationUser.Email,

                },            

                EnrollDetail = new EnrollDetail()
                {
                    SchoolId = school.Id,
                    LogDate = DateTime.Now,
                    DateofBirth = DateTime.Now,
                    SchoolPhoto =  WC.NoImageAvailable,
                    EnrollStatus = WC.StatusPending
                },

                CivilStatusSelectList = WC.ListCivilStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i,
                    Value = i
                })

            };

            return View(EnrollVM);
        }

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(EnrollVM enrollVM)
        {
            //if (Request.Form.Files.Count > 0)
            //{
            //    IFormFile file = Request.Form.Files.FirstOrDefault();
            //    using (var dataStream = new MemoryStream())
            //    {
            //        file.CopyToAsync(dataStream);
            //        enrollUserVM.EnrollDetail.SchoolPhoto = dataStream.ToArray();
            //    }
            //}

            if (ModelState.IsValid)
            {
                //check if header already exists
                if (enrollVM.EnrollHeader.Id == 0)
                {
                    _enrollHeaderRepo.Add(enrollVM.EnrollHeader);
                }
                else
                {
                    _enrollHeaderRepo.FirstOrDefault(u => u.Id == enrollVM.EnrollHeader.Id, isTracking: false);
                    _enrollHeaderRepo.Update(enrollVM.EnrollHeader);
                }
                _enrollHeaderRepo.Save();

                //check detail records

                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (enrollVM.EnrollDetail.Id == 0)
                {
                    //Creating

                    enrollVM.EnrollDetail.EnrollHeaderId = enrollVM.EnrollHeader.Id;

                    //Upload ProfilePhoto
                    if (enrollVM.ProfileImage.Name == files[0].Name)
                    {
                        string uploadPhoto = webRootPath + WC.ProfilePhotoPath;
                        string fileNamePhoto = Guid.NewGuid().ToString();
                        string extensionPhoto = Path.GetExtension(files[0].FileName);

                        using (var fileStream = new FileStream(Path.Combine(uploadPhoto, fileNamePhoto + extensionPhoto), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        enrollVM.EnrollDetail.SchoolPhoto = fileNamePhoto + extensionPhoto;
                    }
                    

                    if (enrollVM.ConsentForm != null)
                    {
                        if (enrollVM.ConsentForm.Name == files[1].Name)
                        {
                            string upload = webRootPath + WC.ConsentForm;
                            string fileName = Guid.NewGuid().ToString();
                            string extension = Path.GetExtension(files[1].FileName);

                            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                            {
                                files[1].CopyTo(fileStream);
                            }

                            enrollVM.EnrollDetail.ConsentForm = fileName + extension;
                        }
                            
                    }
                    

                    enrollVM.EnrollDetail.EnrollStatus = WC.StatusPending;

                    enrollVM.EnrollDetail.LogDate = DateTime.Now;

                    

                    _enrollDetailRepo.Add(enrollVM.EnrollDetail);

                }
                else
                {
                    //Updating
                    // old code **** var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    // no tracking should be set for this object so only one instance of the object will be updated which is ProductVM.Product
                    var objFromDb = _enrollDetailRepo.FirstOrDefault(u => u.Id == enrollVM.EnrollDetail.Id, isTracking: false);

                    //check here if new file has been updated for an existing consent form
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ConsentForm;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.ConsentForm);

                        //delete old file if it exists in the image folder
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        enrollVM.EnrollDetail.ConsentForm = fileName + extension;
                    }
                    else
                    {
                        enrollVM.EnrollDetail.ConsentForm = objFromDb.ConsentForm;
                    }

                    enrollVM.EnrollDetail.EnrollStatus = WC.StatusPending;

                    enrollVM.EnrollDetail.LogDate = DateTime.Now;

                    

                    _enrollDetailRepo.Update(enrollVM.EnrollDetail);
                }
                
                _enrollDetailRepo.Save();
                TempData[WC.Success] = "Action completed successfully";
                return RedirectToAction("Index", "Home");
            }

            // if model state is not valid, the view model should be populated again
            //schoolVM.CategorySelectList = _schoolRepo.GetAllDropdownList(WC.CategoryName);
            enrollVM.EnrollHeader.EnrollDate = DateTime.Now;
            enrollVM.CivilStatusSelectList = WC.ListCivilStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i,
                Value = i
            });
            return View("Index", enrollVM);
        }

        //POST - SaveApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveApplication(EnrollUserVM enrollUserVM)
        {
            if (ModelState.IsValid)
            {
                //check if header already exists
                if (enrollUserVM.EnrollHeader.Id == 0)
                {
                    _enrollHeaderRepo.Add(enrollUserVM.EnrollHeader);
                }
                else
                {
                    _enrollHeaderRepo.FirstOrDefault(u => u.Id == enrollUserVM.EnrollHeader.Id);
                    _enrollHeaderRepo.Update(enrollUserVM.EnrollHeader);
                }
                _enrollHeaderRepo.Save();
                //check detail records

                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (enrollUserVM.EnrollDetail.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ConsentForm;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    enrollUserVM.EnrollDetail.ConsentForm = fileName + extension;

                    enrollUserVM.EnrollDetail.EnrollStatus = WC.StatusSaved;

                    enrollUserVM.EnrollDetail.LogDate = DateTime.Now;

                    // check if school profile picture was uploaded
                    //if (Request.Form.Files.Count > 0)
                    //{
                    //    IFormFile file = Request.Form.Files.FirstOrDefault();
                    //    using (var dataStream = new MemoryStream())
                    //    {
                    //        file.CopyToAsync(dataStream);
                    //        enrollUserVM.EnrollDetail.SchoolPhoto = dataStream.ToArray();
                    //    }
                    //}

                    _enrollDetailRepo.Add(enrollUserVM.EnrollDetail);

                }
                else
                {
                    //Updating
                    // old code **** var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    // no tracking should be set for this object so only one instance of the object will be updated which is ProductVM.Product
                    var objFromDb = _enrollDetailRepo.FirstOrDefault(u => u.Id == enrollUserVM.EnrollDetail.Id, isTracking: false);

                    //check here if new file has been updated for an existing consent form
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ConsentForm;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.ConsentForm);

                        //delete old file if it exists in the image folder
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        enrollUserVM.EnrollDetail.ConsentForm = fileName + extension;
                    }
                    else
                    {
                        enrollUserVM.EnrollDetail.ConsentForm = objFromDb.ConsentForm;
                    }

                    enrollUserVM.EnrollDetail.EnrollStatus = WC.StatusSaved;

                    enrollUserVM.EnrollDetail.LogDate = DateTime.Now;

                    // check if school profile picture was uploaded
                    //if (Request.Form.Files.Count > 0)
                    //{
                    //    IFormFile file = Request.Form.Files.FirstOrDefault();
                    //    using (var dataStream = new MemoryStream())
                    //    {
                    //        file.CopyToAsync(dataStream);
                    //        enrollUserVM.EnrollDetail.SchoolPhoto = dataStream.ToArray();
                    //    }
                    //}

                    _enrollDetailRepo.Update(enrollUserVM.EnrollDetail);
                }
                
                _enrollDetailRepo.Save();
                TempData[WC.Success] = "Action completed successfully";
                return RedirectToAction("Index", "Home");
            }

            // if model state is not valid, the view model should be populated again
            //schoolVM.CategorySelectList = _schoolRepo.GetAllDropdownList(WC.CategoryName);
            enrollUserVM.CivilStatusSelectList = WC.ListCivilStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i,
                Value = i
            });
            return View(enrollUserVM);
        }

    }
}
