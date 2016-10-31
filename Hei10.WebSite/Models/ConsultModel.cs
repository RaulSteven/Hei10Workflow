using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hei10.Core.Utilities;
using Hei10.Domain.Models;

namespace Hei10.WebSite.Models
{
    public class ConsultModel
    {

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(250, ErrorMessage = "{0}最多{1}个字符")]
        public string UserName { get; set; }

        [Display(Name = "电子邮件")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(500, ErrorMessage = "{0}最多{1}个字符")]
        [RegularExpression(RegexUtility.Email, ErrorMessage = "{0}不正确")]
        public string Email { get; set; }

        [Display(Name = "内容")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(500, ErrorMessage = "{0}最多{1}个字符")]
        public string Contents { get; set; } 

        public bool IsSucceed { get; set; }
    }
}