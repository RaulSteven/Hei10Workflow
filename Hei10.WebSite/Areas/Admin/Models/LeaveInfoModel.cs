using Hei10.Core.Utilities;
using Hei10.Domain.Models.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class LeaveInfoModel:WfApplyBase
    {
        #region 请假信息
        public int Days { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [DisplayName("请假类型")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(10, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string LeaveType { get; set; }

        [DisplayName("备注")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Remark { get; set; }
        #endregion

        
    }
}