using Hei10.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class PartnerModel
    {
        public long Id { get; set; }

        [DisplayName("名称")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(50, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("网址")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        [RegularExpression(RegexUtility.Url,ErrorMessage = "请输入以http或https开头的网址")]
        public string Url { get; set; }

        [DisplayName("Logo")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string LogoUrl { get; set; }

        public int Sort { get; set; }
    }
}