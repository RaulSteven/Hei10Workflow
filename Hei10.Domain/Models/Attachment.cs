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
    public partial class Attachment : AggregateRoot
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public long FileSize { get; set; }

        [Required]
        [StringLength(20)]
        public string FileExt { get; set; }

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }

        public int SortIndex { get; set; }

        [StringLength(255)]
        public string Descript { get; set; }

        public TableSource Source { get; set; }

        [Display(Name = "数据源Id")]
        public long SourceId { get; set; }


        public int ViewCount { get; set; }
    }
}
