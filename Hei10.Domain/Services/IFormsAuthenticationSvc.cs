using Hei10.Core.Security;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Hei10.Domain.Services
{
    public interface IFormsAuthenticationSvc
    {

        void CreateAuthenticationTicket(Users user, HttpResponseBase response, HttpContextBase httpContextBase, bool remember);
        void FromAuthenticationTicket(HttpCookie cookie);

        void LogOut(IJRUser user);

        string GetUserNameCookieKey();

    }
}
