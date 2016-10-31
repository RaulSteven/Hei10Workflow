using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Hei10.Core.Extensions;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;
using Hei10.WebSite.Areas.Admin.Models;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class ArticleController : AdminController
    {
        public IArticleClassifyRepository ArticleClassifyRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }

        [ValidatePage]
        public ActionResult Index(string keyWord,long? classifyId,string classifyName, string orderField = "Article.ArticleDateTime", string orderDirection= "desc", int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.KeyWord = keyWord;
            ViewBag.ClassifyId = classifyId;
            ViewBag.ClassifyName = classifyName;
            return PartialView("Index",ArticleRepository.GetPagedList(ArticleClassifyRepository.AdminQueryUnDelete(),(keyWord??"").Trim(),classifyId, orderField, orderDirection, pageCurrent, pageSize));
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(string navtabId,long id = 0)
        {
            var model = new ArticleModel { ArticleDateTime = DateTime.Now ,Author = User.UserInfo.UserName, CommonStatus = CommonStatus.Enabled };
            if (id > 0)
            {
                var article = await ArticleRepository.GetByIdAsync(id);
                if (article == null)
                {
                    var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                Mapper.Map(article, model);
                var classify = await ArticleClassifyRepository.GetUnDeleteByIdAsync(article.ClassifyId);
                if (classify != null)
                {
                    model.ClassifyName = classify.Name;
                }
            }
            return PartialView(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(ArticleModel model)
        {
            var json = new JsonModel();
            if (await ArticleClassifyRepository.IsLastChildAsync(model.ClassifyId))
            {
                ModelState.AddModelError("ClassifyName", "只能添加到最后一级子分类");
            }
            // 数据有误
            if (!ModelState.IsValid)
            {
                json.GetError(ModelState);
                return Json(json);
            }
            var operationType = OperationType.Insert;
            var operationTitle = "添加文章信息";
            Article article = null;
            if (model.Id > 0)
            {
                article = await ArticleRepository.GetByIdAsync(model.Id);
                if (article == null)
                {
                    json.message = "记录不存在了";
                    return Json(json);
                }
                operationType = OperationType.Update;
                operationTitle = "修改文章信息";
            }
            else
            {
                article = new Article();
            }
            Mapper.Map(model, article);   
            await ArticleRepository.SaveAsync(article);
            await LogRepository.Insert(TableSource.Articles, operationType, operationTitle, article.Id);
            await AttachmentSvc.BoundAttachment(article.Id, article.Pic, article.ArticleContent);
            return Json(json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Delete, ActionName = "Index")]
        public async Task<ActionResult> BatchDelete(string ids)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(result);
            }
            var list = await ArticleRepository.BatchDeleteAsync(ids);
            if (list == null)
            {
                return Json(result);
            }
            var msg = string.Join(",", list.Select(m => m.Title).ToArray()).ToEllipsis(100);
            await LogRepository.Insert(TableSource.Articles, OperationType.Delete, string.Format("批量删除{0}等文章", msg), ids);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }
    }
}