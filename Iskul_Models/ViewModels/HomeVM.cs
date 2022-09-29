using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iskul_Models.ViewModels
{
    public class HomeVM
    {
        //public IEnumerable<Product> Products { get; set; }
        public IEnumerable<School> Schools { get; set; }
        //public IEnumerable<Member> Members { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        //public string SearchProduct { get; set; }
        //public string SearchCategory { get; set; }
    }
}
