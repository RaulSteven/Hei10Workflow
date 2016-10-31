using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.WebSite.Models;

namespace Hei10.WebSite.Controllers
{
    public class AdvertController : WebSiteController
    {
        public IAdPositionRepository AdPosRepository { get; set; }
        public IAdvertRepository AdvertRepository { get; set; }
        public ActionResult AdPosDisplay(AdPosKey? key, AdvertType? type, long? id, int takeSize = 0)
        {
            AdPosition adPosModel = null;
            if (key.HasValue)
            {
                adPosModel = AdPosRepository.GetByPosKeyCache(key.Value);
            }

            if (adPosModel == null || adPosModel.Id == 0)
            {
                return PartialView();
            }
            var list = AdvertRepository.GetListByAdPostIdCache(adPosModel.Id, type, takeSize);
            var model = new AdvertDisplayModel()
            {
                AdPosition = adPosModel,
                Adverts = list
            };
            return PartialView(adPosModel.ViewName, model);
        }
    }
}