using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Models.Workflow
{
    /// <summary>
    /// 流程线
    /// </summary>
    public class WfTransition
    {
        public string key { get; set; }

        public string from { get; set; }

        public string to { get; set; }

        public string text { get; set; }
        public string remark { get; set; }

        /// <summary>
        /// 数据筛选规则，SQL语句
        /// </summary>
        public string condition { get; set; }
    }
}
