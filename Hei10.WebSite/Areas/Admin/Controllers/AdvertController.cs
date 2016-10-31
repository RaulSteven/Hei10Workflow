using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Hei10.Core.Utilities;
using Hei10.Domain.Entityframework;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;
using Newtonsoft.Json;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class AdvertController : AdminController
    {
        public IAdPositionRepository AdPositionRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IAdvertRepository AdvertRepository { get; set; }

        [ValidatePage]
        public ActionResult Index()
        {
            var list = AdPositionRepository.GetListByZTreeAsync();
            return PartialView(list);
        }

        [ValidateButton(Buttons = SysButton.Delete, ActionName = "Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (id < 1)
            {
                return Json(result);
            }
            var model = await AdPositionRepository.GetUnDeleteByIdAsync(id);
            if (model == null)
            {
                return Json(result);
            }
            await AdPositionRepository.Delete(model);
            await LogRepository.Insert(TableSource.AdPositions, OperationType.Delete, "删除广告位" + model.Name, id);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(long id = 0)
        {
            AdPosition model;
            if (id > 0)
            {
                model = await AdPositionRepository.GetByIdAsync(id);
                if (model == null)
                {
                    var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                model = DbFactory.CreateAdPosition();
            }
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(AdPosition model)
        {
            ModelState.Remove("ViewName");
            var result = new JsonModel();
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            var operationType = OperationType.Insert;
            var operationTitle = "添加广告位";
            if (model.Id != 0)
            {
                var adPos = await AdPositionRepository.GetByIdAsync(model.Id);
                if (adPos == null)
                {
                    result.message = "记录不存在了";
                    return Json(result);
                }
                operationType = OperationType.Update;
                operationTitle = "修改广告位";
            }
            model.ViewName = string.Format("_{0}", model.Code);
            await AdPositionRepository.SaveAsync(model);
            await LogRepository.Insert(TableSource.Users, operationType, operationTitle, model.Id);
            await AttachmentSvc.BoundAttachment(model.Id, model.ImgUrl);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }

        public async Task<ActionResult> AdvertList(long id, string name, AdvertStatus? status, string orderField, string orderDirection, int pageCurrent = 1, int pageSize = 30)
        {
            var adPos = await AdPositionRepository.GetByIdAsync(id);
            if (adPos == null)
            {
                var json = new JsonModel { message = string.Format("找不到id为{0}的广告位！", id), statusCode = 300 };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            ViewBag.id = id;
            var list = AdvertRepository.GetList(id, name, status, orderField, orderDirection, pageCurrent, pageSize);
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
            await AdvertRepository.BatchDeleteAsync(idString);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> AdvertEdit(long adPosId, long id = 0)
        {
            var adPos = await AdPositionRepository.GetByIdAsync(adPosId);
            if (adPos == null)
            {
                var json = new JsonModel { message = string.Format("找不到id为{0}的广告位！", adPosId), statusCode = 300 };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            Advert model;
            if (id > 0)
            {
                model = await AdvertRepository.GetByIdAsync(id);
                if (model == null)
                {
                    var json = new JsonModel { message = string.Format("找不到id为{0}的广告！", id), statusCode = 300 };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                model.AdPosName = adPos.Name;
                model.Code = adPos.Code;
            }
            else
            {
                model = DbFactory.CreateAdvert(adPos.Id, adPos.Code, adPos.Name);
            }
            return PartialView(model);
        }
        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> AdvertEdit(Advert model)
        {
            if (model.EndTime != null && model.EndTime < model.StartTime)
            {
                ModelState.AddModelError("EndTime", "结束时间不能小于开始时间");
            }
            var result = new JsonModel();
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            var operationType = OperationType.Insert;
            var operationTitle = "添加广告";
            if (model.Id > 0)
            {
                var advert = await AdvertRepository.GetByIdAsync(model.Id);
                if (advert == null)
                {
                    result.message = string.Format("找不到id为{0}的广告！", model.Id);
                    result.statusCode = 300;
                    return Json(result);
                }
                operationType = OperationType.Insert;
                operationTitle = "编辑广告";
            }
            await AdvertRepository.SaveAsync(model);
            await AttachmentSvc.BoundAttachment(model.Id, model.ImgUrl, model.MetaContent);
            await LogRepository.Insert(TableSource.Adverts, operationType, operationTitle, model.Id);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }
    }
}