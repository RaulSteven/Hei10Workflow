using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hei10.Core.Utilities;
using System.ComponentModel;

namespace Hei10.Domain.ViewModels
{
    public class SysApartModel
    {
        public long Id { get; set; }

        public virtual long Pid
        {
            get;
            set;
        }

        public virtual int Sort
        {
            get;
            set;
        }

        [DisplayName("名称")]
        [MaxLength(50,ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        [Required(ErrorMessage =ErrorMsgUtils.Required)]
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
        
    }
}