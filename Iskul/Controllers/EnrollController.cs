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
using Syncfusion.EJ2.Charts;
using Syncfusion.EJ2.Inputs;
using Newtonsoft.Json.Schema;

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

            EnrollHeader enrollHeader = _enrollHeaderRepo.FirstOrDefault(u => u.ApplicationUserId == applicationUser.Id && u.SchoolId == school.Id);
            
            if (enrollHeader != null)
                // enrollment record exists for this user,
                // now check if enrollment record exists for the chosen school
            {
                if (enrollHeader.DetailRecOpen == false)
                {
                    TempData[WC.Error] = "Enrollment unavailable, User may have on-going application or already enrolled, please choose another school.";
                    return RedirectToAction("Index", "Home");
                }

                EnrollDetail enrollDetail = _enrollDetailRepo.FirstOrDefault(u => u.EnrollHeaderId == enrollHeader.Id && u.Id == enrollHeader.LastDetailRec);

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
                            EnrollDate = DateTime.Now,
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
                    SchoolId = school.Id,                    
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    PhoneNumber = applicationUser.PhoneNumber,
                    Email = applicationUser.Email,

                },            

                EnrollDetail = new EnrollDetail()
                {
                    EnrollDate = DateTime.Now,
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
        public async Task<IActionResult> Upsert(EnrollVM enrollVM)
        //public IActionResult Upsert(EnrollVM enrollVM)
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

                    enrollVM.EnrollDetail.EnrollDate = DateTime.Now;

                    enrollVM.EnrollDetail.EnrollStatus = WC.StatusPending;

                    enrollVM.EnrollDetail.LogDate = DateTime.Now;

                    

                    _enrollDetailRepo.Add(enrollVM.EnrollDetail);
                    _enrollDetailRepo.Save();

                    // update header record to mark last detail record
                    
                    try
                    {
                        _enrollHeaderRepo.FirstOrDefault(u => u.Id == enrollVM.EnrollHeader.Id, isTracking: false);
                        // make detail record unavailable for for editing
                        enrollVM.EnrollHeader.DetailRecOpen = false;  
                        // insert detail record ID in Header
                        enrollVM.EnrollHeader.LastDetailRec = enrollVM.EnrollDetail.Id;

                        _enrollHeaderRepo.Update(enrollVM.EnrollHeader);
                        _enrollHeaderRepo.Save();
                    }
                        
                        catch (Exception e)
                    {
                        //Console.WriteLine("Exception caught: {0}", e);
                        TempData[WC.Error] = "Exception caught: {0} " + e;
                        //TempData[WC.Error] = "Action completed with Error on Header record update ... " + ;
                        return RedirectToAction("Index", "Home");
                    }
                   

                }
                else
                {
                    //Updating
                    // old code **** var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    // no tracking should be set for this object so only one instance of the object will be updated which is ProductVM.Product
                    var objFromDb = _enrollDetailRepo.FirstOrDefault(u => u.Id == enrollVM.EnrollDetail.Id, isTracking: false);

                    if (files.Count > 0)
                    {
                        if (enrollVM.ProfileImage.Name == files[0].Name)
                        {
                            string uploadPhoto = webRootPath + WC.ProfilePhotoPath;
                            string fileNamePhoto = Guid.NewGuid().ToString();
                            string extensionPhoto = Path.GetExtension(files[0].FileName);

                            if (files[0].FileName != objFromDb.SchoolPhoto)
                            {
                                var oldPhotoFile = Path.Combine(uploadPhoto, objFromDb.SchoolPhoto);

                                //delete old file if it exists in the image folder

                                if (System.IO.File.Exists(oldPhotoFile))
                                {
                                    System.IO.File.Delete(oldPhotoFile);
                                }
                                using (var fileStream = new FileStream(Path.Combine(uploadPhoto, fileNamePhoto + extensionPhoto), FileMode.Create))
                                {
                                    files[0].CopyTo(fileStream);
                                }
                                enrollVM.EnrollDetail.SchoolPhoto = fileNamePhoto + extensionPhoto;
                            }
                            else
                            {
                                enrollVM.EnrollDetail.SchoolPhoto = objFromDb.SchoolPhoto;
                            }
                        }

                        //check here if new file has been updated for an existing consent form

                        if (enrollVM.ConsentForm != null)
                        {
                            if (enrollVM.ConsentForm.Name == files[1].Name)
                            {
                                string upload = webRootPath + WC.ConsentForm;
                                string fileName = Guid.NewGuid().ToString();
                                string extension = Path.GetExtension(files[1].FileName);

                                
                                if (objFromDb.ConsentForm != null)
                                {
                                    if (enrollVM.EnrollDetail.ConsentForm != objFromDb.ConsentForm)
                                    {
                                        var oldFile = Path.Combine(upload, objFromDb.ConsentForm);
                                        //delete old file if it exists in the Consent Form folder
                                        if (System.IO.File.Exists(oldFile))
                                        {
                                            System.IO.File.Delete(oldFile);
                                        }
                                    }   
                                }
                                //create
                                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                                {
                                    files[1].CopyTo(fileStream);
                                }
                                enrollVM.EnrollDetail.ConsentForm = fileName + extension;
                            }
                        }
                        else
                        {
                            // check if consent form previously exist then delete old consent form
                            string upload = webRootPath + WC.ConsentForm;
                            string fileName = Guid.NewGuid().ToString();
                            string extension = Path.GetExtension(files[1].FileName);


                            if (objFromDb.ConsentForm != null)
                            {
                                var oldFile = Path.Combine(upload, objFromDb.ConsentForm);
                                //delete old file if it exists in the Consent Form folder
                                if (System.IO.File.Exists(oldFile))
                                {
                                    System.IO.File.Delete(oldFile);
                                }
                                enrollVM.EnrollDetail.ConsentForm = "";


                            }
                        }
                    }
                    else
                    {
                        enrollVM.EnrollDetail.SchoolPhoto = objFromDb.SchoolPhoto;
                        enrollVM.EnrollDetail.ConsentForm = objFromDb.ConsentForm;
                    }

                    enrollVM.EnrollDetail.EnrollStatus = WC.StatusPending;
                    enrollVM.EnrollDetail.EnrollDate = DateTime.Now;
                    enrollVM.EnrollDetail.LogDate = DateTime.Now;                    

                    _enrollDetailRepo.Update(enrollVM.EnrollDetail);

                    _enrollDetailRepo.Save();

                    // update header record to mark last detail record

                    try
                    {
                        _enrollHeaderRepo.FirstOrDefault(u => u.Id == enrollVM.EnrollHeader.Id, isTracking: false);
                        // make detail record unavailable for for editing
                        enrollVM.EnrollHeader.DetailRecOpen = false;
                        // insert detail record ID in Header
                        enrollVM.EnrollHeader.LastDetailRec = enrollVM.EnrollDetail.Id;

                        _enrollHeaderRepo.Update(enrollVM.EnrollHeader);
                        _enrollHeaderRepo.Save();
                    }

                    catch (Exception e)
                    {
                        //Console.WriteLine("Exception caught: {0}", e);
                        TempData[WC.Error] = "Exception caught: {0} " + e;
                        //TempData[WC.Error] = "Action completed with Error on Header record update ... " + ;
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                TempData[WC.Success] = "Action completed successfully";
                
                // send confirmation email

                var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
              + "templates" + Path.DirectorySeparatorChar.ToString() +
              "EnrollApplication.html";

                var subject = "New Application";
                string HtmlBody = "";
                using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
                {
                    HtmlBody = sr.ReadToEnd();
                }

                //Name: {0}
                //Email: { 1}
                //Phone: { 2}
                //Enrollment Date: {3}
                //School: { 4}

                StringBuilder SchoolListSB = new StringBuilder();
                //foreach (var prod in enrollVM.SchoolId)
                //{
                //SchoolListSB.Append($" - Name: {enrollVM.SchoolName} <span style='font-size:14px;'> (ID: {enrollVM.SchoolId})</span><br />");
                //}
                String SchoolInfo = " - Name: " + enrollVM.SchoolName + " " + "<span style='font-size:14px;'> (ID: " + enrollVM.SchoolId.ToString() + ")</span><br />";
                string fullname = enrollVM.EnrollHeader.FirstName + " " + enrollVM.EnrollHeader.LastName;
                string eDate = enrollVM.EnrollDetail.EnrollDate.ToShortDateString();
                string messageBody = string.Format(HtmlBody,
                    fullname,
                    enrollVM.EnrollHeader.Email,
                    enrollVM.EnrollHeader.PhoneNumber,
                    eDate,
                    SchoolInfo);
                    //SchoolListSB.ToString());

                await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

                return View("EnrollConfirmation", enrollVM);

                
            }

            // if model state is not valid, the view model should be populated again
            //schoolVM.CategorySelectList = _schoolRepo.GetAllDropdownList(WC.CategoryName);
            //enrollVM.EnrollHeader.EnrollDate = DateTime.Now;
            enrollVM.CivilStatusSelectList = WC.ListCivilStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i,
                Value = i
            });
            return View("Index", enrollVM);
        }

        

        // Enrollment Confirmation
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("EnrollConfirmation")]
        public async Task<IActionResult> EnrollConfirmation()
        {
            // we need to send confirmation email
            //var objFromDb = _enrollHeaderRepo.FirstOrDefault(u => u.Id == EnrollVM.EnrollHeader.Id, isTracking: false);
            //var objSchoolDb = _schoolRepo.FirstOrDefault(u => u.Id == objFromDb.SchoolId, isTracking: false);

            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
              + "templates" + Path.DirectorySeparatorChar.ToString() +
              "Inquiry.html";

            var subject = "New Application";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            //Name: {0}
            //Email: { 1}
            //Phone: { 2}
            //Enrollment Date: {3}
            //School: { 4}

            StringBuilder SchoolListSB = new StringBuilder();
            //foreach (var prod in enrollVM.SchoolId)
            //{
            SchoolListSB.Append($" - Name: {EnrollVM.SchoolName} <span style='font-size:14px;'> (ID: {EnrollVM.SchoolId})</span><br />");
            //}
            string fullname = EnrollVM.EnrollHeader.FirstName + " " + EnrollVM.EnrollHeader.LastName;
            string messageBody = string.Format(HtmlBody,
                fullname,
                EnrollVM.EnrollHeader.Email,
                EnrollVM.EnrollHeader.PhoneNumber,
                SchoolListSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);
            //var obj = _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);
            
            return View(EnrollVM);
        }


        //POST - SaveApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveApplication(EnrollVM enrollVM)
        {
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

                    enrollVM.EnrollDetail.EnrollDate = DateTime.Now;

                    enrollVM.EnrollDetail.EnrollStatus = WC.StatusSaved;

                    enrollVM.EnrollDetail.LogDate = DateTime.Now;



                    _enrollDetailRepo.Add(enrollVM.EnrollDetail);
                    _enrollDetailRepo.Save();

                    // update header record to mark last detail record

                    try
                    {
                        _enrollHeaderRepo.FirstOrDefault(u => u.Id == enrollVM.EnrollHeader.Id, isTracking: false);
                        // make detail record unavailable for for editing
                        enrollVM.EnrollHeader.DetailRecOpen = true;
                        // insert detail record ID in Header
                        enrollVM.EnrollHeader.LastDetailRec = enrollVM.EnrollDetail.Id;

                        _enrollHeaderRepo.Update(enrollVM.EnrollHeader);
                        _enrollHeaderRepo.Save();
                    }

                    catch (Exception e)
                    {
                        //Console.WriteLine("Exception caught: {0}", e);
                        TempData[WC.Error] = "Exception caught: {0} " + e;
                        //TempData[WC.Error] = "Action completed with Error on Header record update ... " + ;
                        return RedirectToAction("Index", "Home");
                    }


                }
                else
                {
                    //Updating
                    // old code **** var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    // no tracking should be set for this object so only one instance of the object will be updated which is ProductVM.Product
                    var objFromDb = _enrollDetailRepo.FirstOrDefault(u => u.Id == enrollVM.EnrollDetail.Id, isTracking: false);

                    if (files.Count > 0)
                    {
                        if (enrollVM.ProfileImage.Name == files[0].Name)
                        {
                            string uploadPhoto = webRootPath + WC.ProfilePhotoPath;
                            string fileNamePhoto = Guid.NewGuid().ToString();
                            string extensionPhoto = Path.GetExtension(files[0].FileName);

                            var oldPhotoFile = Path.Combine(uploadPhoto, objFromDb.SchoolPhoto);

                            //delete old file if it exists in the image folder

                            if (System.IO.File.Exists(oldPhotoFile))
                            {
                                System.IO.File.Delete(oldPhotoFile);
                            }
                            using (var fileStream = new FileStream(Path.Combine(uploadPhoto, fileNamePhoto + extensionPhoto), FileMode.Create))
                            {
                                files[0].CopyTo(fileStream);
                            }
                            enrollVM.EnrollDetail.SchoolPhoto = fileNamePhoto + extensionPhoto;
                        }
                        //check here if new file has been updated for an existing consent form

                        if (enrollVM.ConsentForm != null)
                        {
                            if (enrollVM.ConsentForm.Name == files[1].Name)
                            {
                                string upload = webRootPath + WC.ConsentForm;
                                string fileName = Guid.NewGuid().ToString();
                                string extension = Path.GetExtension(files[1].FileName);

                                var oldFile = Path.Combine(upload, objFromDb.ConsentForm);

                                //delete old file if it exists in the Consent Form folder
                                if (System.IO.File.Exists(oldFile))
                                {
                                    System.IO.File.Delete(oldFile);
                                }
                                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                                {
                                    files[1].CopyTo(fileStream);
                                }

                                enrollVM.EnrollDetail.ConsentForm = fileName + extension;
                            }

                        }

                    }
                    else
                    {
                        enrollVM.EnrollDetail.SchoolPhoto = objFromDb.SchoolPhoto;
                        enrollVM.EnrollDetail.ConsentForm = objFromDb.ConsentForm;
                    }

                    enrollVM.EnrollDetail.EnrollStatus = WC.StatusSaved;
                    enrollVM.EnrollDetail.EnrollDate = DateTime.Now;
                    enrollVM.EnrollDetail.LogDate = DateTime.Now;

                    _enrollDetailRepo.Update(enrollVM.EnrollDetail);

                    _enrollDetailRepo.Save();

                    // update header record to mark last detail record

                    try
                    {
                        _enrollHeaderRepo.FirstOrDefault(u => u.Id == enrollVM.EnrollHeader.Id, isTracking: false);
                        // make detail record unavailable for for editing
                        enrollVM.EnrollHeader.DetailRecOpen = true;
                        // insert detail record ID in Header
                        enrollVM.EnrollHeader.LastDetailRec = enrollVM.EnrollDetail.Id;

                        _enrollHeaderRepo.Update(enrollVM.EnrollHeader);
                        _enrollHeaderRepo.Save();
                    }

                    catch (Exception e)
                    {
                        //Console.WriteLine("Exception caught: {0}", e);
                        TempData[WC.Error] = "Exception caught: {0} " + e;
                        //TempData[WC.Error] = "Action completed with Error on Header record update ... " + ;
                        return RedirectToAction("Index", "Home");
                    }
                }

                TempData[WC.Success] = "Application saved successfully";
                return RedirectToAction("Index", "Home");
            }

            
            // if model state is not valid, the view model should be populated again
            //schoolVM.CategorySelectList = _schoolRepo.GetAllDropdownList(WC.CategoryName);
            enrollVM.CivilStatusSelectList = WC.ListCivilStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i,
                Value = i
            });
            return View("Index", enrollVM);
        }

    }
}
