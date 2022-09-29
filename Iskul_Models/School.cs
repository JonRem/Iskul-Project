using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Iskul_Models
{
    public class School
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDesc { get; set; }
        public string Description { get; set; }
       
        public string Image { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "School Address")]

        public string SchoolAddress { get; set; }
        [Display(Name = "Contact Number")]

        public string SchoolContactNo { get; set; }

        [Display(Name = "Additional Notes")]
        public string Notes { get; set; }

        
    }
}
