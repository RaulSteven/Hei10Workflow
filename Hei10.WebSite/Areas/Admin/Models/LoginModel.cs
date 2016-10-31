using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class LoginModel
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "“{0}”不能为空")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        [Required(ErrorMessage = "“{0}”不能为空")]
        public string Password { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "“{0}”不能为空")]
        public string ValidateCode { get; set; }

        public bool IsRemember { get; set; }

    }
}