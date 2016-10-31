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
    public class Consult : AggregateRoot
    {
        public long ClassifyId { get; set; }

        [Display(Name = "标题")] 
        [StringLength(500)]
        public string Title { get; set; }

        [Display(Name = "内容")]
        [StringLength(5000)]
        public string Contents { get; set; }

        [Display(Name = "姓名")]
        [StringLength(250)]
        public string UserName { get; set; }

        [Display(Name = "联系电话")]
        [StringLength(250)]
        public string Phone { get; set; }

        [Display(Name = "电子邮件")]
        [StringLength(500)]
        public string Email { get; set; }
         
    }
}
