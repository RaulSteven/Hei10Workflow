using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using AutoMapper;
using Hei10.Domain.Services;
using Hei10.Domain.ViewModels;
using Newtonsoft.Json;
using Hei10.WebSite.Areas.Admin.Models;
using Hei10.Web.Framework.Security;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class SysApartController : AdminController
    {
        public ISysApartmentRepository SysApartRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }
        public IUserRoleRepository UserRoleRepository { get; set; }
        public IUserRole2ApartmentRepository UserRole2ApartRepository { get; set; }

        public ISysApartmentRepository SysApartmentRepository { get; set; }

        public ISysApartmenSvc SysApartmenSvc { get; set; }

        public IUser2ApartmentRepository User2ApartmentRepository { get; set; }

        public IUsersRepository UsersRepository { get; set; }

        [ValidatePage]
        public async Task<ActionResult> Index()
        {
            var data = await SysApartRepository.GetJsonList();
            return PartialView("Index",JsonConvert.SerializeObject(data));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Edit| SysButton.Add,ActionName ="Index")]
        public async Task<ActionResult> Save(SysApartModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            SysApartment apart = null;
            var operationType = OperationType.Insert;
            if (model.Id > 0)
            {
                operationType = OperationType.Update;
                apart = await SysApartRepository.GetEnableByIdAsync(model.Id);
                if (apart == null)
                {
                    result.statusCode = 300;
                    result.message = "该条数据不存在，请刷新重试！";
                    return Json(result);
                }
            }
            else
            {
                apart = new SysApartment();
            }
            apart = Mapper.Map(model, apart);
            apart.CommonStatus = CommonStatus.Enabled;
            await SysApartRepository.SaveAsync(apart);
            await LogRepository.Insert(TableSource.SysApartments, operationType, "", apart.Id);
            apart.IndexOfParent = await SysApartmentRepository.GetIndexOfParent(apart);
            result.Data = apart;
            result.message = "保存成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Delete,ActionName ="Index")]
        public async Task<ActionResult> Delete(long id)
        {
            var json = new JsonModel();
            await SysApartRepository.DeleteAsync(id);
            json.message = "删除成功！";
            return Json(json);
        }

        [ValidateButton(Buttons = SysButton.Grant,ActionName ="Index")]
        public async Task<ActionResult> SetRole(long apartId)
        {
            var model = new SetRoleModel()
            {
                UserId = apartId,
                SelectedRoleIdList = await UserRole2ApartRepository.GetRoleIdListAsync(apartId),
                UserRoleList = await UserRoleRepository.GetListAsync()
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant,ActionName ="Index")]
        public async Task<ActionResult> SetRole(long apartId, string roleIds)
        {
            var result = new JsonModel();
            await UserRole2ApartRepository.SaveListAsync(apartId, roleIds);
            result.message = "保存成功！";
            return Json(result);
        }
    }
}