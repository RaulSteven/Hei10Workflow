using Hei10.Domain.Enums;
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
    public class ValidateButtonAttribute : ActionFilterAttribute
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public SysButton Buttons { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var url = new UrlHelper(filterContext.RequestContext);
            var sysUser = filterContext.HttpContext.User as SysUser;
            ControllerName = ControllerName ?? filterContext.RouteData.Values["controller"].ToString();
            ActionName = ActionName ?? filterContext.RouteData.Values["action"].ToString();
            var targetUrl = $"/Admin/{ControllerName}/{ActionName}";
            sysUser.UserInfo.FindCurrentMenu(targetUrl);
            if (sysUser.UserInfo.CurrentMenu == null
                || !sysUser.HasButton(Buttons))
            {
                if (filterContext.HttpContext.Request.RequestType
                    .Equals("Get", StringComparison.OrdinalIgnoreCase))
                {
                    //跳转到无权限页面
                    var noPerUrl = url.AdminNoPermission();
                    filterContext.Result = new RedirectResult(noPerUrl);
                }else
                {
                    var model = new JsonModel();
                    model.statusCode = 300;
                    model.message = "对不起，您没有权限进行此项操作！";
                    var result = new JsonResult();
                    result.Data = model;
                    filterContext.Result = result;
                }
                return;
            }
        }
    }
}
