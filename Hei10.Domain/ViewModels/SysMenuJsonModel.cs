using Hei10.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Extensions;

namespace Hei10.Domain.ViewModels
{
    public class SysMenuJsonModel
    {
        public long id { get; set; }
        public long pid { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public int sort { get; set; }
        public string url { get; set; }
        public string faicon { get; set; }
        public string source { get; set; }

        public string @checked { get; set; }
        public SysButton buttons { get; set; }

        private string _buttonNames = string.Empty;
        public string buttonNames
        {
            get
            {
                if (string.IsNullOrEmpty(_buttonNames))
                {
                    var sb = new StringBuilder();
                    foreach (var btn in buttons.GetDescriptDict())
                    {
                        if (buttons.HasFlag((SysButton)btn.Key))
                        {
                            sb.Append(btn.Value).Append(',');
                        }
                    }
                    _buttonNames = sb.Remove(sb.Length -1,1).ToString();
                }
                return _buttonNames;
            }
        }


        public List<SysMenuJsonModel> children { get; set; }
    }
}
