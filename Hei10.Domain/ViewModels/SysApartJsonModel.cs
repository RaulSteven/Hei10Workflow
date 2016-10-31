using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.ViewModels
{
    public class SysApartJsonModel
    {
        public long id { get; set; }
        public long pid { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public int sort { get; set; }
        public string @checked { get; set; }

        public List<SysApartJsonModel> children { get; set; }
    }

    public class SysApartZTreeModel
    {
        public long id { get; set; }
        public long pid { get; set; }
        public string name { get; set; }
        public string treePath { get; set; }
        public int sort { get; set; }

        public string userTip { get; set; }

        public bool open { get; set; }

        public List<SysApartZTreeModel> children { get; set; }
    }

    public class SysApartWorkflowModel
    {
        public long id { get; set; }
        public long parentID { get; set; }
        public string title { get; set; }

        public string treePath { get; set; }

        public string ico { get; set; }
        public int type { get; set; }
        

        public string link { get; set; }

        public int hasChilds { get; set; }

        public List<SysApartWorkflowModel> childs { get; set; }
    }
}
