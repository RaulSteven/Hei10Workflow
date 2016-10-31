using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Core.Utilities;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Enums;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Repositories;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class CommonController : AdminController
    {
        public IUsersRepository UsersRepository { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }

        public ActionResult Delete(long id,TableSource src)
        {
            var jsonModel = new JsonModel();
            UsersRepository.Factory.Context.Database.ExecuteSqlCommand($"Update {src} set CommonStatus= {(int)CommonStatus.Deleted} where Id={id}");
            jsonModel.message = "删除成功！";
            jsonModel.closeCurrent = false;
            return Json(jsonModel);
        }

        [HttpPost]
        public ActionResult BatchDelete(string ids, TableSource src)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(result);
            }
            var arr = StringUtility.ConvertToBigIntArray(ids, ',');
            if (arr == null || !arr.Any())
            {
                return Json(result);
            }
            var idString = string.Join(",", arr);   
            var commandResult = UsersRepository.Factory.Context.Database.ExecuteSqlCommand($"Update {src} set CommonStatus= {(int)CommonStatus.Deleted} where Id in ({idString})");
            if (commandResult < 1)
            {
                return Json(result);
            }
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

        public ActionResult SelectIcon()
        {
            return PartialView(SysConfigRepository.Icons);
        }
    }
}