using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.ViewModels;

namespace Hei10.Domain.Infrastructure
{
    public class JRUserEmpty : IJRUser
    {
        public IIdentity Identity
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public SysUserModel UserInfo
        {
            get
            {
                return new SysUserModel();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
