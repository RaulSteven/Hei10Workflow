using Hei10.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Models
{
    public partial class FrontMenu
    {
        [NotMapped]
        public string IconSmallPath
        {
            get
            {
                return StringUtility.XBase64Encode(IconSmall);
            }
        }
        [NotMapped]
        public string IconMiddlePath
        {
            get
            {
                return StringUtility.XBase64Encode(IconMiddle);
            }
        }
        [NotMapped]
        public string IconLargePath
        {
            get
            {
                return StringUtility.XBase64Encode(IconLarge);
            }
        }

        /// <summary>
        /// 在同辈中的位置，从0开始算
        /// </summary>
        [NotMapped]
        public int IndexOfParent { get; set; }
    }
}
