using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hei10.Web.Framework.Security
{
    public class ValidatePageAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var url = new UrlHelper(filterContext.RequestContext);
            var sysUser = filterContext.HttpContext.User as SysUser;
            var targetUrl = $"/Admin/{filterContext.RouteData.Values["controller"]}/{filterContext.RouteData.Values["action"]}";
            sysUser.UserInfo.FindCurrentMenu(targetUrl);
            if (sysUser.UserInfo.CurrentMenu == null)
            {
                //跳转到无权限页面
                var noPerUrl = url.AdminNoPermission();
                filterContext.Result = new RedirectResult(noPerUrl);
                return;
            }
        }
    }
}
