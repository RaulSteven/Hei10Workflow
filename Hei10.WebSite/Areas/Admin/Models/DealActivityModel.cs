using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Workflow;
namespace Hei10.WebSite.Areas.Admin.Models
{
    public class DealActivityViewModel
    {

        public WfActivityButton Buttons { get; set; }
        public AggregateRoot SourceData { get; set; }

        public TableSource TableSource { get; set; }

        public WfActivityInstance ActivityInstance { get; set; }

        public WfProcessInstance ProcessInstance { get; set; }

        public List<WfActivityInstance> DealActivityList { get; set; }
    }

    public class DealActivityModel
    {
        public long ProcessInstanceId { get; set; }
        public long ActivityInstanceId { get; set; }

        public string Comment { get; set; }
        public WfActivityButton ClickButton { get; set; }

        public string NextStepId { get; set; }
        public string AssignToUserIds { get; set; }
        public string AssignToUserNames { get; set; }

        public string BackStepId { get; set; }
    }
}