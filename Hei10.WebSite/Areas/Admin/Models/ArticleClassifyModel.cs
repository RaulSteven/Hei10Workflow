using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hei10.Domain.Enums;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class ArticleClassifyModel
    {
        public long Id { get; set; }
        public int PId { get; set; }

        [Display(Name = "名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(50, ErrorMessage = "{0}最多{1}个字符")]
        public string Name { get; set; }

        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "{0}最多{1}个字符")]
        public string Remark { get; set; }

        public string TreePath { get; set; }

        [Display(Name = "显示模板")]
        [Required(ErrorMessage = "{0}不能为空")]
        public ArticleListType PartialViewCode { get; set; }
         
    }
}