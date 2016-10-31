using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hei10.Core.Utilities;

namespace Hei10.Domain.Models
{
    public class LeaveInfo:AggregateRoot
    {
        public int Days { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [DisplayName("请假类型")]
        [Required(ErrorMessage =ErrorMsgUtils.Required)]
        [MaxLength(10,ErrorMessage =ErrorMsgUtils.MaxStringLength)]
        public string LeaveType { get; set; }

        [DisplayName("请假事由")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Remark { get; set; }
    }
}
