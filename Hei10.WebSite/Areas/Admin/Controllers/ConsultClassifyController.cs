using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Hei10.Core.Extensions;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;
using Hei10.WebSite.Areas.Admin.Models;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class ConsultClassifyController : AdminController
    {
        public IConsultClassifyRepository ConsultClassifyRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        public ActionResult Index(string keyWord, string orderField, string orderDirection, int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.KeyWord = keyWord; 
            return PartialView("Index", ConsultClassifyRepository.GetPagedList((keyWord ?? "").Trim(), orderField, orderDirection, pageCurrent, pageSize));
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(string navtabId, long id = 0)
        {
            var model = new ConsultClassifyModel();
            if (id > 0)
            {
                var consultClassify = await ConsultClassifyRepository.GetByIdAsync(id);
                if (consultClassify == null)
                {
                    var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                Mapper.Map(consultClassify, model);
            }
            return PartialView(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(ConsultClassifyModel model)
        {
            var json = new JsonModel(); 
            // 数据有误
            if (!ModelState.IsValid)
            {
                json.GetError(ModelState);
                return Json(json);
            }
            var operationType = OperationType.Insert;
            var operationTitle = "添加咨询分类信息";
            ConsultClassify consultClassify = null;
            if (model.Id > 0)
            {
                consultClassify = await ConsultClassifyRepository.GetByIdAsync(model.Id);
                if (consultClassify == null)
                {
                    json.message = "记录不存在了";
                    return Json(json);
                }
                operationType = OperationType.Update;
                operationTitle = "修改咨询分类信息";
            }
            else
            {
                consultClassify = new ConsultClassify();
            }
            Mapper.Map(model, consultClassify);
            consultClassify.CommonStatus = CommonStatus.Enabled;
            await ConsultClassifyRepository.SaveAsync(consultClassify);
            await LogRepository.Insert(TableSource.ConsultClassifies, operationType, operationTitle, consultClassify.Id); 
            return Json(json);
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
            var list = await ConsultClassifyRepository.BatchDeleteAsync(ids);
            if (list == null)
            {
                return Json(result);
            }
            var msg = string.Join(",", list.Select(m => m.Title).ToArray()).ToEllipsis(100);
            await LogRepository.Insert(TableSource.ConsultClassifies, OperationType.Delete, string.Format("批量删除{0}等咨询分类", msg), ids);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }
    }
}