using Hei10.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.ViewModels
{
    public class SysUserRole2MenuModel
    {
        public long Id { get; set; }

        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public SysButton Buttons { get; set; }

        public SysButton SelectedButtons { get; set; }

        public string MenuTreePath { get; set; }
    }
}
