using Iskul_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iskul_DataAccess.Repository.IRepository
{
    public interface IMemberRepository : IRepository<Member>
    {
        void Update(Member obj);
    }
}
