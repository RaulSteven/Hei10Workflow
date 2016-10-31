using Hei10.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Models
{
    public class AdvertDisplayModel
    {
        public AdPosition AdPosition { get; set; }
        public List<Advert> Adverts { get; set; }
    }
}