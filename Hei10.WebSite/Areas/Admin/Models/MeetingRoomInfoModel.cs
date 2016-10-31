using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hei10.Core.Utilities;
using Hei10.Domain.Models.Workflow;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class MeetingRoomInfoModel : WfApplyBase
    {

        [DisplayName("名称")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("使用时间")]
        public DateTime UsedTime { get; set; }

        [DisplayName("申请时间")]
        public DateTime ApplyTime { get; set; }

        [DisplayName("申请人")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string ApplyName { get; set; }

        [DisplayName("使用说明")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Remark { get; set; }
    }
}