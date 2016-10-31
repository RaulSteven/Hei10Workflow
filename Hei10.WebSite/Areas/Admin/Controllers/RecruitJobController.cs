using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;
using Hei10.Domain.Entityframework;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class RecruitJobController : AdminController
    {
        public IRecruitJobRepository RecruitJobRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        [ValidatePage]
        public ActionResult Index(string name,string orderField, string orderDirection, int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.name = name;
            var list = RecruitJobRepository.GetPagedList(pageCurrent, pageSize, orderField, orderDirection,name);
            return PartialView(list);
        }

        [ValidateButton(Buttons = SysButton.Delete, ActionName = "Index")]
        [HttpPost]
        public async Task<ActionResult> BatchDelete(string ids)
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
            await RecruitJobRepository.BatchDeleteAsync(idString);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(long id = 0)
        {
            RecruitJob model;
            if (id > 0)
            {
                model = await RecruitJobRepository.GetByIdAsync(id);
                if (model == null)
                {
                    var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                model = DbFactory.CreateRecruitJob();
                model.WorkDuty = "1."+ Environment.NewLine + "2."+ Environment.NewLine+"3.";
                model.Requirement= "1." + Environment.NewLine + "2." + Environment.NewLine + "3.";
            }
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(RecruitJob model)
        {
            var result = new JsonModel();
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            var operationType = OperationType.Insert;
            var operationTitle = "添加招聘职位";
            if (model.Id > 0)
            {
                var adPos = await RecruitJobRepository.GetByIdAsync(model.Id);
                if (adPos == null)
                {
                    result.message = "记录不存在了";
                    return Json(result);
                }
                operationType = OperationType.Update;
                operationTitle = "修改招聘职位";
            }
            await RecruitJobRepository.SaveAsync(model);
            await LogRepository.Insert(TableSource.RecruitJob, operationType, operationTitle, model.Id);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }
    }
}