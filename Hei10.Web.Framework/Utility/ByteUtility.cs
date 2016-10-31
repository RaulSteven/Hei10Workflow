using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Web.Framework.Utility
{
    public class ByteUtility
    {
        private static string Compute(long dividend, long divisor, string unit)
        {
            return Math.Round((dividend/(double) divisor),1) + unit;
        }

        public static string ToConversion(long value)
        {
            if (value < 1)
            {
                return "0KB";
            }
            const int kb = 1024;
            const int mb = 1024 * 1024;
            const int gb = 1024 * 1024 * 1024; 
            if (value >= gb)
            {
                return Compute(value, gb, "G");
            }
            if (value >= mb)
            {
                return Compute(value, mb, "M");
            }
            return Compute(value, kb, "KB");
        }
    }
}
