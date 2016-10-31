using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class SealInfo : AggregateRoot
    {
        [Display(Name = "盖章的名称")]
        [StringLength(250)]
        public string Name { get; set; }

        [Display(Name = "申请时间")]
        public DateTime ApplyTime { get; set; }

        [Display(Name = "申请人")]
        [StringLength(250)]
        public string ApplyUserName { get; set; }

        [Display(Name="盖章文件说明")]
        [StringLength(1000)]
        public string FileExplain { get; set; }

    }
}
