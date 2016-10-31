using Hei10.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class SysOperationLog:AggregateRoot
    {
        [Required]
        public TableSource LogCat { get; set; }

        [Required]
        public OperationType LogType { get; set; }

        [Required]
        [StringLength(200)]
        public string LogTitle { get; set; }

        public string LogDesc { get; set; }

        [StringLength(20)]
        public string DataSource { get; set; }

        [StringLength(50)]
        public string DataSouceId { get; set; }
    }
}
