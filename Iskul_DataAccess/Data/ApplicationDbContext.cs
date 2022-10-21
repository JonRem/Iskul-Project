using Iskul_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iskul_DataAccess
{
    public class ApplicationDbContext : IdentityDbContext 
    {
        //internal readonly object InquiryDetail;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<EnrollHeader> EnrollHeader { get; set; }
        public DbSet<EnrollDetail> EnrollDetail { get; set; }
        //public DbSet<ApplicationType> ApplicationType { get; set; }
        //public DbSet<Product> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        //public DbSet<InquiryHeader> InquiryHeader { get; set; }
        //public DbSet<InquiryDetail> InquiryDetail { get; set; }
        //public DbSet<OrderHeader> OrderHeader { get; set; }
        //public DbSet<OrderDetail> OrderDetail { get; set; }

    }
}
