using Hei10.Domain.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.ViewModels
{
    public class UserMenuModel
    {
        public long Id { get; set; }

        public virtual long Pid
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Url
        {
            get;
            set;
        }

        public virtual string Icon
        {
            get;
            set;
        }

        public virtual int Sort
        {
            get;
            set;
        }
        public virtual string Source
        {
            get;
            set;
        }

        public SysButton Buttons { get; set; }

        //public string FilterGroups { get; set; }

        public FilterGroup FilterGroup
        {
            get;set;
        }

        public bool HasFilter()
        {
            return null != FilterGroup
                && ((FilterGroup.ListRule != null && FilterGroup.ListRule.Count > 0)
                   || (FilterGroup.ListGroup != null && FilterGroup.ListGroup.Count > 0));
        }

        public string GetTabUrl()
        {
            if (string.IsNullOrEmpty(Url))
            {
                return Url;
            }
            return Url + (Url.Contains("?") ? "&" : "?") + "tabId=" + GetTabId();
        }

        public string GetTabId()
        {
            return "page_" + Id;
        }
    }
}
