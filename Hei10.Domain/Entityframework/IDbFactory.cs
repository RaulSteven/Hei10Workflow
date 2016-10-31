using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Entityframework
{
    public interface IDbFactory
    {
        Hei10Context Context { get; set; }

        Task<int> SaveChangesAsync();
    }
}
