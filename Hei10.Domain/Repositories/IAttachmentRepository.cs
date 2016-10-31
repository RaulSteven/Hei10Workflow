using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.Domain.Repositories
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        /// <summary>
        /// 通过文件路径返回所有的记录
        /// </summary>
        /// <param name="filePaths">文件路径</param>
        /// <returns></returns>
        Task<List<Attachment>> GetListAsync(string[] filePaths);
    }
}
