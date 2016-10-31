using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Utility;
using Kaliko.ImageLibrary;
using Hei10.Core.Utilities;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using System.Threading.Tasks;

namespace Hei10.WebSite.Controllers
{
    public class UtilityController : WebSiteController
    {
        public IAttachmentSvc AttachmentSvc { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }
        public IWorkflowSvc WorkflowSvc { get; set; }

        public ActionResult GetVerifyCode()
        {
            return File(VeryfyCodeUtility.CreateVerifyImage(Session, 5), @"image/jpeg");
        }

        [OutputCache(CacheProfile = "FileCache")]
        public ActionResult Thumb(string size, ThumbnailMethod mode, int q, string fn, WaterMarkingPosition? position)
        {
            Response.Cache.SetOmitVaryStar(true);
            var filePath = StringUtility.XBase64Decode(fn);

            var arrSize = size.ToLower().Split('x');
            var dirRoot = AttachmentSvc.GetFullPath($@"thumb\{mode}-{size}-{position}");
            var filePathAbs = Path.Combine(dirRoot, filePath);
            var dirPath = Path.GetDirectoryName(filePathAbs);
            if (string.IsNullOrEmpty(dirPath))
            {
                return HttpNotFound("-File Not Found-");
            }
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var fileExt = Path.GetExtension(filePathAbs);
            if (!System.IO.File.Exists(filePathAbs))
            {
                var iThumb = new ImgThumbUtillity()
                {
                    QualityLevel = q
                };
                var filePathSrc = AttachmentSvc.GetFullPath(filePath);
                iThumb.MakeThumbnail(filePathSrc, filePathAbs, fileExt, StringUtility.ConvertToInt(arrSize[0]), StringUtility.ConvertToInt(arrSize[1]), mode, position, SysConfigRepository.WaterMarkingPath);
            }

            var fileContentType = AttachmentSvc.GetFileContentType(fileExt);
            return File(filePathAbs, fileContentType);
        }

        [OutputCache(CacheProfile = "FileCache")]
        public ActionResult GetFile(string fn)
        {
            Response.Cache.SetOmitVaryStar(true);
            var filePath = StringUtility.XBase64Decode(fn);
            filePath = AttachmentSvc.GetFullPath(filePath);
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound("-File Not Found-");
            }

            var fileExt = Path.GetExtension(filePath);
            var fileContentType = AttachmentSvc.GetFileContentType(fileExt);
            return File(filePath, fileContentType);
        }

        public async Task<ActionResult> TestInit()
        {
            await WorkflowSvc.InitTestProcess();
            return Content("ok");
        }

    }
}