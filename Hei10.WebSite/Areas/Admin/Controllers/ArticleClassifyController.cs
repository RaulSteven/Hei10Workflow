using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;
using Hei10.WebSite.Areas.Admin.Models;
using Newtonsoft.Json;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class ArticleClassifyController : AdminController
    {
        public IArticleClassifyRepository ArticleClassifyRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        [ValidatePage]
        public async Task<ActionResult> Index()
        {
            var list = await ArticleClassifyRepository.GetListByZTreeAsync();
            return PartialView("Index",JsonConvert.SerializeObject(list));
        }

        public async Task<ActionResult> GetZTreeJson()
        {
            var list = await ArticleClassifyRepository.GetListByZTreeAsync();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(ArticleClassifyModel model)
        {
            var result = new JsonModel(); 
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            ArticleClassify p = null;
            if (model.PId != 0)
            {
                p = await ArticleClassifyRepository.GetUnDeleteByIdAsync(model.PId);
                if (p == null)
                {
                    result.message = "找不到id为" + model.PId + "的主分类！";
                    return Json(result); 
                }
            }

            var operationType = OperationType.Insert;
            var operationTitle = "添加管文章分类";
            ArticleClassify articleClassify = null;
            if (model.Id > 0)
            {
                articleClassify = await ArticleClassifyRepository.GetByIdAsync(model.Id);
                if (articleClassify == null)
                {
                    result.message = "记录不存在了";
                    return Json(result);
                }
                operationType = OperationType.Update;
                operationTitle = "修改管文章分类";
            }
            else
            {
                articleClassify = new ArticleClassify();
            }
            Mapper.Map(model, articleClassify); 
            articleClassify.CommonStatus = CommonStatus.Enabled;
            await ArticleClassifyRepository.SaveAsync(articleClassify,p);
            await LogRepository.Insert(TableSource.ArticleClassifies, operationType, operationTitle, articleClassify.Id);
            result.message = "保存成功！";
            result.Data = articleClassify;
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateButton(Buttons = SysButton.Delete, ActionName = "Index")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！",closeCurrent = false };
            if (id < 1)
            { 
                return Json(result);
            }
            var model = await ArticleClassifyRepository.GetUnDeleteByIdAsync(id);
            if (model == null)
            {
                return Json(result);
            }
            if (await ArticleClassifyRepository.IsLastChildAsync(model.Id))
            {
                result.message = "请先删除其子分类";
                return Json(result);
            }
            if (await ArticleRepository.IsAnyRecordAsync(model.Id))
            {
                result.message = "请先删除文章记录";
                return Json(result);
            }
            model.CommonStatus = CommonStatus.Deleted;
            await ArticleClassifyRepository.SaveAsync(model);
            await LogRepository.Insert(TableSource.ArticleClassifies, OperationType.Delete, "删除" + model.Name + "及其子分类", id);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

    }
}