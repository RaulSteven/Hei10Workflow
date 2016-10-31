using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Models
{
    public partial class SysApartment
    {
        /// <summary>
        /// 在同辈中的位置，从0开始算
        /// </summary>
        [NotMapped]
        public int IndexOfParent { get; set; }
    }
}
