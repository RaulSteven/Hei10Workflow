using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class ConsultClassifyModel
    {
        public long Id { get; set; }

        [Display(Name = "标题")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(100, ErrorMessage = "{0}最多{1}个字符")]
        public string Title { get; set; }

        [Display(Name = "内容")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(1000, ErrorMessage = "{0}最多{1}个字符")]
        public string Contents { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }
    }
}