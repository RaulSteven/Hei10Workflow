using System;
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
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class ConsultController : AdminController
    {
        public IConsultRepository ConsultRepository { get; set; }
        public IConsultClassifyRepository ConsultClassifyRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        [ValidatePage]
        public async Task<ActionResult> Index(string keyWord, long? consultClassifyId, string orderField = "Consult.CreateTime", string orderDirection = "desc", int pageCurrent = 1, int pageSize = 30)
        { 
            ViewBag.KeyWord = keyWord;
            ViewBag.ConsultClassifyId = consultClassifyId;
            ViewBag.Select = await ConsultClassifyRepository.GetListBySelectAsync();
            return PartialView("Index", ConsultRepository.GetPagedList(ConsultClassifyRepository.QueryUnDelete(), (keyWord??"").Trim(), consultClassifyId, orderField, orderDirection, pageCurrent, pageSize));
        }

        [ValidateButton(Buttons = SysButton.Browse, ActionName = "Index")]
        public async Task<ActionResult> Detail(long id)
        {
            var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
            if (id < 1)
            {
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            var model = await ConsultRepository.GetUnDeleteByIdAsync(id);
            if (model == null)
            {
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            var consultClassify = await ConsultClassifyRepository.GetByIdAsync(model.ClassifyId);
            ViewBag.ConsultClassifyName = consultClassify == null ? "" : consultClassify.Title;
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Delete, ActionName = "Index")]
        public async Task<ActionResult> BatchDelete(string ids)
        { 
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(result);
            }
            var list = await ConsultRepository.BatchDeleteAsync(ids);
            if (list == null)
            {
                return Json(result);
            }
            var msg = string.Join(",", list.Select(m => m.Title).ToArray()).ToEllipsis(100);
            await LogRepository.Insert(TableSource.Consults, OperationType.Delete, string.Format("批量删除{0}等咨询", msg), ids);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }
    }
}