using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Hei10.Domain.Infrastructure
{
    [Serializable]
    public class AggregateRoot : IAggregateRoot
    {
        [DisplayName("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }
        [DisplayName("通用状态")]
        public CommonStatus CommonStatus
        {
            get;
            set;
        }

        [DisplayName("创建时间")]
        public DateTime CreateTime
        {
            get;
            set;
        }
        [DisplayName("创建人")]
        [MaxLength(50)]
        public string CreateUserName { get; set; }

        [DisplayName("创建人Id")]
        public long CreateUserId
        {
            get;
            set;
        }


        [DisplayName("更新时间")]
        public DateTime UpdateTime
        {
            get;
            set;
        }

        [DisplayName("更新人Id")]
        public long UpdateUserId
        {
            get;
            set;
        }

        [MaxLength(50)]
        [DisplayName("创建IP")]
        public string CreateIP
        {
            get;
            set;
        }

        [MaxLength(50)]
        [DisplayName("更新IP")]
        public string UpdateIP
        {
            get;
            set;
        }
    }
}
