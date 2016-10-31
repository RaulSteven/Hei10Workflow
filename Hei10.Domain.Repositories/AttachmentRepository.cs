using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.Domain.Repositories
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        /// <summary>
        /// 通过文件路径返回所有的记录
        /// </summary>
        /// <param name="filePaths">文件路径</param>
        /// <returns></returns>
        public Task<List<Attachment>> GetListAsync(string[] filePaths)
        {
            if (filePaths == null || filePaths.Length == 0)
            {
                return null;
            }
            return QueryUnDelete().Where(m => filePaths.Contains(m.FilePath))
                        .ToListAsync();
        }
    }
}
