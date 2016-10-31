using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Repositories;
using System.Threading.Tasks;
using AutoMapper;
using Hei10.Domain.Entityframework;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.WebSite.Models;

namespace Hei10.WebSite.Controllers
{
    public class HomeController : FrontController
    {
        public ISysConfigRepository SysConfigRepository { get; set; }
        public IArticleClassifyRepository ClassifyRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public IRecruitJobRepository RecruitJobRepository { get; set; }
        public IPartnerRepository PartnerRepository { get; set; }
        public IConsultRepository ConsultRepository { get; set; }

        public async Task<ActionResult> Index()
        { 
            ViewBag.Staff = (SysConfigRepository.StaffPic == null ? "" : string.Join("|", SysConfigRepository.StaffPic));
            ViewBag.Case = await ArticleRepository.GetListByTreePathAsync(ClassifyRepository.QueryUnDelete(), SysConfigRepository.CaseClassifyId.ToString(ArticleClassifyRepository.TreePathFormat));
            ViewBag.Job = await RecruitJobRepository.GetListAsync();
            return View();
        }

        public async Task<ActionResult> CaseDetail(long id)
        {
            if (id < 1)
            {
                return RedirectTo404(Url.Action("Index"), "找不到记录！");
            }
            var article = await ArticleRepository.GetEnableByIdAsync(id);
            if (article == null)
            {
                return RedirectTo404(Url.Action("Index"), "找不到记录！");
            }
            ViewBag.Random = await ArticleRepository.GetListByRandomAsyn(ClassifyRepository.QueryUnDelete(), SysConfigRepository.CaseClassifyId.ToString(ArticleClassifyRepository.TreePathFormat),4,article.Id);
            return View(article);
        }

        public ActionResult About()
        {
            return View();
        }
         
        public ActionResult NotFind(string reUrl, string msg)
        {
            ViewBag.ReUrl = reUrl;
            ViewBag.Msg = msg;
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public async Task<ActionResult> PartnerList()
        {
            var list = await PartnerRepository.GetListAsync();
            return PartialView("_Partner", list);
        }

        public ActionResult Consult()
        {
            return PartialView("_Consult", new ConsultModel {});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Consult(ConsultModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Consult", model);
            }
            var consult = new Consult();
            Mapper.Map(model, consult);
            consult.CommonStatus = CommonStatus.Enabled;
            await ConsultRepository.SaveAsync(consult);
            model.IsSucceed = true;
            return PartialView("_Consult", model);
        }
    }
}