using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Hei10.Core.Utilities;
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
    public class UCenterController : AdminController
    {
        public IUsersRepository UsersRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        #region //修改密码
        public async Task<ActionResult> ModifyPwd()
        {
            var model = new ModifyPwdModel();
            var user = await UsersRepository.GetByIdAsync(User.UserInfo.UserId);
            if (user == null)
            {
                var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ModifyPwd(ModifyPwdModel model)
        {
            var result = new JsonModel();
            var user = await UsersRepository.GetByIdAsync(User.UserInfo.UserId);
            if (user == null)
            {
                result.statusCode = 300;
                result.message = "该条数据不存在，请刷新重试！";
                return Json(result);
            }
            //原密码不正确
            if (!user.Password.Equals(HashUtils.HashPasswordWithSalt(model.OldPwssword, user.PasswordSalt)))
            {
                ModelState.AddModelError("OldPwssword", "原密码输入有误，请重新输入！");
            }
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordSalt = HashUtils.GenerateSalt();
                user.Password = HashUtils.HashPasswordWithSalt(model.Password, user.PasswordSalt);
            }
            await LogRepository.Insert(TableSource.Users, OperationType.Update, "修改密码", user.Id);
            await UsersRepository.SaveAsync(user);
            result.Data = user;
            result.message = "保存成功！";
            return Json(result);
        }


        #endregion

        #region //修改用户信息
        public async Task<ActionResult> ModifyInfo()
        {
            var user = await UsersRepository.GetByIdAsync(User.UserInfo.UserId);
            var model = Mapper.Map<Users, AdminModel>(user);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ModifyInfo(AdminModel model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("PasswordConfirm");
            var result = new JsonModel();
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }
            var oldModel = await UsersRepository.GetByIdAsync(User.UserInfo.UserId);
            if (oldModel == null)
            {
                result.statusCode = 300;
                result.message = "该条数据不存在，请刷新重试！";
                return Json(result);
            }
            oldModel = Mapper.Map(model, oldModel);
            await UsersRepository.SaveAsync(oldModel);
            await AttachmentSvc.BoundAttachment(model.Id, model.HeadImg);
            //插入日志
            await LogRepository.Insert(TableSource.Users, OperationType.Update, "修改个人资料", oldModel.Id);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }
        #endregion
    }
}