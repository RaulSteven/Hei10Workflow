using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using System.Threading.Tasks;
using Hei10.WebSite.Areas.Admin.Models;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        public ISysMenuRepository SysMenuRepository { get; set; }
        public IWorkflowSvc WorkflowSvc { get; set; }

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyInfo()
        {
            return View();
        }

    }
}