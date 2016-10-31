using Hei10.Core.Extensions;
using Hei10.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Models
{
    public partial class SysMenu
    {
        [NotMapped]
        public List<SysMenu> Children { get; set; }

        [NotMapped]
        private string _ButtonNames = string.Empty;
        [NotMapped]
        public string ButtonNames
        {
            get
            {
                if (string.IsNullOrEmpty(_ButtonNames))
                {
                    var sb = new StringBuilder();
                    foreach (var btn in Buttons.GetDescriptDict())
                    {
                        if (Buttons.HasFlag((SysButton)btn.Key))
                        {
                            sb.Append(btn.Value).Append(',');
                        }
                    }
                    _ButtonNames = sb.Remove(sb.Length - 1, 1).ToString();
                }
                return _ButtonNames;
            }
        }

        /// <summary>
        /// 在同辈中的位置，从0开始算
        /// </summary>
        [NotMapped]
        public int IndexOfParent { get; set; }
    }
}
