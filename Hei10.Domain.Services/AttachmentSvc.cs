using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hei10.Core.Utilities;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;

namespace Hei10.Domain.Services
{
    public class AttachmentSvc : IAttachmentSvc
    {
        public IAttachmentRepository AttachmentRepository { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }

        public async Task<bool> Save(HttpPostedFileBase postedFile, Attachment oAtt, string fCats = null)
        {
            if (postedFile == null || postedFile.ContentLength <= 0)
            {
                return false;
            }
            var refFileName = "";
            try
            {
                oAtt.Name = Path.GetFileNameWithoutExtension(postedFile.FileName);
                if (oAtt.Name != null && oAtt.Name.Length > 50)
                {
                    oAtt.Name = oAtt.Name.Substring(0, 50);
                }
                var fileName = string.Format("{0}{1}", DateTime.Now.ToString("HHmmss"), Math.Abs((oAtt.Name + Guid.NewGuid()).GetHashCode()));
                refFileName = SaveFile(postedFile, fCats, fileName);
                oAtt.FilePath = refFileName;
                oAtt.FileSize = postedFile.ContentLength;
                oAtt.FileExt = Path.GetExtension(postedFile.FileName);
                oAtt.ViewCount = 0;
                oAtt.SortIndex = 0;
                await AttachmentRepository.SaveAsync(oAtt);
            }
            catch (Exception ex)
            {
                var f = GetFullPath(refFileName);
                if (File.Exists(f)) File.Delete(f);
                return false;
            }
            return true;
        }

        public string GetFullPath(string path)
        {
            return Path.Combine(SysConfigRepository.UploadPath, path);
        }

        public string SaveFile(HttpPostedFileBase fileBase, string fCats, string name)
        {
            var fExt = Path.GetExtension(fileBase.FileName);
            var fReleative = BuildNewReleativeFileName(fCats, name, fExt);
            var fAbs = Path.Combine(SysConfigRepository.UploadPath, fReleative);
            var fDir = Path.GetDirectoryName(fAbs);
            if (!Directory.Exists(fDir)) Directory.CreateDirectory(fDir);
            fileBase.SaveAs(fAbs);
            return fReleative;
        }
        private string BuildNewReleativeFileName(string fileCatName, string name, string ext)
        {
            if (String.IsNullOrEmpty(fileCatName)) fileCatName = "default";
            DateTime dt = DateTime.Now;
            if (!ext.StartsWith(".")) ext = "." + ext;
            return String.Format(@"{0}\{1}_{2}\{3}\{4}{5}", fileCatName, dt.Year, dt.Month.ToString("00"), dt.Day, name, ext);
        }

        public string GetFileContentType(string ext)
        {
            string contentType = "text/plain"; // text/html
            switch (ext.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".tif":
                    contentType = "image/tiff";
                    break;
                case ".swf":
                case ".flv":
                    contentType = "application/x-shockwave-flash";
                    break;
                case ".ico":
                    contentType = "image/x-icon";
                    break;
                case ".js":
                    contentType = "application/javascript";
                    break;
                case ".css":
                    contentType = "text/css";
                    break;
                case ".html":
                case ".htm":
                    contentType = "text/html";
                    break;
                case ".xml":
                    contentType = "text/xml";
                    break;
                case ".apk":
                    contentType = "application/vnd.android.package-archive";
                    break;
                case ".rar":
                    contentType = "application/x-rar";
                    break;
                case ".zip":
                    contentType = "application/zip";
                    break;
                case ".arj":
                    contentType = "application/x-arj";
                    break;
                case ".gz":
                    contentType = "application/x-gzip";
                    break;
                case ".z":
                    contentType = "application/x-compress";
                    break;
                case ".7z":
                    contentType = "application/x-7z-compressed";
                    break;
                case ".doc":
                    contentType = "application/msword";
                    break;
                case ".docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".dotx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                    break;
                case ".dot":
                    contentType = "text/vnd.graphviz";
                    break;
                case ".pdf":
                    contentType = "application/pdf";
                    break;
            }
            return contentType;

        }

        /// <summary>
        /// 绑定当前记录所用到的附件
        /// </summary>
        /// <param name="id">当前记录的Id</param>
        /// <param name="url">当前记录图片地址，以“,”为分割</param>
        /// <param name="content">编辑器中的内容（如果有插入图片）</param>
        /// <param name="split">分割符</param>
        public async Task<string> BoundAttachment(long id, string url, string content = "", char split = ',')
        {
            if (!string.IsNullOrEmpty(content))
            {
                url = url + split + StringUtility.GetEditorImgUrl(content);
            }
            if (id < 1 || string.IsNullOrEmpty(url))
            {
                return "";
            }
            var filePaths = url.Split(split)
                               .Where(m => !string.IsNullOrEmpty(m))
                               .ToArray();
            if (filePaths.Length == 0)
            {
                return "";
            }

            var list = await AttachmentRepository.GetListAsync(filePaths);
            if (!list.Any())
            {
                return "";
            }
            foreach (var item in list)
            { 
                item.SourceId = id;
            }
            await AttachmentRepository.SaveChangesAsync();
            return "";
        }
    }
}
