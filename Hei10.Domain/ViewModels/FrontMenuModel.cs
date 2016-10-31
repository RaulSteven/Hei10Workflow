using Hei10.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.ViewModels
{
   public class FrontMenuModel
    {
        public long Id { get; set; }
        public virtual long Pid
        {
            get;
            set;
        }

        [DisplayName("名称")]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength), Required(ErrorMessage = ErrorMsgUtils.Required)]
        public virtual string Name
        {
            get;
            set;
        }

        [DisplayName("备注")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string Remark
        {
            get;
            set;
        }

        [DisplayName("地址")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string Url
        {
            get;
            set;
        }

        [DisplayName("小图标")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string IconSmall
        {
            get;
            set;
        }
        [DisplayName("中图标")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string IconMiddle
        {
            get;
            set;
        }
        [DisplayName("大图标")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string IconLarge
        {
            get;
            set;
        }

        public virtual int Sort
        {
            get;
            set;
        }
    }
}
