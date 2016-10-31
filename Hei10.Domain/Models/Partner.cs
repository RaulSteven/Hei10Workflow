using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using System.ComponentModel;
using Hei10.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Hei10.Domain.Models
{
    /// <summary>
    /// 合作伙伴
    /// </summary>
    public class Partner : AggregateRoot
    {
        [DisplayName("名称")]
        [Required(ErrorMessage =ErrorMsgUtils.Required)]
        [MaxLength(50,ErrorMessage =ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("网址")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Url { get; set; }

        [DisplayName("Logo")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string LogoUrl { get; set; }

        public int Sort { get; set; }
    }
}
