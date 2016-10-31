using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Domain.Services;
using Hei10.Domain.Repositories;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Extensions;
using Hei10.WebSite.Models;
using Hei10.Web.Framework.Utility;
using Hei10.Domain.Models;
using Hei10.Domain.Enums;
using System.Threading.Tasks;
using Hei10.Core.Extensions;
using Hei10.Core.Utilities;
using Hei10.Domain.ViewModels;
using Hei10.WebSite.Areas.Admin.Models;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class AccountController : WebSiteController
    {

        public IUsersRepository UsersRepository { get; set; }
        public IFormsAuthenticationSvc FormsAuthSvc { get; set; }
        public ISysOperationLogRepository SysOperationLogRepository { get; set; }
        public ActionResult NoPermission()
        {
            return PartialView();
        }


        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(Url.AdminHome());
            }
            var model = new LoginModel();
            model.UserName = CookieUtils.GetCookie(FormsAuthSvc.GetUserNameCookieKey(), "");
            if (!string.IsNullOrEmpty(model.UserName))
            {
                model.IsRemember = true;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!VeryfyCodeUtility.IsVerifyCodeMatch(Session, model.ValidateCode))
            {
                ModelState.AddModelError("ValidateCode", "验证码错误！");
                return View(model);
            }

            var loginResult = await UsersRepository.Login(model.UserName, model.Password);
            if (loginResult.Status == SigninStatus.Succ)
            {
                FormsAuthSvc.CreateAuthenticationTicket(loginResult.UserInfo, Response, HttpContext, model.IsRemember);
                //添加到Fom
                if (model.IsRemember)
                {
                    CookieUtils.SetCookie(FormsAuthSvc.GetUserNameCookieKey(), model.UserName, true);
                }
                else
                {
                    CookieUtils.RemoveCookie(FormsAuthSvc.GetUserNameCookieKey());
                }
                //添加登录日志
                await SysOperationLogRepository.Insert(TableSource.Users, OperationType.UserLogin, "", "");
                return Redirect(Url.AdminHome());
            }

            ModelState.AddModelError("UserName", "登录失败！" + loginResult.Status.GetDescriotion());
            return View(model);
        }

        public ActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Action("Login"));
            }
            FormsAuthSvc.LogOut(User);
            return Redirect(Url.Action("Login"));
        }

        public ActionResult LoginDialog(string reUrl)
        {
            reUrl = reUrl ?? Url.AdminHome();
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(reUrl);
            }
            var model = new LoginModel();
            model.UserName = CookieUtils.GetCookie(FormsAuthSvc.GetUserNameCookieKey(), "");
            if (!string.IsNullOrEmpty(model.UserName))
            {
                model.IsRemember = true;
            }
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> LoginDialog(LoginModel model)
        {
            var result = new JsonModel();

            if (!VeryfyCodeUtility.IsVerifyCodeMatch(Session, model.ValidateCode))
            {
                ModelState.AddModelError("ValidateCode", "验证码错误！");
            }
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            var loginResult = await UsersRepository.Login(model.UserName, model.Password);
            if (loginResult.Status == SigninStatus.Succ)
            {
                FormsAuthSvc.CreateAuthenticationTicket(loginResult.UserInfo, Response, HttpContext, model.IsRemember);
                //添加到Fom
                if (model.IsRemember)
                {
                    CookieUtils.SetCookie(FormsAuthSvc.GetUserNameCookieKey(), model.UserName, true);
                }
                else
                {
                    CookieUtils.RemoveCookie(FormsAuthSvc.GetUserNameCookieKey());
                }
                //添加登录日志
                await SysOperationLogRepository.Insert(TableSource.Users, OperationType.UserLogin, "", "");
                result.message = "登录成功！";
                return Json(result);
            }

            result.statusCode = 300;
            result.message = "登录失败！" + loginResult.Status.GetDescriotion();
            //result.Error.Add(new ErorrModel() {Key="UserName",Value= "登录失败！" + loginResult.Status.GetDescriotion() });
            return Json(result);
        }
    }
}