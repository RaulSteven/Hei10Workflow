using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Enums;

namespace Hei10.Domain.ViewModels
{
    public class SysUser : IJRUser
    {

        #region properties
        public SysUserModel UserInfo { get; set; }

        public IIdentity Identity
        {
            get;
            private set;
        }


        #endregion

        public SysUser(SysUserModel serializeModel)
        {
            UserInfo = serializeModel;
            Identity = new GenericIdentity(serializeModel.UserName);
        }

        public SysUser()
        {
            Identity = new GenericIdentity("");
        }

       
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool HasButton(SysButton btn)
        {
            if (UserInfo == null || UserInfo.CurrentMenu == null)
            {
                return false;
            }

            return (UserInfo.CurrentMenu.Buttons & btn) != SysButton.None;
        }
    }
}
