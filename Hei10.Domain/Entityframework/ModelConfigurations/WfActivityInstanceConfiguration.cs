using Hei10.Domain.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Entityframework.ModelConfigurations
{
    public class WfActivityInstanceConfiguration : EntityTypeConfiguration<WfActivityInstance>
    {
        public WfActivityInstanceConfiguration()
        {
            Property(m => m.RowVersion)
                .IsRowVersion();
        }
    }
}
