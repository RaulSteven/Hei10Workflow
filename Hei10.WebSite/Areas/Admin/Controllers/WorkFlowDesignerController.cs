using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Hei10.Core.Extensions;
using Hei10.Domain.Enums;
using Hei10.Domain.Models.Workflow;
using Hei10.Domain.Repositories;
using Hei10.Domain.ViewModels;
using Hei10.Web.Framework.Controllers;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class WorkFlowDesignerController : AdminController
    {
        public IUserRoleRepository UserRoleRepository { get; set; }

        public IWfProcessRepository WfProcessRepository { get; set; }

        public ISysOperationLogRepository LogRepository { get; set; }

        public IWfProcessInstanceRepository WfProcessInstanceRepository { get; set; }

        public IWfActivityInstanceRepository WfActivityInstanceRepository { get; set; }

        public ActionResult Index(long flowId=0)
        {
            ViewBag.FlowId = flowId;
            return View();
        }

        public ActionResult FlowList(string keyWord, string orderField = "UpdateTime", string orderDirection = "desc", int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.KeyWord = keyWord;
            var list = WfProcessRepository.GetPagedList(keyWord, orderField, orderDirection, pageCurrent, pageSize);
            return PartialView("FlowList", list);
        }

        public ActionResult Manager(string keyWord, string orderField = "UpdateTime", string orderDirection = "desc", int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.KeyWord = keyWord;
            var list = WfProcessRepository.GetPagedList(keyWord, orderField, orderDirection, pageCurrent, pageSize);
            return PartialView("Manager", list);
        }

        public ActionResult SetFlow()
        {
            return PartialView("SetFlow");
        }

        public async Task<ActionResult> SetRole()
        {
            var result = await UserRoleRepository.GetListAsync();
            return PartialView(result);
        }
        
        public ActionResult EditStep()
        {
            return PartialView("EditStep");
        }

        public ActionResult EditLink()
        {
            return PartialView("EditLink");
        }

        public async Task<ActionResult> GetFlowData(long id)
        {
            var flow = await WfProcessRepository.GetByIdAsync(id);
            return Json(flow, JsonRequestBehavior.AllowGet);
        }

        // 暂存，发布之后才能使用
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Save(long id, string name, string processContent, TableSource tableSource, string remark)
        {
            var result = new JsonModel();

            try
            {
                var flow = await WfProcessRepository.GetByIdAsync(id) ?? new WfProcess();

                flow.Name = name;
                flow.ProcessContent = processContent;
                flow.TableSource = tableSource;
                flow.Remark = remark;
                flow.CommonStatus = CommonStatus.Disabled;

                await WfProcessRepository.SaveAsync(flow);
                result.Data = flow.Id;
                result.message = "保存成功。请发布流程，否则无法显示~";
            }
            catch (Exception ex)
            {
                result.message = "保存出错";
            }

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BatchDelete(string ids)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(result);
            }
            var list = await WfProcessRepository.BatchDeleteAsync(ids);
            if (list == null)
            {
                return Json(result);
            }
            var msg = string.Join(",", list.Select(m => m.Name).ToArray()).ToEllipsis(100);
            await LogRepository.Insert(TableSource.Articles, OperationType.Delete, string.Format("批量删除{0}等管理员", msg), ids);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Publish(long id, string name)
        {
            var result = new JsonModel { statusCode = 300, message = "发布失败,流程不存在！", closeCurrent = false };
            if (id <= 0)
            {
                return Json(result);
            }
            var flow = await WfProcessRepository.GetByIdAsync(id);
            if (flow == null)
            {
                return Json(result);
            }

            // 每个数据源有且只有一个发布的流程图
            var hasPublished = await WfProcessRepository.HasPublishedProcess(flow.TableSource);
            if (hasPublished)
            {
                result.message = $"发布失败，数据源【{flow.TableSource.GetDescriotion()}】已经有一个已发布的流程~";
                return Json(result);
            }

            // 不允许同名
            var existedName = await WfProcessRepository.IsExsitName(name);
            if (existedName)
            {
                result.message = $"发布失败，流程名称已存在，请更改名称~";
                return Json(result);
            }

            flow.CommonStatus = CommonStatus.Enabled;
            await WfProcessRepository.SaveAsync(flow);

            result.statusCode = 200;
            result.message = "发布成功！";
            return Json(result);
        }

        /// <summary>
        /// 是否有流程示例在运行
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [HttpPost] 
        public async Task<ActionResult> CheckIsRun(long processId)
        {
            var isRun = await WfProcessInstanceRepository.IsRunAsync(processId);
            return Json(isRun);
        }

        [HttpPost]
        public async Task<ActionResult> RunReturn(long processId)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (processId < 1)
            {
                return Json(result);
            }

            var processInstanceQuery = WfProcessInstanceRepository.QueryEnable()
                .Where(m => m.WfProcessId == processId);
            foreach (var wfProcessInstance in processInstanceQuery)
            {
                wfProcessInstance.ProcessState = WfProcessState.Canceled;
            }
            var activityInstanceQuery=WfActivityInstanceRepository.QueryEnable()
                .Where(m => m.ProcessId == processId);
            foreach (var activityInstance in activityInstanceQuery)
            {
                activityInstance.ActivityState = WfActivityState.Withdrawed;
            }
            await WfProcessRepository.SaveChangesAsync();
            result.statusCode = 200;
            return Json(result);
        }
    }
}