using Hei10.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Models;

namespace Hei10.Domain.Services
{
    public interface ISysApartmenSvc
    {
        Task<List<SysApartJsonModel>> GetJsonListAsync(long userId);
    }
}
