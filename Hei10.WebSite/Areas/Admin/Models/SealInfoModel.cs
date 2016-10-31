using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hei10.Core.Utilities;
using Hei10.Domain.Models.Workflow;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class SealInfoModel : WfApplyBase
    {

        [Display(Name = "盖章的名称")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [StringLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [Display(Name = "申请时间")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        public DateTime ApplyTime { get; set; }

        [Display(Name = "申请人")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [StringLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string ApplyUserName { get; set; }

        [Display(Name = "盖章文件说明")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [StringLength(1000, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string FileExplain { get; set; }
    }
}