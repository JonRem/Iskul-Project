using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(int? id)
        {
            return View();
        }
    }
}
