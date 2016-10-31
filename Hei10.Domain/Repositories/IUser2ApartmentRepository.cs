using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.Domain.Repositories
{
   public interface IUser2ApartmentRepository:IRepository<User2Apartment>
    {
        Task<List<long>> GetApartIdListAsync(long userId);
        Task SaveListAsync(long id, string apartIds);
    }
}
