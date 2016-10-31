using Hei10.Core.Extensions;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.ViewModels
{
    public class SysMenuModel
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

        [DisplayName("Icon")]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string Icon
        {
            get;
            set;
        }

        public virtual int Sort
        {
            get;
            set;
        }

        [DisplayName("数据源")]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string Source
        {
            get;
            set;
        }

        public SysButton Buttons { get; set; }

        
    }
}
