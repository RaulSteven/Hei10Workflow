using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kaliko.ImageLibrary;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.ViewModels;

namespace Hei10.Web.Framework.Extensions
{
    public static class UrlHelperExtentions
    {
        public const string ControllerDefault = "Default";
        public const string AdminDefault = "Admin_default";
        public const string DefaultJavascriptLink = "javascript:;";
        public const string ArticleListDefault = "Article_List_Default";
        public const string ArticleDetailDefault = "Article_Detail_Default";
        public const string ConsultDefault = "Consult_Default"; 

        //首页
        public const string HomeRoute = "HomeRoute";
        //#region // Helpers

        public static string GenerateUrl(this UrlHelper urlHelper, string routeName, string actionName, string controllerName, object values)
        {
            return UrlHelper.GenerateUrl(
                routeName,
                actionName,
                controllerName,
                new RouteValueDictionary(values),
                RouteTable.Routes,
                urlHelper.RequestContext,
                true);
        }

        //#endregion

        #region // 登陆与退出

        public static string Login(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(AdminDefault, "Login", "Account",null);
        }
        public static string AdminLogin(this UrlHelper urlHelper, string returnUrl = "")
        {
            if (String.IsNullOrEmpty(returnUrl)) return urlHelper.GenerateUrl(AdminDefault, "Login", "Account", null);
            return urlHelper.GenerateUrl(AdminDefault, "Login", "Account", new { returnUrl });
        }

        public static string Logout(this UrlHelper urlHelper, string gotoUrl = "")
        {
            gotoUrl = gotoUrl ?? "/";
            return urlHelper.GenerateUrl(AdminDefault, "Logout", "Account", new { gotoUrl });
        }

        public static string AdminLogout(this UrlHelper urlHelper, string returnUrl = "")
        {
            if (String.IsNullOrEmpty(returnUrl)) return urlHelper.GenerateUrl(AdminDefault, "Logout", "Account", null);
            return urlHelper.GenerateUrl(AdminDefault, "Logout", "Account", new { returnUrl });
        }

        #endregion

        #region // 管理页

        public static string AdminHome(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(AdminDefault, "Index", "Home", null);
        }

        /// <summary>
        /// 无权限
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="reUrl"></param>
        /// <returns></returns>
        public static string AdminNoPermission(this UrlHelper urlHelper, string reUrl = "")
        {
            if (String.IsNullOrEmpty(reUrl))
                return urlHelper.GenerateUrl(AdminDefault, "NoPermission", "Account", null);
            return urlHelper.GenerateUrl(AdminDefault, "NoPermission", "Account", new { reUrl });
        }

        public static string ShowFlowActivityList(this UrlHelper urlHelper,long proInsId)
        {
            return urlHelper.GenerateUrl(AdminDefault, "FlowActivityList", "Workflow", new { proInsId });
        }
        #endregion

        #region // 公共

        public static string GetFileUrl(this UrlHelper urlHelper, string fn)
        {
            if (string.IsNullOrEmpty(fn))
            {
                return "";
            }
            fn = StringUtility.XBase64Encode(fn);
            return urlHelper.GenerateUrl("GetFile", "GetFile", "Utility", new { fn });
        }

        public static string FileUrl(this UrlHelper urlHelper, string fn)
        {
            if (string.IsNullOrEmpty(fn))
            {
                return "";
            }
            fn = StringUtility.XBase64Encode(fn);
            var result = urlHelper.GenerateUrl("GetFile", "GetFile", "Utility", new { fn });
            return GetImgSiteFile(result);
        }

        private static string GetImgSiteFile(string result)
        {
            var configSvc = DependencyResolver.Current.GetService<ISysConfigRepository>();
            if (configSvc.ImgSites == null || configSvc.ImgSites.Length == 0)
            {
                return result;
            }
            var index = Math.Abs(result.GetHashCode() % configSvc.ImgSites.Length);
            return string.Format("{0}{1}", configSvc.ImgSites[index], result);
        }

        public static string ThumbUrl(this UrlHelper urlHelper, string fn, string size, ThumbnailMethod mode = ThumbnailMethod.Crop, int q = 90, string defUrl = "")
        {
            if (string.IsNullOrEmpty(fn))
            {
                return defUrl;
            }
            fn = StringUtility.XBase64Encode(fn);
            var result = urlHelper.GenerateUrl("Thumbnail", "Thumb", "Utility", new { size, mode, fn });
            return GetImgSiteFile(result);
        }

        public static string ThumbUrl(this UrlHelper urlHelper, string fn, string size, WaterMarkingPosition position, ThumbnailMethod mode = ThumbnailMethod.Crop, string defUrl = "")
        {
            if (string.IsNullOrEmpty(fn))
            {
                return defUrl;
            }
            fn = StringUtility.XBase64Encode(fn);
            var result = urlHelper.GenerateUrl("WaterThumbnail", "Thumb", "Utility", new { size, mode, fn, position });
            return GetImgSiteFile(result);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string GetVerifyCode(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(ControllerDefault, "GetVerifyCode", "Utility", null);
        }
        /// <summary>
        /// 图片管理
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string GetFileMana(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(AdminDefault, "FileMana", "Attachment", null);
        }


        #endregion

        #region 首页
        public static string Home(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(HomeRoute, "Index", "Home", null);
        }
        #endregion

        #region 文章
        public static string ArticleList(this UrlHelper urlHelper, long classifyId)
        {
            return urlHelper.GenerateUrl(ArticleListDefault, "Index", "Article", new { classifyId });
        }
        public static string ArticleDetail(this UrlHelper urlHelper, long id)
        {
            return urlHelper.GenerateUrl(ArticleDetailDefault, "Detail", "Article", new { id });
        }
        #endregion

        #region 咨询
        public static string Consult(this UrlHelper urlHelper, long id)
        {
            return urlHelper.GenerateUrl(ConsultDefault, "Index", "Consult", new { id });
        }
        #endregion

        #region 流程
        public static string FlowProcessing(this UrlHelper urlHelper, long proInsId)
        {
            return urlHelper.GenerateUrl(AdminDefault, "FlowProcessing", "WorkFlow", new { proInsId });
        }
        #endregion
    }
}
