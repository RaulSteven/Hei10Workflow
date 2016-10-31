using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hei10.Domain.Models;

namespace Hei10.Domain.Services
{
    public interface IAttachmentSvc
    {
        Task<bool> Save(HttpPostedFileBase postedFile, Attachment oAtt, string fCats = null);
        string GetFullPath(string path);
        string GetFileContentType(string ext);

        /// <summary>
        /// 绑定当前记录所用到的附件
        /// </summary>
        /// <param name="id">当前记录的Id</param>
        /// <param name="url">当前记录图片地址，以“,”为分割</param>
        /// <param name="content">编辑器中的内容（如果有插入图片）</param>
        /// <param name="split">分割符</param>
        Task<string> BoundAttachment(long id, string url, string content = "", char split = ',');
    }
}
