using Microsoft.AspNetCore.Http;
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

        
        [Display(Name = "Enroll Header Id")]
        public int EnrollHeaderId { get; set; }
        [ForeignKey("EnrollHeaderId")]
        public EnrollHeader EnrollHeader { get; set; }

        [Display(Name = "School Id")]
        public int SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public School School { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Log Date")]
        public DateTime LogDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
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
        [Display(Name = "Reason for Enrollment")]
        public string EnrollReason { get; set; }

        [Display(Name = "Consent Form")]
        public string ConsentForm { get; set; }

        //[Required]
        //[Display(Name = "School Profile Photo")]
        //public byte[] SchoolPhoto { get; set; }
        [Display(Name = "Profile Photo")]
        public string SchoolPhoto { get; set; }

        [Required]
        [Display(Name = "Emergency Contact Person")]
        public string EmergencyContactName { get; set; }
        [Required]
        [Display(Name = "Emergency Contact Number")]
        public string EmergencyContactNo { get; set; }
        public string EnrollStatus { get; set; }  // Saved, Pending, Approved, Processing, Cancelled, Disapproved

        //[Required(ErrorMessage = "Please choose Profile Photo")]
        //[Display(Name = "Profile Photo")]
        //[NotMapped]
        //public IFormFile ProfileImage  { get; set; }

    }
}
