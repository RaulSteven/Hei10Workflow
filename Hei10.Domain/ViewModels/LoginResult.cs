using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;

namespace Hei10.Domain.ViewModels
{
    public class LoginResult
    {
        public SigninStatus Status { get; set; }
        public Users UserInfo { get; set; }

    }
}
