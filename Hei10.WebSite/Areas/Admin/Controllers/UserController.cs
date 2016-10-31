using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Hei10.Core.Extensions;
using Hei10.Core.Utilities;
using Hei10.Domain.Entityframework;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;
using Hei10.WebSite.Areas.Admin.Models;
using Newtonsoft.Json;
using Hei10.Web.Framework.Security;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        public IUsersRepository UsersRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IUserRoleRepository UserRoleRepository { get; set; }
        public IUser2RoleRepository User2RoleRepository { get; set; }
        public ISysApartmenSvc SysApartSvc { get; set; }
        public IUser2ApartmentRepository User2ApartRepository { get; set; }

        [ValidatePage]
        public ActionResult Index(string keyWord, Gender? gender,string orderField = "CreateTime", string orderDirection= "desc", int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.KeyWord = keyWord;
            ViewBag.Gender = gender;
            return PartialView("_List", UsersRepository.GetPagedList((keyWord ?? "").Trim(), gender, orderField, orderDirection, pageCurrent, pageSize));
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(int userId = 0)
        {
            var model = new AdminModel();
            if (userId > 0)
            {
                var user = await UsersRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                Mapper.Map(user, model);
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(AdminModel model)
        {
            var json = new JsonModel();
            // 修改时，不需要修改密码
            if (model.Id > 0)
            {
                ModelState.Remove("Password");
                ModelState.Remove("PasswordConfirm");
                ModelState.Remove("LoginName");
            }
            var existLoginName = await UsersRepository.ExistLoginNameAsync(model.Id, model.LoginName);
            if (existLoginName)
            {
                ModelState.AddModelError("LoginName", "用户名已存在");
            }
            // 数据有误
            if (!ModelState.IsValid)
            {
                json.GetError(ModelState);
                return Json(json);
            }
            var operationType = OperationType.Insert;
            var operationTitle = "添加管理员信息";
            Users user = null;
            if (model.Id > 0)
            {
                user = await UsersRepository.GetByIdAsync(model.Id);
                if (user == null)
                {
                    json.message = "记录不存在了";
                    return Json(json);
                }
                operationType = OperationType.Update;
                operationTitle = "修改管理员信息";
            }
            else
            {
                user = new Users();
            }
            Mapper.Map(model, user);
            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordSalt = HashUtils.GenerateSalt();
                user.Password = HashUtils.HashPasswordWithSalt(model.Password, user.PasswordSalt);
            }
            await UsersRepository.SaveAsync(user);
            await LogRepository.Insert(TableSource.Users, operationType, operationTitle, user.Id);
            await AttachmentSvc.BoundAttachment(user.Id, user.HeadImg);
            return Json(json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> BatchPassReset(string ids)
        {
            var json = new JsonModel { message = "操作失败！", statusCode = 300 };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(json);
            }
            var list = await UsersRepository.BatchPasswordResetAsync(ids);
            if (list == null || list.Count <= 0)
            {
                return Json(json);
            }
            var msg = string.Join(",", list.Select(m => m.LoginName).ToArray()).ToEllipsis(100);
            //插入日志 
            await LogRepository.Insert(TableSource.Users, OperationType.Update, string.Format("批量重置{0}等密码", msg), ids);
            json.statusCode = 200;
            json.message = "操作成功！";
            json.closeCurrent = false;
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
            var list = await UsersRepository.BatchDeleteAsync(ids);
            if (list == null)
            {
                return Json(result);
            }
            var msg = string.Join(",", list.Select(m => m.LoginName).ToArray()).ToEllipsis(100);
            await LogRepository.Insert(TableSource.Articles, OperationType.Delete, string.Format("批量删除{0}等管理员", msg), ids);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

        /// <summary>
        /// 远程效验 用户名
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]   //清除缓存 
        public async Task<ActionResult> CheckIsHaveName(string loginName)
        {
            var result = await UsersRepository.ExistLoginNameAsync(0, loginName);
            return result ? Json(new { error = "用户名已存在" }) : Json(new { ok = "用户名可以使用" });
        }

        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetRole(long userId)
        {
            var model = new SetRoleModel()
            {
                UserId = userId,
                SelectedRoleIdList = await User2RoleRepository.GetRoleIdListAsync(userId),
                UserRoleList = await UserRoleRepository.GetListAsync()
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetRole(long userId,string roleIds)
        {
            var result = new JsonModel();
            await User2RoleRepository.SaveListAsync(userId, roleIds);
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetApart(long id)
        {
            ViewBag.UserId = id;
            var data = await SysApartSvc.GetJsonListAsync(id);
            return PartialView("SetApart", JsonConvert.SerializeObject(data));
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetApart(long id, string apartIds)
        {
            var result = new JsonModel();
            await User2ApartRepository.SaveListAsync(id, apartIds);
            result.message = "保存成功！";
            return Json(result);
        }
    }
}