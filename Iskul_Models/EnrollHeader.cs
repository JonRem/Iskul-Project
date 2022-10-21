using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iskul_Models
{
    public class EnrollHeader
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [DisplayName("Enroll Date")]
        [Required]
        public DateTime EnrollDate { get; set; }
        [DisplayName("Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [DisplayName("Email Address")]
        [Required]
        public string Email { get; set; }
    }
}
