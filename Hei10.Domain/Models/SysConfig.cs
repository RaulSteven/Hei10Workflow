using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class SysConfig : AggregateRoot
    {

        [Required]
        [StringLength(50)]
        public string ConKey { get; set; }

        [Required]
        public string ConValue { get; set; }
    }
}
