using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JR.Domain.Repositories;
using JR.Web.Framework.Controllers;
using JR.Web.Framework.Extensions;

namespace JR.WebSite.Controllers
{
    public class JoinController : FrontController
    {
        public IRecruitJobRepository RecruitJobRepository { get; set; }
        public async Task<ActionResult> Index()
        {
            var list = await RecruitJobRepository.GetListAsync();
            return View(list);
        }
        public async Task<ActionResult> Detail(long id)
        {
            if (id < 1)
            {
                return RedirectTo404(Url.Home(), "找不到记录！");
            }
            var model = await RecruitJobRepository.GetEnableByIdAsync(id);
            if (model == null)
            {
                return RedirectTo404(Url.Home(), "找不到记录！");
            }

            var url = Url.Action("Index");
            SetCurrMenu(url);
            return View(model);
        }
    }
}