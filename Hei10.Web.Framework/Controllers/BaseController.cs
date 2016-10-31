using System.Web.Mvc;
using Hei10.Web.Framework.Security;
using log4net;
using Hei10.Domain.ViewModels;
using Hei10.Core.Utilities;
using System.Web;

namespace Hei10.Web.Framework.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILog Log = null;

        public virtual string AreaName { get; }

        public BaseController()
        {
            Log = LogManager.GetLogger(GetType().FullName);
        }

        public new SysUser User
        {
            get
            {
                if (base.User is SysUser)
                {
                    return base.User as SysUser;
                }
                return new SysUser();
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.CurrUser = User;
            ViewBag.AreaName = AreaName;
        
            base.OnActionExecuted(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var ip = filterContext.HttpContext.Request != null
                ? filterContext.HttpContext.Request.UserHostAddress
                : "";
            var rawUrl = filterContext.HttpContext.Request != null && filterContext.HttpContext.Request.UrlReferrer != null
                ? filterContext.HttpContext.Request.UrlReferrer.ToString()
                : "";
            Log.FatalFormat("\r\n controller:{0},action:{1},IP:{2},RawUrl:{3},msg:{4}",
                RouteData.Values["controller"],
                RouteData.Values["action"],
                ip,
                rawUrl,
                filterContext.Exception);
        }
    }
}