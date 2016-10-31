using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class AdminModel
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [StringLength(30, ErrorMessage = "{0}长度为2到30个字", MinimumLength = 2)]
        [RegularExpression(RegexUtility.LoginName, ErrorMessage = "{0}输入不正确")] 
        public string LoginName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(16, ErrorMessage = "{0}的长度为{2}-{1}个字符", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "密码确认")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "真实姓名")]
        [StringLength(50, ErrorMessage = "{0}最多{1}个字符")]
        public string RealName { get; set; }

        [Display(Name = "备注")]
        [MaxLength(250, ErrorMessage = "{0}最多{1}个字符")]
        public string Remark { get; set; }

        [Display(Name = "头像")]
        [StringLength(200, ErrorMessage = "{0}最多{1}个字符")]
        public string HeadImg { get; set; }

        [Display(Name = "性别")]
        public Gender Gender { get; set; }

        [Display(Name = "状态")]
        public CommonStatus CommonStatus { get; set; }


    }
}