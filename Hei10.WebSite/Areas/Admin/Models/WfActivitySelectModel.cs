using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hei10.Domain.Models.Workflow;
using Hei10.Domain.Models;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class WfActivitySelectModel
    {
        public WfActivity Activity { get; set; }
        public List<Users> UserList { get; set; }
    }
}