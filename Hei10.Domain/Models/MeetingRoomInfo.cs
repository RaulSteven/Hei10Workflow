using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Utilities;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class MeetingRoomInfo : AggregateRoot
    {
        [DisplayName("名称")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("使用时间")]
        public DateTime UsedTime { get; set; }

        [DisplayName("申请时间")]
        public DateTime ApplyTime { get; set; }

        [DisplayName("申请人")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string ApplyName { get; set; }

        [DisplayName("使用说明")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Remark { get; set; }
    }
}
