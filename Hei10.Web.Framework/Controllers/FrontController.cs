using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Hei10.Domain.Repositories;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;

namespace Hei10.Web.Framework.Controllers
{
    public class FrontController : WebSiteController
    {
        public IFrontMenuRepository FrontMenuRepository { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var menuList = FrontMenuRepository.GetJsonListCache();
            ViewBag.MenuList = menuList;
            var targetUrl = Request.RawUrl;
            SetCurrMenu(targetUrl);

        }

        protected void SetCurrMenu(string targetUrl)
        {
            List<FrontMenuJsonModel> menuList = ViewBag.MenuList;
            FrontMenuJsonModel currMenu = null;
            FrontMenuJsonModel currPage = null;
            currMenu = menuList.FirstOrDefault(m => !string.IsNullOrEmpty(m.url)
                && m.url.Equals(targetUrl, StringComparison.OrdinalIgnoreCase));
            if (currMenu == null)
            {
                currMenu = menuList.FirstOrDefault(m => m.children != null
                    && m.children.Any(c => !string.IsNullOrEmpty(c.url)
                        && c.url.Equals(targetUrl, StringComparison.OrdinalIgnoreCase)));
                if (currMenu != null)
                {
                    currPage = currMenu.children.FirstOrDefault(c => !string.IsNullOrEmpty(c.url)
                        && c.url.Contains(targetUrl));
                }
            }
            ViewBag.CurrMenu = currMenu ?? new FrontMenuJsonModel();
            ViewBag.CurrPage = currPage ?? new FrontMenuJsonModel();
        }
    }
}
