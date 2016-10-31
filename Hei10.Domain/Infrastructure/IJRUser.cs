using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Security;
using Hei10.Domain.ViewModels;

namespace Hei10.Domain.Infrastructure
{
    public interface IJRUser:ISysUser
    {
        SysUserModel UserInfo { get; set; }
    }
}
