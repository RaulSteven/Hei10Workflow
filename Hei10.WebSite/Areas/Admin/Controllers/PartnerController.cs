using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Repositories;
using Hei10.Domain.Models;
using Hei10.WebSite.Areas.Admin.Models;
using Hei10.Web.Framework.Security;
using Hei10.Domain.Enums;
using System.Threading.Tasks;
using AutoMapper;
using Hei10.Core.Extensions;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class PartnerController : AdminController
    {
        public IPartnerRepository PartnerRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        [ValidatePage]
        public ActionResult Index(string name,string orderField,string orderDirection,int pageSize=30, int pageIndex = 1)
        {
            ViewBag.Name = name;
            var list = PartnerRepository
                .GetList(name, orderField, orderDirection, pageIndex, pageSize);
            return View(list);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(long id = 0)
        {
            var model = new PartnerModel();
            if (id == 0)
            {
                return PartialView(model);
            }
            var partner = await PartnerRepository.GetEnableByIdAsync(id);
            if (partner == null)
            {
                var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            Mapper.Map(partner, model);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PartnerModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            Partner partner = null;
            var operationType = OperationType.Update;
            if (model.Id > 0)
            {
                operationType = OperationType.Update;
                partner = await PartnerRepository.GetEnableByIdAsync(model.Id);
                if (partner == null)
                {
                    result.statusCode = 300;
                    result.message = "该条数据不存在，请刷新重试！";
                    return Json(result);
                }
            }
            else
            {
                partner = new Partner();
            }
            Mapper.Map(model, partner);
            partner.CommonStatus = CommonStatus.Enabled;
            await PartnerRepository.SaveAsync(partner);
            await LogRepository.Insert(TableSource.Partner, operationType, "", partner.Id);
            result.Data = partner;
            result.message = "保存成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Delete, ActionName = "Index")]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(result);
            }
            var list = await PartnerRepository.BatchDeleteAsync(ids);
            if (list == null)
            {
                return Json(result);
            }
            var msg = string.Join(",", list.Select(m => m.Name).ToArray()).ToEllipsis(100);
            await LogRepository.Insert(TableSource.Partner, OperationType.Delete, string.Format("批量删除{0}等合作伙伴", msg), ids);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }
    }
}