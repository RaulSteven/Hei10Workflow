using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hei10.Domain.Enums;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class ArticleModel
    {
        public long Id { get; set; }

        [Display(Name = "分类")]
        public long ClassifyId { get; set; }

        [Display(Name = "标题")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(255, ErrorMessage = "{0}最多{1}个字符")]
        public string Title { get; set; }

        [Display(Name = "副标题")]
        [StringLength(255, ErrorMessage = "{0}最多{1}个字符")]
        public string TitleSub { get; set; }

        [Display(Name = "作者")]
        [StringLength(50, ErrorMessage = "{0}最多{1}个字符")]
        public string Author { get; set; }

        [Display(Name = "文章来源")]
        [StringLength(200, ErrorMessage = "{0}最多{1}个字符")]
        public string Source { get; set; }

        [Display(Name = "文章内容")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string ArticleContent { get; set; }

        [Display(Name = "发布时间")]
        [Required(ErrorMessage = "{0}不能为空")]
        public DateTime ArticleDateTime { get; set; }

        [Display(Name = "封面图")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(255, ErrorMessage = "{0}最多{1}个字符")]
        public string Pic { get; set; }

        [Display(Name = "浏览次数")]
        public int ViewCount { get; set; }

        [Display(Name = "所属分类")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string ClassifyName { get; set; }

        public CommonStatus CommonStatus { get; set; }

        [Display(Name = "显示模板")]
        [Required(ErrorMessage = "{0}不能为空")]
        public ArticleDetailType PartialViewCode { get; set; }
    }
}