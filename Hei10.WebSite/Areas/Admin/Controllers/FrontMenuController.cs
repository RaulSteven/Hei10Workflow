using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Repositories;
using Hei10.Web.Framework.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hei10.Domain.Enums;
using AutoMapper;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class FrontMenuController : AdminController
    {
        public IFrontMenuRepository FrontMenuRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        [ValidatePage]
        public async Task<ActionResult> Index()
        {
            var data = await FrontMenuRepository.GetJsonListAsync();
            return PartialView("Index", JsonConvert.SerializeObject(data));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Save(FrontMenuModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            FrontMenu menu = null;
            var operationType = OperationType.Insert;
            if (model.Id > 0)
            {
                operationType = OperationType.Update;
                menu = await FrontMenuRepository.GetByIdAsync(model.Id);
                if (menu == null)
                {
                    result.statusCode = 300;
                    result.message = "该条数据不存在，请刷新重试！";
                    return Json(result);
                }
            }
            else
            {
                menu = new FrontMenu();
            }
            menu = Mapper.Map(model, menu);
            menu.CommonStatus = CommonStatus.Enabled;
            await FrontMenuRepository.SaveAsync(menu);
            await LogRepository.Insert(TableSource.FrontMenu, operationType, "", menu.Id);
            menu.IndexOfParent =await FrontMenuRepository.GetIndexOfParent(menu);
            result.Data = menu;
            result.message = "保存成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Delete, ActionName = "Index")]
        public async Task<ActionResult> Delete(long id)
        {
            var json = new JsonModel();
            await FrontMenuRepository.DeleteAsync(id);
            json.message = "删除成功！";
            return Json(json);
        }
    }
}