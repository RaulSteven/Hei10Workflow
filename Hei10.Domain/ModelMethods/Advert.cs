using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;

namespace Hei10.Domain.Models
{
   public partial class Advert
    {
        #region methods

        public bool IsNormal()
        {
            var isNormal = CommonStatus == CommonStatus.Enabled
                           && StartTime <= DateTime.Now
                           && (EndTime == null || EndTime >= DateTime.Now);
            return isNormal;
        }

        public string GetTarget()
        {
            return "_" + Target.ToString().ToLower();
        }

        #endregion
    }
}
