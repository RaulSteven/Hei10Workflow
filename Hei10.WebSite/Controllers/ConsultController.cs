using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using JR.Domain.Enums;
using JR.Domain.Models;
using JR.Domain.Repositories;
using JR.Web.Framework.Controllers;
using JR.Web.Framework.Extensions;
using JR.WebSite.Models;

namespace JR.WebSite.Controllers
{
    public class ConsultController : FrontController
    {
        public IConsultRepository ConsultRepository { get; set; }
        public IConsultClassifyRepository ConsultClassifyRepository { get; set; }

        public async Task<ActionResult> Index(long id)
        {
            if (id < 1)
            {
                return RedirectTo404(Url.Home(), "找不到记录！");
            }
            var consultClassify = await ConsultClassifyRepository.GetEnableByIdAsync(id);
            if (consultClassify == null)
            {
                return RedirectTo404(Url.Home(), "找不到记录！");
            }
            return View(new ConsultModel { ClassifyId = id,ConsultClassifyTitle = consultClassify.Title , ConsultClassifyContents = consultClassify.Contents});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ConsultModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var consult = new Consult();
            Mapper.Map(model, consult);
            consult.CommonStatus = CommonStatus.Enabled;
            await ConsultRepository.SaveAsync(consult);
            model.IsSucceed = true;
            return View(model);
        }
    }
}