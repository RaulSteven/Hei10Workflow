using Hei10.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class SetRoleModel
    {
        public List<long> SelectedRoleIdList { get; set; }

        public List<UserRole> UserRoleList { get; set; }
        public long UserId { get; set; }

    }
}