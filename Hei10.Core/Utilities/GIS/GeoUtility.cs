using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace Hei10.Core.Utilities.GIS
{
    public class GeoUtility
    {
        public static SqlGeography GetSqlGeo(decimal? lat, decimal? lng)
        {
            return SqlGeography.Point((double)(lat ?? 0), (double)(lng ?? 0), DbGeography.DefaultCoordinateSystemId);
        }

        public static SqlGeography GetSqlGeo(decimal lat, decimal lng)
        {
            return GetSqlGeo((decimal?)lat, lng);
        }
    }
}
