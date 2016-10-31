using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Security;
using Hei10.Domain.Models;
using Hei10.Core.Cache;
using Hei10.Domain.Repositories;
using System.Web;
using Hei10.Domain.ViewModels;
using System.Web.Security;
using Hei10.Core.Utilities;
using Hei10.Domain.Infrastructure;
using System.Threading;

namespace Hei10.Domain.Services
{
    public class FormsAuthenticationSvc : IFormsAuthenticationSvc
    {
        public ICacheManager Cache { get; set; }
        public const int CacheTime = 10800;
        public IUsersRepository UserRep { get; set; }
        public IUserRoleSvc UserRoleSvc { get; set; }
        public ISysMenuSvc SysMenuSvc { get; set; }
        public IUser2ApartmentRepository User2ApartRepository { get; set; }
        public IUserRole2FilterRepository UserRole2FilterRepository { get; set; }

        public void CreateAuthenticationTicket(Users user, HttpResponseBase response, HttpContextBase httpContextBase, bool remember)
        {
            var serializeModel = GetModel(user);

            Cache.Add(user.GId.ToString(), serializeModel, CacheTime);
            var expiration = remember ? DateTime.Now.AddYears(1) : DateTime.Now.AddHours(1);
            var authTicket = new FormsAuthenticationTicket(
              1, user.GId.ToString(), DateTime.Now, expiration, remember, user.GId.ToString());
            string encTicket = FormsAuthentication.Encrypt(authTicket);

            CookieUtils.AddCookie(GetCookieName(), encTicket, remember);
            var userPrincipal = new SysUser(serializeModel);
            httpContextBase.User = userPrincipal;
        }

        private SysUserModel GetModel(Users user)
        {
            var roleIdList = UserRoleSvc.GetRoleIdList(user.Id);
            var menuList = SysMenuSvc.GetList(roleIdList);
            var apartIdList = User2ApartRepository.GetApartIdListAsync(user.Id).Result;
            var userFilterList = UserRole2FilterRepository.GetRoleFilterList(roleIdList);
            var serializeModel = new SysUserModel(user,roleIdList,menuList,apartIdList,userFilterList);

            return serializeModel;
        }

        public static string GetCookieName()
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            if (HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/admin", true, null))
            {
                cookieName += "_a";
            }
            return cookieName;
        }

        public string GetUserNameCookieKey()
        {
            return FormsAuthentication.FormsCookieName + "_uname";
        }


        public void FromAuthenticationTicket(HttpCookie cookie)
        {
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
            if (authTicket == null)
            {
                return;
            }
            var userGid = authTicket.UserData;
            var serializeModel = Cache.Get<SysUserModel>(userGid);
            if (serializeModel == null)
            {
                var user = UserRep.GetByGid(userGid).Result;
                if (user == null)
                {
                    Cache.Remove(userGid);
                    FormsAuthentication.SignOut();
                    return;
                }
                serializeModel = GetModel(user);

                Cache.Add(user.GId.ToString(), serializeModel, CacheTime);
            }

            var userPrincipal = new SysUser(serializeModel);
            HttpContext.Current.User = userPrincipal;
            Thread.CurrentPrincipal = userPrincipal;
        }

        public void LogOut(IJRUser user)
        {
            var cookieName = GetCookieName();
            CookieUtils.RemoveCookie(cookieName);
            if (user != null)
            {
                Cache.Remove(user.UserInfo.GId);
            }
            FormsAuthentication.SignOut();
        }
    }
}
