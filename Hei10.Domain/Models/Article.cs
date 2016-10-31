using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class Article : AggregateRoot
    {

        [Display(Name = "分类")] 
        public long ClassifyId { get; set; }

        [Display(Name = "标题")]
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Display(Name = "副标题")]
        [StringLength(255)]
        public string TitleSub { get; set; }

        [Display(Name = "作者")]
        [StringLength(50)]
        public string Author { get; set; }

        [Display(Name = "文章简介")]
        [StringLength(500)]
        public string Brief { get; set; }

        [Display(Name = "文章内容")]
        [Required]
        public string ArticleContent { get; set; }

        [Display(Name = "发布时间")]
        [Required]
        public DateTime ArticleDateTime { get; set; } 

        [Display(Name = "标签")]
        [StringLength(255)]
        public string Tags { get; set; }

        [Display(Name = "文章来源")]
        [StringLength(200)]
        public string Source { get; set; }

        [Display(Name = "来源网址")]
        [StringLength(255)] 
        public string SourceLink { get; set; }
         
        public int SortIndex { get; set; }  

        [Display(Name = "封面图")]
        [StringLength(255)]
        public string Pic { get; set; } 

        [Display(Name = "浏览次数")] 
        public int ViewCount { get; set; }

        public ArticleDetailType PartialViewCode { get; set; }


    }
}
