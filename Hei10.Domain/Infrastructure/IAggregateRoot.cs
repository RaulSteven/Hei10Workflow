using Hei10.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Infrastructure
{
    public interface IAggregateRoot : IEntity
    {
        long CreateUserId { get; set; }
        DateTime CreateTime { get; set; }
        string CreateUserName { get; set; }

        string CreateIP { get; set; }
        long UpdateUserId { get; set; }

        DateTime UpdateTime { get; set; }
        string UpdateIP { get; set; }
        CommonStatus CommonStatus { get; set; }
    }
}
