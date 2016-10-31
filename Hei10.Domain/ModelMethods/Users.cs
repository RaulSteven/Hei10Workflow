using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Models
{
    public partial class Users
    {
        public const string GIdPrefix = "UsersGId-";
        [NotMapped]
        public string GId
        {
            get
            {
                return GIdPrefix + Id;
            }
        }
    }
}
