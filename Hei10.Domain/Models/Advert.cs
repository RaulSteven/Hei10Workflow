﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public partial class Advert : AggregateRoot
    {
        #region  properties

        [Display(Name = "名称")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [MaxLength(50, ErrorMessage = "{0}最多{1}个字符")]
        public string Name { get; set; }

        public long AdPosId { get; set; }

        [Display(Name = "开始时间")]
        [Required(ErrorMessage = "请填写“{0}”")]
        public DateTime StartTime { get; set; }

        [Display(Name = "结束时间")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "小组")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [RegularExpression(RegexUtility.NoNegativeInt, ErrorMessage = "“{0}”只能输入整数")]
        public short AdGroup { get; set; }

        [Display(Name = "位置")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [RegularExpression(RegexUtility.NoNegativeInt, ErrorMessage = "“{0}”只能输入整数")]
        public short Seat { get; set; }

        [Display(Name = "状态")]
        public AdvertStatus AdvertStatus { get; set; }

        [Display(Name = "类型")]
        public AdvertType AdType { get; set; }

        [Display(Name = "链接窗口")]
        [Required]
        public Target Target { get; set; }

        [Display(Name = "富文本")]
        public string MetaContent { get; set; }

        [Display(Name = "链接地址")]
        [MaxLength(255, ErrorMessage = "{0}最多{1}个字符")]
        [Required(ErrorMessage = "请填写“{0}”")]
        public string LinkUrl { get; set; }

        [Display(Name = "宽高")]
        [MaxLength(50, ErrorMessage = "{0}最多{1}个字符")]
        public string Size { get; set; }

        [Display(Name = "文本")]
        [MaxLength(255, ErrorMessage = "{0}最多{1}个字符")]
        public string TextContent { get; set; }

        [Display(Name = "图片")]
        [MaxLength(500, ErrorMessage = "{0}最多{1}个字符")]
        public string ImgUrl { get; set; }

        [NotMapped]
        public string AdPosName { get; set; }

        [Display(Name = "排序")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [RegularExpression(RegexUtility.NoNegativeInt, ErrorMessage = "{0}输入不正确")]
        public int SortIndex { get; set; }

        [Display(Name = "数据Id")]
        [Required(ErrorMessage = "请填写“{0}”")]
        [RegularExpression(RegexUtility.NoNegativeInt, ErrorMessage = "{0}输入不正确")]
        public int SourceId { get; set; }

        [NotMapped]
        [Display(Name = "位置")]
        public AdPosKey Code { get; set; }
        #endregion


    }
}
