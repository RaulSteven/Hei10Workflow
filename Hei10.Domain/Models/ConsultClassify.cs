using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class ConsultClassify : AggregateRoot
    {
        [Display(Name = "标题")]
        [StringLength(500)]
        public string Title { get; set; }

        [Display(Name = "内容")]
        [StringLength(1000)]
        public string Contents { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }
    }
}
