using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Repositories;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Models;
using Hei10.Domain.Enums;
using AutoMapper;
using Hei10.WebSite.Areas.Admin.Models;
using Hei10.Web.Framework.Security;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class SysMenuController : AdminController
    {
        public ISysMenuRepository SysMenuRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        // GET: Admin/SysApart
        [ValidatePage]
        public async Task<ActionResult> Index()
        {
            var data = await SysMenuRepository.GetJsonList();
            return PartialView("Index", JsonConvert.SerializeObject(data));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Edit | SysButton.Add, ActionName ="Index")]
        public async Task<ActionResult> Save(SysMenuModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            SysMenu menu = null;
            var operationType = OperationType.Insert;
            if (model.Id > 0)
            {
                operationType = OperationType.Update;
                menu = await SysMenuRepository.GetByIdAsync(model.Id);
                if (menu == null)
                {
                    result.statusCode = 300;
                    result.message = "该条数据不存在，请刷新重试！";
                    return Json(result);
                }
            }
            else
            {
                menu = new SysMenu();
            }
            menu = Mapper.Map(model, menu);
            if (!string.IsNullOrEmpty(menu.Url))
            {
                menu.Url = menu.Url.Trim().ToLower();
            }
            menu.CommonStatus = CommonStatus.Enabled;
            await SysMenuRepository.SaveAsync(menu);
            await LogRepository.Insert(TableSource.SysMenus, operationType, "", menu.Id);
            menu.IndexOfParent =await SysMenuRepository.GetIndexOfParent(menu);
            result.Data = menu;
            result.message = "保存成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Delete,ActionName ="Index")]
        public async Task<ActionResult> Delete(long id)
        {
            var json = new JsonModel();
            await SysMenuRepository.DeleteAsync(id);
            json.message = "删除成功！";
            return Json(json);
        }

        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public ActionResult SelectButtons()
        {
            return PartialView();
        }

    }
}