using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iskul_Models
{
    public class EnrollDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EnrollHeaderId { get; set; }

        [ForeignKey("EnrollHeaderId")]
        public EnrollHeader EnrollHeader { get; set; }

        [Required]
        public int SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public School School { get; set; }

        [Required]
        [Display(Name = "Log Date")]
        public DateTime LogDate { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateofBirth { get; set; }

        [Required]
        [Display(Name = "Street Address - House/Unit No.")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Province")]
        public string Province { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "True Contact Number")]
        public string ContactNumber { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Civil Status")]
        public string CivilStatus { get; set; }
        [Required]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }
        [Required]
        [Display(Name = "State Reason for Enrollment")]
        public string EnrollReason { get; set; }

        [Display(Name = "For Ages below 18, please attach Parents Consent Form here")]
        public string ConsentForm { get; set; }

        [Required]
        [Display(Name = "School Profile Photo")]
        public byte[] SchoolPhoto { get; set; }
        [Required]
        [Display(Name = "Emergency Contact Person")]
        public string EmergencyContactName { get; set; }
        [Required]
        [Display(Name = "Emergency Contact Number")]
        public string EmergencyContactNo { get; set; }
        public string EnrollStatus { get; set; }  // Saved, Pending, Approved, Processing, Cancelled, Disapproved
        
    }
}
