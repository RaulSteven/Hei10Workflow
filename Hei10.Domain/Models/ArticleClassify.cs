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
    public class ArticleClassify : AggregateRoot
    { 
        public int PId { get; set; }

        [Display(Name = "名称")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "备注")]
        [StringLength(200)]
        public string Remark { get; set; }

        [Display(Name = "排序")]
        public int SortIndex { get; set; }

        [Display(Name = "子数量")]
        public int ChildrenCount { get; set; }

        [Display(Name = "深度")]
        public int Depth { get; set; }

        [Required]
        [StringLength(255)]
        public string TreePath { get; set; }

        public ArticleListType PartialViewCode { get; set; }

        [NotMapped]
        public int OldPId { get; set; }

        [NotMapped]
        public string ViewCode { get { return PartialViewCode + ""; } }

    }
}
