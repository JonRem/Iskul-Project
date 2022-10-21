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
        public EnrollUserVM EnrollUserVM { get; set; }
        
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
                EnrollDetail enrollDetail = _enrollDetailRepo.FirstOrDefault(u => u.Id == enrollHeader.Id && u.SchoolId == school.Id);

                if (enrollDetail != null && enrollDetail.EnrollStatus == WC.StatusSaved)
                {
                    EnrollUserVM enrollUserVM = new EnrollUserVM()
                    {
                        ApplicationUser = applicationUser,
                        School = school,
                        EnrollHeader = enrollHeader,
                        EnrollDetail = enrollDetail
                    };
                    return View(EnrollUserVM);
                }

            }

            EnrollUserVM = new EnrollUserVM()
            {
                ApplicationUser = applicationUser,
                EnrollHeader = new EnrollHeader()
                {
                    ApplicationUserId = applicationUser.Id,
                    EnrollDate = DateTime.Today,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    PhoneNumber = applicationUser.PhoneNumber,
                    Email = applicationUser.Email
                },
                EnrollDetail = new EnrollDetail()
                {
                    SchoolPhoto = applicationUser.ProfilePicture
                },

                School = school

            };




            return View(EnrollUserVM);
        }
    }
}
