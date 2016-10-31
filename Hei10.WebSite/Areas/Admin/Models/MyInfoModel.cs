using Hei10.Domain.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class MyInfoModel
    {
        public List<WfProcessInstance> MyApplyList { get; set; }

        public List<WfActivityInstance> MyTaskList { get; set; }
    }
}