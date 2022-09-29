using Microsoft.AspNetCore.Mvc.Rendering;
using Iskul_DataAccess.Repository.IRepository;
using Iskul_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iskul_Models;

namespace Iskul_DataAccess.Repository
{
    public class EnrollDetailRepository : Repository<EnrollDetail>, IEnrollDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public EnrollDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(EnrollDetail obj)
        {
            //var objFromDb = _db.Category.FirstOrDefault(u => u.Id == obj.Id);
            // if individual properties only ned to be updated then use previous method in CategoryRepository as an example
            _db.EnrollDetail.Update(obj);
        }
    }
}
