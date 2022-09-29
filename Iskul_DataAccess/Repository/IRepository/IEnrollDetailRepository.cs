using Microsoft.AspNetCore.Mvc.Rendering;
using Iskul_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iskul_DataAccess.Repository.IRepository
{
    public interface IEnrollDetailRepository : IRepository<EnrollDetail>
    {
        void Update(EnrollDetail obj);

    }
}
