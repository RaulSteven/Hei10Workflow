using System.Web.Mvc;
using Hei10.Domain.Enums;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Extensions;
using System.Linq;

namespace Hei10.Web.Framework.Security
{
    public class ValidateAdminLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var url = new UrlHelper(filterContext.RequestContext);
            var sysUser = filterContext.HttpContext.User as SysUser;
            if (sysUser == null || !sysUser.Identity.IsAuthenticated)
            {
                var requestUrl = filterContext.HttpContext.Request.RawUrl.ToLower();

                if (requestUrl.Contains(url.AdminHome().ToLower()))
                {
                    var loginUrl = url.Login();
                    filterContext.Result = new RedirectResult(loginUrl);
                    return;
                }
                var jsonModel = new JsonModel();
                jsonModel.statusCode = 301;
                jsonModel.message = "请重新登录！";
                var result = new JsonResult();
                result.Data = jsonModel;
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                filterContext.Result = result;
            }
        }

    }
}
