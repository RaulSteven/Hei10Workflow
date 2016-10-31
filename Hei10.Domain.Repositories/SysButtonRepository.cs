//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Hei10.Domain.Infrastructure;
//using Hei10.Domain.Models;
//using Hei10.Domain.Entityframework;
//using System.Data.Entity;
//using Hei10.Core.Extensions;
//using PagedList;
//namespace Hei10.Domain.Repositories
//{
//    public class SysButtonRepository : Repository<SysButton>, ISysButtonRepository
//    {
//        public IPagedList<SysButton> GetList(int pageSize,int pageCurrent)
//        {
//            var list = QueryEnable()
//                .OrderBy(m => m.Sort)
//                .ToPagedList(pageCurrent, pageSize);
//            return list;
//        }

//        public Task<List<SysButton>> GetList()
//        {
//            var list = QueryEnable()
//                .OrderBy(m => m.Sort)
//                .ToListAsync();
//            return list;
//        }
//    }
//}
