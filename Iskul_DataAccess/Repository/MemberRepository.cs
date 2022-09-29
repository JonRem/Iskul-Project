using Iskul_DataAccess.Repository.IRepository;
using Iskul_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iskul_DataAccess.Repository
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        private readonly ApplicationDbContext _db;
        public MemberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Member obj)
        {
            //var objFromDb = _db.Category.FirstOrDefault(u => u.Id == obj.Id);
            var objFromDb = base.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                //objFromDb.Name = obj.Name;
                //objFromDb.DisplayOrder = obj.DisplayOrder;
            }
        }
    }
}
