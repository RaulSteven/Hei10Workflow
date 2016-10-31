using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.ViewModels
{
    public class FilterGroupModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Source { get; set; }
        public string FilterGroups { get; set; }
        //public FilterGroup FilterGroup { get; set; }

        public string SourceProperties { get; set; }

    }
}
