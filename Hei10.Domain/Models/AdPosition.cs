using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class AdPosition : AggregateRoot
    {
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [MaxLength(50, ErrorMessage = "{0}最多{1}个字符")]
        public string Name { get; set; }

        [Display(Name = "位置")]
        [Required(ErrorMessage = "请填写“{0}”")]
        public AdPosKey Code { get; set; }

        [Display(Name = "显示模版")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [MaxLength(255, ErrorMessage = "{0}最多{1}个字符")]
        public string ViewName { get; set; }

        [Display(Name = "描述")]
        public string Descript { get; set; }

        [Display(Name = "图片")]
        [MaxLength(255, ErrorMessage = "{0}最多{1}个字符")]
        public string ImgUrl { get; set; }

        [Display(Name = "宽高")]
        [MaxLength(50, ErrorMessage = "{0}最多{1}个字符")]
        public string Size { get; set; }

        [Display(Name = "排序")]
        [Required(ErrorMessage = "请填写“{0}”")]
        public int Sort { get; set; }

        [Display(Name = "链接目标")]
        [MaxLength(255, ErrorMessage = "{0}最多{1}个字符")]
        public string LinkUrl { get; set; }
    }
}
