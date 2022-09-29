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
    public class SchoolRepository : Repository<School>, ISchoolRepository
    {
        private readonly ApplicationDbContext _db;
        //private List<School> SchoolList { get; set; }

        public SchoolRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (obj == WC.CategoryName)
            {
                return _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            //if (obj == WC.ApplicationTypeName)
            //{
            //    return _db.ApplicationType.Select(i => new SelectListItem
            //    {
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    });
            //}
            return null;
        }

        //public IEnumerable<Product> Search(string searchProduct)
        //{

        //    //ProductList = IEnumerable < Product > GetAll(includeProperties: "Category,ApplicationType");




        //    if (string.IsNullOrEmpty(searchProduct))
        //    {
        //        return ProductList;
        //    }

        //    return ProductList.Where(u => u.Name.Contains(searchProduct) ||
        //            u.Name.Contains(searchProduct));

        //    // throw new NotImplementedException();
        //}

        public void Update(School obj)
        {
            // var objFromDb = _db.Category.FirstOrDefault(u => u.Id == obj.Id);
            // if individual properties only ned to be updated then use previous method in CategoryRepository as an example
            _db.School.Update(obj);
        }
    }
}
