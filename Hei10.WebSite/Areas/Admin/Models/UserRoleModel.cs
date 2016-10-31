using Hei10.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class UserRoleModel
    {
        public long Id { get; set; }

        public virtual int Sort
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

        [DisplayName("名称")]
        [MaxLength(10, ErrorMessage = ErrorMsgUtils.MaxStringLength), Required(ErrorMessage = ErrorMsgUtils.Required)]
        public virtual string Name
        {
            get;
            set;
        }
    }
}