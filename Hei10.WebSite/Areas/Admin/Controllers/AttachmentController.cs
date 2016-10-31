using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Hei10.Core.Extensions;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Extensions;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class AttachmentController : AdminController
    {
        public ISysOperationLogRepository LogRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 实现批量上传
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcId"></param>
        /// <returns></returns>
        public async Task<JsonResult> BatchUpload(TableSource src, long srcId = 0,int type = 0)
        {
            var postedFile = Request.Files["file"];
            var table = new Hashtable();
            if (null != postedFile && postedFile.ContentLength > 0)
            {
                var oAtt = new Attachment
                {
                    Descript = "",
                    Source = src,
                    SourceId = 0,
                    CreateTime = DateTime.Now,
                    CreateUserId = User.UserInfo.UserId,
                    CreateUserName = User.UserInfo.UserName,
                    CreateIP = Request.UserHostAddress,
                    FilePath = "",
                    CommonStatus = CommonStatus.Enabled
                };
                if (await AttachmentSvc.Save(postedFile, oAtt))
                {
                    await LogRepository.Insert(TableSource.Attachments, OperationType.Insert, "", oAtt.Id);
                }
                table.Add("id", oAtt.Id);
                table.Add("imgPath", Url.FileUrl(oAtt.FilePath));
                table.Add("filePath", oAtt.FilePath);
                table.Add("base64Path", StringUtility.XBase64Encode(oAtt.FilePath));
                table.Add("name", oAtt.Name);
                table.Add("time", oAtt.CreateTime.ToDisplayDateTime());
                table.Add("size", oAtt.FileSizeStr);
                table.Add("type", type);
                table.Add("statusCode", 200);
            }
            else
            {
                table.Add("statusCode", 300);
                table.Add("message", "上传失败");
            }
            return Json(table, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> KindEditorUpload(TableSource src)
        {
            var postedFile = Request.Files["imgFile"];
            var table = new Hashtable();
            if (null != postedFile && postedFile.ContentLength > 0)
            {
                var oAtt = new Attachment
                {
                    Descript = "",
                    Source = src,
                    SourceId = 0,
                    CreateTime = DateTime.Now,
                    CreateUserId = User.UserInfo.UserId,
                    CreateUserName = User.UserInfo.UserName,
                    CreateIP = Request.UserHostAddress,
                    FilePath = "",
                    CommonStatus = CommonStatus.Enabled
                };
                if (await AttachmentSvc.Save(postedFile, oAtt))
                {
                    await LogRepository.Insert(TableSource.Attachments, OperationType.Insert, "", oAtt.Id);
                } 
                table.Add("url", Url.FileUrl(oAtt.FilePath)); 
                table.Add("error", 0);
            }
            else
            {
                table.Add("error", 1);
                table.Add("message", "上传失败");
            }
            return Json(table, JsonRequestBehavior.AllowGet);
        }
    }
}