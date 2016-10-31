using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper.Execution;
using JR.Core.Utilities;
using JR.Domain.Enums;
using JR.Domain.Repositories;
using JR.Web.Framework.Controllers;
using JR.Web.Framework.Extensions;
using JR.Domain.ViewModels;

namespace JR.WebSite.Controllers
{
    public class ArticleController : FrontController
    {
        public IArticleClassifyRepository ArticleClassifyRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }
        public readonly int PageSize = 30;

        public async Task<ActionResult> Index(long classifyId)
        {
            var classify = await ArticleClassifyRepository.GetEnableByIdAsync(classifyId);
            if (classify == null)
            {
                return RedirectTo404(Url.Home(),"找不到记录！");
            }
            ViewBag.Classify = classify;
            var viewCode = classify.PartialViewCode == ArticleListType.Image ? "ImageList" : "TextList"; 
            return View(viewCode, ArticleRepository.GetPagedList("", classifyId,1, PageSize));
        }

        public ActionResult TextList(long classifyId,int pageIndex = 2)
        { 
            return PartialView("_TextList", ArticleRepository.GetPagedList("", classifyId, pageIndex, PageSize));
        }

        public ActionResult ImageList(long classifyId, int pageIndex = 2)
        {
            return PartialView("_ImageList", ArticleRepository.GetPagedList("", classifyId, pageIndex, PageSize));
        }

        public async Task<ActionResult> Detail(long id)
        {
            if (id < 1)
            {
                return RedirectTo404(Url.Home(), "找不到记录！");
            }
            var model = await ArticleRepository.GetEnableByIdAsync(id);
            if (model == null)
            {
                return RedirectTo404(Url.Home(), "找不到记录！");
            }
            //var articleClassify = await ArticleClassifyRepository.GetUnDeleteByIdAsync(model.ClassifyId);
            
            //if (articleClassify != null)
            //{
            //    ViewBag.Title = articleClassify.Name;
            //}
            model.ViewCount += 1;
            await ArticleRepository.SaveChangesAsync();
            var viewCode = "SingleDetail";
            switch (model.PartialViewCode)
            {
                case ArticleDetailType.Single:
                    viewCode = "SingleDetail";
                    break;
                case ArticleDetailType.Simple:
                    viewCode = "SimpleDetail";
                    break;
                case ArticleDetailType.More:
                    viewCode = "MoreDetail";
                    break;
            }
            FrontMenuJsonModel currPage = ViewBag.CurrPage;
            if (currPage.id == 0)
            {
                var targetUrl = Url.Action("Index",new {classifyId=model.ClassifyId });
                SetCurrMenu(targetUrl);
            }
            return View(viewCode, model);
        }

        public ActionResult Search(string keyWord)
        {
            ViewBag.KeyWord = keyWord;
            var classify = SysConfigRepository.SeachClassifyId;
            if (string.IsNullOrEmpty(classify))
            {
                return View();
            } 
            var classifyArray = StringUtility.ConvertToBigIntArray(classify, ',').ToList();
            if (!classifyArray.Any())
            {
                return View();
            }
            var classifySort = classifyArray.Select((d, sort) => new { d, sort }).ToList();
            var classifyList = ArticleClassifyRepository.QueryEnable()
                                                        .Where(d => classifyArray.Any(m => m == d.Id))
                                                        .ToList()
                                                        .Select(d => new { Classify = d, Id = classifySort.FirstOrDefault(m => m.d == d.Id) }) 
                                                        .OrderBy(d => d.Id.sort)
                                                        .Select(d => d.Classify)
                                                        .ToList();
            ViewBag.ClassifyList = classifyList;
            if (!classifyList.Any())
            {
                return View();
            }
            ViewBag.ArticleList = ArticleRepository.GetListBySearch((keyWord ?? "").Trim(), classifyList.Select(d => d.Id).ToList(), 1, PageSize);

            return View();
        }

        public ActionResult SearchByPage(string keyWord,long classifyId, ArticleListType listType, int pageIndex = 2)
        {
            return PartialView(listType== ArticleListType.Image ? "_ImageList" : "_TextList", ArticleRepository.GetPagedList((keyWord??"").Trim(),classifyId, pageIndex, PageSize));
        }
    }
}