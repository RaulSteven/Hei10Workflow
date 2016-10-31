using System.Web;
using System.Web.Mvc;
using JR.Domain.Enums;
using JR.Domain.ViewModels;
using JR.Web.Framework.Extensions;

namespace JR.Web.Framework.Security
{
    public class ValidateUserLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var url = new UrlHelper(filterContext.RequestContext);
            var loginUrl = url.Login(filterContext.HttpContext.Request.RawUrl);
            var sysUser = filterContext.HttpContext.User as SysUser;
            if (sysUser == null || !sysUser.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult(loginUrl);
                return;
            }
        }

    }
}
