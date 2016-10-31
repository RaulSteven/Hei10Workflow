using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Utilities;
using Hei10.Domain.Infrastructure;

namespace Hei10.Domain.Models
{
    public class UserRole2Filter : AggregateRoot
    {
        public virtual long RoleId
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

        [DisplayName("数据源")]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string Source
        {
            get;
            set;
        }

        public virtual string FilterGroups
        {
            get;
            set;
        }
        [NotMapped]
        public List<PropertyModel> SourceProperties { get; set; }
    }
}
