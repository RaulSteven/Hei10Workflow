using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.WebSite.Areas.Admin.Models;
using Hei10.Domain.Enums;
using Hei10.Domain.ViewModels;
using System.Threading.Tasks;
using AutoMapper;
using Hei10.Domain.Models.Workflow;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class LeaveInfoController : AdminController
    {
        public IWorkflowSvc WorkflowSvc { get; set; }
        public ILeaveInfoRepository LeaveInfoRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        // GET: Admin/LeaveInfo
        public ActionResult Index()
        {
            return View();
        }

    }
}