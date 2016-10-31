using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Hei10.Domain.Enums;
using Hei10.Domain.Repositories;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;
using Hei10.WebSite.Areas.Admin.Models;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class ConfigController : AdminController
    {
        public ISysConfigRepository SysConfigRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        [ValidatePage]
        public ActionResult WebSiteSettings()
        {
            var oModel = new WebsiteSettingsModel();
            oModel.Init(SysConfigRepository);
            return PartialView(oModel);
        }

        [ValidateButton(Buttons = SysButton.Save, ActionName = "WebSiteSettings")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> WebSiteSettings(WebsiteSettingsModel model)
        {
            var result=new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            model.Save(SysConfigRepository);
            await LogRepository.Insert(TableSource.SysConfigures, OperationType.Update, "", 0);
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidatePage]
        public ActionResult TongJiSettings()
        {
            var oModel = new TongJiSettingsModel();
            oModel.Init(SysConfigRepository);
            return PartialView(oModel);
        }

        [ValidateButton(Buttons = SysButton.Save, ActionName = "TongJiSettings")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> TongJiSettings(TongJiSettingsModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            model.Save(SysConfigRepository);
            await LogRepository.Insert(TableSource.SysConfigures, OperationType.Update, "", 0);
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidatePage]
        public ActionResult UploadSetting()
        {
            var model = new UploadModel();
            model.Init(SysConfigRepository);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Save, ActionName = "UploadSetting")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UploadSetting(UploadModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            model.Save(SysConfigRepository);
            await LogRepository.Insert(TableSource.SysConfigures, OperationType.Update, "", 0);
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidatePage]
        public ActionResult IconsSetting()
        {
            var model = new IconsModel();
            model.Init(SysConfigRepository);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Save, ActionName = "IconsSetting")]
        [HttpPost]
        public async Task<ActionResult> IconsSetting(IconsModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            model.Save(SysConfigRepository);
            await LogRepository.Insert(TableSource.SysConfigures, OperationType.Update, "保存Icons", 0);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidatePage]
        public ActionResult StaffPicSetting()
        {
            var model = new StaffPicModel();
            model.Init(SysConfigRepository);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Save, ActionName = "StaffPicSetting")]
        [HttpPost]
        public async Task<ActionResult> StaffPicSetting(StaffPicModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            model.Save(SysConfigRepository);
            await LogRepository.Insert(TableSource.SysConfigures, OperationType.Update, "保存员工图片", 0);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidatePage]
        public ActionResult ArticleClassifySetting()
        {
            var model = new ArticleClassifySetModel();
            model.Init(SysConfigRepository);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Save, ActionName = "ArticleClassifySetting")]
        [HttpPost]
        public async Task<ActionResult> ArticleClassifySetting(ArticleClassifySetModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            model.Save(SysConfigRepository);
            await LogRepository.Insert(TableSource.SysConfigures, OperationType.Update, "保存首页文章分类配置", 0);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidatePage]
        public ActionResult ArticleSearchSetting()
        {
            var model = new ArticleSearchSetModel();
            model.Init(SysConfigRepository);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Save, ActionName = "ArticleSearchSetting")]
        [HttpPost]
        public async Task<ActionResult> ArticleSearchSetting(ArticleSearchSetModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            model.Save(SysConfigRepository);
            await LogRepository.Insert(TableSource.SysConfigures, OperationType.Update, "保存文章搜索分类配置", 0);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }
    }
}