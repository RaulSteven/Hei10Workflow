using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kaliko.ImageLibrary;
using Hei10.Web.Framework.Extensions;

namespace Hei10.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            var namespaces = new[] { "Hei10.WebSite.Controllers" };
            #region 首页
            routes.MapRoute(
                name: UrlHelperExtentions.HomeRoute,
                url: "",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                },
                namespaces: namespaces
           );
            #endregion

            #region 工具类，如：图片缩略图、二维码、广告跳转、404、500
            routes.MapRoute(
                name: "Thumbnail",
                url: "Thumbnail/{fn}/{mode}/{size}/{q}",
                defaults: new
                {
                    controller = "Utility",
                    action = "Thumb",
                    q = 90,
                    size = UrlParameter.Optional,
                    mode = ThumbnailMethod.Crop
                },
                namespaces: namespaces
           );

            routes.MapRoute(
                 name: "WaterThumbnail",
                 url: "WaterThumb/{fn}/{mode}/{size}/{position}",
                 defaults: new
                 {
                     controller = "Utility",
                     action = "Thumb",
                     q = 90,
                     size = UrlParameter.Optional,
                     mode = ThumbnailMethod.Crop
                 },
                 namespaces: namespaces
            );

            routes.MapRoute(
                name: "GetFile",
                url: "File/{fn}",
                defaults: new
                {
                    controller = "Utility",
                    action = "GetFile"
                },
                namespaces: namespaces
            );

            routes.MapRoute(
                name: "NotFound",
                url: "404.html",
                defaults: new
                {
                    controller = "Home",
                    action = "NotFind"
                },
                namespaces: namespaces);
            routes.MapRoute(
                name: "Error",
                url: "Error.html",
                defaults: new
                {
                    controller = "Home",
                    action = "Error"
                },
                namespaces: namespaces);

            #endregion

            #region 加入建荣

            routes.MapRoute(
                name: "JoinIndex",
                url: "Join.html",
                defaults: new
                {
                    controller = "Join",
                    action = "Index",
                },
                namespaces: namespaces
           );

            routes.MapRoute(
                name: "JoinDetail",
                url: "Join/{id}.html",
                defaults: new
                {
                    controller = "Join",
                    action = "Detail",
                },
                namespaces: namespaces
           );

            #endregion

            #region 员工天地

            routes.MapRoute(
                name: "StaffIndex",
                url: "Staff.html",
                defaults: new
                {
                    controller = "Home",
                    action = "Staff",
                },
                namespaces: namespaces
           );
            #endregion

            #region 文章

            routes.MapRoute(
                name: UrlHelperExtentions.ArticleListDefault,
                url: "list/{classifyId}.html",
                defaults: new
                {
                    controller = "Article",
                    action = "Index",
                    classifyId = UrlParameter.Optional
                },
                namespaces: namespaces
           );
            routes.MapRoute(
                name: UrlHelperExtentions.ArticleDetailDefault,
                url: "page/{id}.html",
                defaults: new
                {
                    controller = "Article",
                    action = "Detail",
                    id = UrlParameter.Optional
                },
                namespaces: namespaces
           );
            #endregion

            #region 咨询

            routes.MapRoute(
                name: UrlHelperExtentions.ConsultDefault,
                url: "Consult/{id}.html",
                defaults: new
                {
                    controller = "Consult",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: namespaces
           );
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: namespaces
            );
        }
    }
}
