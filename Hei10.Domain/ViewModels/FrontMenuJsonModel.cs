using Hei10.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.ViewModels
{
    public class FrontMenuJsonModel
    {
        public long id { get; set; }
        public long pid { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public int sort { get; set; }
        public string url { get; set; }

        public string iconSmall { get; set; }
        public string iconSmallPath
        {
            get
            {
                return StringUtility.XBase64Encode(iconSmall);
            }
        }
        public string iconMiddle { get; set; }
        public string iconMiddlePath
        {
            get
            {
                return StringUtility.XBase64Encode(iconMiddle);
            }
        }
        public string iconLarge { get; set; }
        public string iconLargePath
        {
            get
            {
                return StringUtility.XBase64Encode(iconLarge);
            }
        }

        public List<FrontMenuJsonModel> children { get; set; }

    }
}
