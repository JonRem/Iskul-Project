using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iskul_Models.ViewModels
{
    public class SchoolUserVM
    {
        public SchoolUserVM()
        {
            SchoolList = new List<School>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IList<School> SchoolList { get; set; }        
    }
}
    

