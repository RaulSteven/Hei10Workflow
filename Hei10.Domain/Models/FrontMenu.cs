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
    public partial class FrontMenu : AggregateRoot
    {
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

        [MaxLength(100)]
        public string TreePath { get; set; }
    }
}
