using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Hei10.WebSite
{
    public class BundleConfig
    {
        public static string JqueryJS = "~/Res/b-jui/BJUI/js/jqjs";

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(JqueryJS)
                .Include("~/Res/b-jui/BJUI/js/jquery-1.7.2.min.js")
                .Include("~/Res/b-jui/BJUI/js/jquery.cookie.js"));
        }
    }
}