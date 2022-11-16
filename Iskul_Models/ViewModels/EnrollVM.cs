using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iskul_Models.ViewModels
{
    public class EnrollVM
    {
        //public EnrollVM()
        //{
        //    ProductList = new List<Product>();
        //}

        //public IList<Product> ProductList { get; set; }

        public string AppUserId { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolImage { get; set; }
        public EnrollHeader EnrollHeader { get; set; }
        public EnrollDetail EnrollDetail { get; set; }
        public IEnumerable<SelectListItem> CivilStatusSelectList { get; set; }
        [Required]
        [Display(Name ="Profile Photo")]
        public IFormFile ProfileImage { get; set; }
        [Display(Name = "Consent Form")]
        public IFormFile ConsentForm { get; set; }
    }
}
    

