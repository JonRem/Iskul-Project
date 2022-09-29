using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Iskul_DataAccess;
using Iskul_DataAccess.Repository.IRepository;
using Iskul_Models;
using Iskul_Models.ViewModels;
using Iskul_Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
// home controller comment for github push test only .....
namespace Iskul.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IProductRepository _prodRepo;
        private readonly ICategoryRepository _catRepo;

        private readonly ISchoolRepository _schoolRepo;

        //[BindProperty(SupportsGet = true)]
        //public string SearchProduct { get; set; }

        //public HomeController(ILogger<HomeController> logger, IProductRepository prodRepo,
        //    ICategoryRepository catRepo)

        public HomeController(ILogger<HomeController> logger, ISchoolRepository schoolRepo,
               ICategoryRepository catRepo)
                              
        {
            _logger = logger;
            _schoolRepo = schoolRepo;
            //_prodRepo = prodRepo;
            _catRepo = catRepo;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                // old code ==> Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType),
                //Products = _prodRepo.GetAll(includeProperties:"Category,ApplicationType"),
                Categories = _catRepo.GetAll(),
                Schools = _schoolRepo.GetAll()
            };
            return View(homeVM);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public IActionResult FindProduct(string SearchProduct)
        //public ViewResult OnGet()
        {
            
            if (string.IsNullOrEmpty(SearchProduct))
            {
                HomeVM homeVM = new HomeVM()
                {
                    //Products = _prodRepo.GetAll(includeProperties: "Category,ApplicationType"),
                    //Categories = _catRepo.GetAll()
                };
                return View("Index", homeVM);
            }
            else
            {
                HomeVM homeVM = new HomeVM()
                {
                // old code ==> Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType),
                //Products = _prodRepo.GetAll(u => u.Name.Contains(SearchProduct) || u.Category.Name.Contains(SearchProduct), 
                //        includeProperties: "Category,ApplicationType"),

                //Products = _prodRepo.Search(SearchProduct),
                //Categories = _catRepo.GetAll()
                };
                //SearchProduct = "";
                return View("Index", homeVM);
            }
            //return RedirectToAction(nameof(Index));

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public IActionResult FindCategory(string SearchCategory)
        {

            if (string.IsNullOrEmpty(SearchCategory))
            {
                HomeVM homeVM = new HomeVM()
                {
                    //Products = _prodRepo.GetAll(includeProperties: "Category,ApplicationType"),
                    //Categories = _catRepo.GetAll()
                };
                return View("Index", homeVM);
            }
            else
            {
                HomeVM homeVM = new HomeVM()
                {
                    // old code ==> Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType),
                    //Products = _prodRepo.GetAll(u => u.Category.Name.Contains(SearchCategory),
                    //    includeProperties: "Category,ApplicationType"),

                    //Products = _prodRepo.Search(SearchProduct),
                    //Categories = _catRepo.GetAll()
                };
                //SearchProduct = "";
                return View("Index", homeVM);
            }
            //return RedirectToAction(nameof(Index));

        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }


            DetailsVM DetailsVM = new DetailsVM()
            {
                // old code (without repository)
                //Product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType)
                //.Where(u => u.Id == id).FirstOrDefault(),
                //ExistsInCart = false
                //Product = _prodRepo.FirstOrDefault(u=> u.Id ==id, includeProperties: "Category,ApplicationType"),
                //ExistsInCart = false

                School = _schoolRepo.FirstOrDefault(u=> u.Id ==id, includeProperties: "Category"),
                ExistsInCart = false
            };

            foreach(var item in shoppingCartList)
            {
                if (item.ProductId == id)
                {
                    DetailsVM.ExistsInCart = true;
                }
            }

            return View(DetailsVM);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id, DetailsVM detailsVM)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            //shoppingCartList.Add(new ShoppingCart { ProductId = id, SqFt = detailsVM.Product.TempSqFt});
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            //TempData[WC.Success] = "Item added to cart successfully";
            TempData[WC.Success] = "Enter enrollment details";
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(actionName: "Index", controllerName: "Enroll", routeValues:id);
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);
            if (itemToRemove != null)
            {
                shoppingCartList.Remove(itemToRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            TempData[WC.Success] = "Item removed from cart successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
