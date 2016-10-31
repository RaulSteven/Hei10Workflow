using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class RecruitJob : AggregateRoot
    {
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [MaxLength(50, ErrorMessage = "{0}最多{1}个字符")]
        public string Name { get; set; }

        [Display(Name = "工作职责")]
        public string WorkDuty { get; set; }

        [Display(Name = "招聘要求")]
        public string Requirement { get; set; }

        [Display(Name = "排序")]
        [Required(ErrorMessage = "请填写“{0}”")]
        public int Sort
        {
            get;
            set;
        }

        [Display(Name = "详情")]
        public string Contents { get; set; }
    }
}
