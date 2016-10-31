using System.ComponentModel.DataAnnotations;

namespace Hei10.WebSite.Areas.Admin.Models
{
   public class ModifyPwdModel
    {

       [Display(Name = "原密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string OldPwssword { get; set; }

       [Display(Name = "新密码")]
       [Required(ErrorMessage = "{0}不能为空")]
       [StringLength(16, ErrorMessage = "{0}的长度为{2}-{1}个字符", MinimumLength = 6)]
       public string Password { get; set; }

       [Display(Name = "密码确认")]
       [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
       public string PasswordConfirm { get; set; }

    }
}
