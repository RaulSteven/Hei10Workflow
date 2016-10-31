using System.Web.Mvc;
using Hei10.Web.Framework.Extensions;

namespace Hei10.Web.Framework.Controllers
{
    public class WebSiteController :BaseController
    {
        public override string AreaName
        {
            get { return UrlHelperExtentions.ControllerDefault; }
        }

        [NonAction]
        public ActionResult RedirectTo404(string reUrl, string msg)
        {
            ViewBag.ReUrl = reUrl;
            ViewBag.Msg = msg;
            return View("NotFind");
        }
    }
}