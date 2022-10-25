using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iskul_Models.ViewModels
{
    public class EnrollUserVM
    {
        //public EnrollUserVM()
        //{
        //    ProductList = new List<Product>();
        //}
        public ApplicationUser ApplicationUser { get; set; }
        //public IList<Product> ProductList { get; set; }
        public School School { get; set; }
        public EnrollHeader EnrollHeader { get; set; }
        public EnrollDetail EnrollDetail { get; set; }
        public IEnumerable<SelectListItem> CivilStatusSelectList { get; set; }
    }
}
    

