using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iskul_Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            School = new School();
        }
        public School School { get; set; }
        public bool ExistsInCart  { get; set; }
    }
}
