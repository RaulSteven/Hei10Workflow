using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;

namespace Hei10.Domain.ViewModels
{
    public class FilterGroup
    {
        public List<FilterRule> ListRule { get; set; }
        public FilterGroupOp Op { get; set; }
        public List<FilterGroup> ListGroup { get; set; }

    }

    public class FilterRule
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public FilterRuleOp Op { get; set; }

    }
}
