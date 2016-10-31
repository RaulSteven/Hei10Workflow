using Hei10.Domain.Services;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Hei10.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            DependencyConfig.Register();
            AutoMapperConfig.Register();
            //在应用程序启动时运行的代码  
            log4net.Config.XmlConfigurator.Configure();
            MiniProfilerEF6.Initialize();
        }


        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)//这里是允许本地访问启动监控,可不写
            {
                MiniProfiler.Start();
            }
        }


        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthenticationSvc.GetCookieName()];
            if (authCookie != null)
            {
                var formAuthSvc = DependencyResolver.Current.GetService<IFormsAuthenticationSvc>();
                formAuthSvc.FromAuthenticationTicket(authCookie);
            }
        }
    }
}
