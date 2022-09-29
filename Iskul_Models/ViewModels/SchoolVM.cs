using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iskul_Models.ViewModels
{
    public class SchoolVM
    {
        public School School { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
