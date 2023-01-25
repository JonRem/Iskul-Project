using Braintree;
using Iskul_DataAccess.Repository.IRepository;
using Iskul_Models.ViewModels;
using Iskul_Models;
using Iskul_Utility.BrainTree;
using Iskul_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace Iskul.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class EnrollEvalController : Controller
    {
        private readonly IEnrollHeaderRepository _enrollHeaderRepo;
        private readonly IEnrollDetailRepository _enrollDetailRepo;

        [BindProperty]
        public EnListVM EnListVM { get; set; }
        public EnrollDataVM EnrollDataVM { get; set; }

        public EnrollEvalController(IEnrollHeaderRepository enrollHeaderRepo,
            IEnrollDetailRepository enrollDetailRepo)
        {
            _enrollHeaderRepo = enrollHeaderRepo;
            _enrollDetailRepo = enrollDetailRepo;
        }
        public IActionResult Index(string searchSchool = null, string searchLastName = null, 
            string searchFirstName = null,  string searchEmail = null, 
            string searchPhone = null, string Status = null)
        {
            EnListVM enListVM = new EnListVM()
            {
                EnrollHList = _enrollHeaderRepo.GetAll(includeProperties: "School"),
                StatusList = WC.AppStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };

            if (!string.IsNullOrEmpty(searchSchool))
            {
                enListVM.EnrollHList = enListVM.EnrollHList.Where(u => u.School.SchoolName.ToLower().Contains(searchSchool.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchLastName))
            {
                enListVM.EnrollHList = enListVM.EnrollHList.Where(u => u.LastName.ToLower().Contains(searchLastName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchFirstName))
            {
                enListVM.EnrollHList = enListVM.EnrollHList.Where(u => u.FirstName.ToLower().Contains(searchFirstName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                enListVM.EnrollHList = enListVM.EnrollHList.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPhone))
            {
                enListVM.EnrollHList = enListVM.EnrollHList.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPhone))
            {
                enListVM.EnrollHList = enListVM.EnrollHList.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
            }
            if (!string.IsNullOrEmpty(Status) && Status != "--Application Status--")
            {
                Boolean StatusBool = true;
                if (Status == "Open")
                {
                    StatusBool = true;
                }
                else
                {
                    StatusBool = false;
                }
               enListVM.EnrollHList = enListVM.EnrollHList.Where(u => u.DetailRecOpen == StatusBool);
            }

            return View(enListVM);
        }
        public IActionResult Details(int id, string Status = null)
        {
            EnrollDataVM = new EnrollDataVM()
            {
                EnrollHeader = _enrollHeaderRepo.FirstOrDefault(u => u.Id == id, includeProperties: "School"),
                EnrollDetailList = _enrollDetailRepo.GetAll(o => o.EnrollHeaderId == id),
                ApplicationStatusList = WC.listStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };

            if (!string.IsNullOrEmpty(Status) && Status != "--Application Status--")
            {
                EnrollDataVM.EnrollDetailList = EnrollDataVM.EnrollDetailList.Where(u => u.EnrollStatus == Status);
            }
            return View(EnrollDataVM);
        }
    }
    
}




//        [HttpPost]
//        public IActionResult StartProcessing()
//        {
//            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
//            orderHeader.OrderStatus = WC.StatusInProcess;
//            _orderHRepo.Save();
//            TempData[WC.Success] = "Order is in Process";
//            return RedirectToAction(nameof(Index));
//        }
//        [HttpPost]
//        public IActionResult ShipOrder()
//        {
//            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
//            orderHeader.OrderStatus = WC.StatusShipped;
//            orderHeader.ShippingDate = DateTime.Now;
//            _orderHRepo.Save();
//            TempData[WC.Success] = "Order Shipped Successfully";
//            return RedirectToAction(nameof(Index));
//        }
//        [HttpPost]
//        public IActionResult CancelOrder()
//        {
//            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

//            var gateway = _brain.GetGateway();
//            Transaction transaction = gateway.Transaction.Find(orderHeader.TransactionId);

//            if (transaction.Status == TransactionStatus.AUTHORIZED || transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
//            {
//                //no refund
//                Result<Transaction> resultvoid = gateway.Transaction.Void(orderHeader.TransactionId);
//            }
//            else
//            {
//                //refund
//                Result<Transaction> resultRefund = gateway.Transaction.Refund(orderHeader.TransactionId);
//            }
//            orderHeader.OrderStatus = WC.StatusRefunded;
//            _orderHRepo.Save();
//            TempData[WC.Success] = "Order Cancelled Successfully";

//            return RedirectToAction(nameof(Index));
//        }
//        [HttpPost]
//        public IActionResult UpdateOrderDetails()
//        {
//            OrderHeader orderHeaderFromDb = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
//            orderHeaderFromDb.FullName = OrderVM.OrderHeader.FullName;
//            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.FullName;
//            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
//            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
//            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
//            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
//            orderHeaderFromDb.Email = OrderVM.OrderHeader.Email;

//            _orderHRepo.Save();
//            TempData[WC.Success] = "Order Details Updated Successfully";

//            return RedirectToAction("Details", "Order", new { id = orderHeaderFromDb.Id });
//        }

//    }
//}
