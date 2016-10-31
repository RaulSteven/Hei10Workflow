using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;

namespace Hei10.Domain.ViewModels
{
    public class ArticleClassifyBizModel
    {
        public long id { get; set; }
        public long pid { get; set; }
        public string name { get; set; }
        public string treePath { get; set; }
        public string remark { get; set; }
        public string partialViewCode { get; set; }
        public List<ArticleClassifyBizModel> children { get; set; }
    }
}
