using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Iskul_Models
{
    public class Member
    {
        
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime DateofBirth { get; set; }
        public string Address { get; set; }
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Civil Status")]
        public string CivilStatus { get; set; }
        [Display(Name = "Occupation")]
        public string Occupation { get; set; }
        //[Display(Name = "Member Status")]
        //public string MemberStatus { get; set; }
        // active/inactive/expelled/lifetime member
        //[Display(Name = "Member Degree")]
        //public string MemberDegree { get; set; }
        [Display(Name = "Emergency Contact Info")]
        public string EmergencyContact { get; set; }
        public string Photo { get; set; }
        public string Notes { get; set; }
        //public string Description { get; set; }
        //[Range(1,int.MaxValue)]
        //public double Price { get; set; }
        //public string Image { get; set; }

        //[Display(Name="Category Type")]
        //public int CategoryId { get; set; }
        //[ForeignKey("CategoryId")]
        //public virtual Category Category { get; set; }

        //[Display(Name="Application Type")]
        //public int ApplicationTypeId { get; set; }
        //[ForeignKey("ApplicationTypeId")]
        //public virtual ApplicationType ApplicationType { get; set; }

        //[NotMapped]
        //[Range(1,10000,ErrorMessage ="Sqft must be greater than 0.")]
        //public int TempSqFt { get; set; }
    }
}
