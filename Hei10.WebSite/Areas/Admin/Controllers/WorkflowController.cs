using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Services;
using Hei10.Domain.Enums;
using System.Threading.Tasks;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Models.Workflow;
using Hei10.WebSite.Areas.Admin.Models;
using Hei10.Domain.Models;
using Hei10.Domain.Repositories;
using Hei10.Core.Extensions;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class WorkflowController : AdminController
    {
        public IWorkflowSvc WorkflowSvc { get; set; }
        public IUserRoleSvc UserRoleSvc { get; set; }
        public IUsersRepository UsersRepository { get; set; }

        #region 选择下一步骤及人员

        /// <summary>
        /// 选择下一步骤及人员
        /// </summary>
        /// <param name="type"></param>
        /// <param name="processId"></param>
        /// <param name="currActId"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public async Task<ActionResult> FlowStepSelect(WfActivityType type,
            long processId,
            string currActId,
            string conditions,
            long proInsId = 0)
        {
            var jsonResult = new JsonModel();
            jsonResult.statusCode = 300;

            #region 找流程
            var process = await WorkflowSvc.GetProcessById(processId);
            if (process == null)
            {
                jsonResult.message = $"找不到id为{processId}的流程！";
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region 找当前节点
            WfActivity currAct = null;
            switch (type)
            {
                case WfActivityType.Start:
                    currAct = process.GetFirstActivity();
                    break;
                case WfActivityType.Task:
                    if (string.IsNullOrEmpty(currActId))
                    {
                        jsonResult.message = "请输入当前节点Id！";
                        return Json(jsonResult, JsonRequestBehavior.AllowGet);
                    }
                    currAct = process.GetActivity(currActId);
                    break;
                default:
                    break;
            }
            if (currAct == null)
            {
                jsonResult.message = "当前节点不存在！";
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region 找可选节点列表
            var condition = GetCondition(conditions);
            var nextList = process.GetNextActivityList(currAct.key, condition);
            if (nextList == null || nextList.Count == 0)
            {
                jsonResult.message = "可选节点为空！请重新编辑流程！";
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }

            List<Users> launchUsers = new List<Users>();

            var model = new List<WfActivitySelectModel>();
            foreach (var act in nextList)
            {
                switch (act.dealType)
                {
                    case WfActivityDealType.Creater:
                        var getResult = await GetLaunchUser(proInsId, launchUsers, model, act,jsonResult);
                        if (!getResult)
                        {
                            return Json(jsonResult, JsonRequestBehavior.AllowGet);
                        }
                        break;
                    case WfActivityDealType.Role:
                        await GetRoleUsers(model, act);
                        break;
                    default:
                        break;
                }
            }

            #endregion

            return View(model);
        }

        private async Task GetRoleUsers(List<WfActivitySelectModel> model, WfActivity act)
        {
            var users = await UserRoleSvc.GetUserListByRoleIds(act.roleIds);
            if (users != null && users.Count > 0)
            {
                model.Add(new WfActivitySelectModel()
                {
                    Activity = act,
                    UserList = users
                });
            }
        }

        private async Task<bool> GetLaunchUser(long proInsId, List<Users> launchUsers, List<WfActivitySelectModel> model, WfActivity act,JsonModel jsonResult)
        {
            if (launchUsers.Count == 0)
            {
                long launchUserId = 0;
                if (proInsId == 0)
                {
                    launchUserId = User.UserInfo.UserId;
                }
                else
                {
                    var proIns = await WorkflowSvc.GetProcessInstanceAsync(proInsId);
                    if (proIns == null)
                    {
                        jsonResult.message = $"找不到id为{proInsId}的流程实例！";
                        return false;
                    }
                    launchUserId = proIns.CreateUserId;
                }
                var launchUser = await UsersRepository.GetByIdAsync(launchUserId);
                launchUsers.Add(launchUser);
            }
            model.Add(new WfActivitySelectModel()
            {
                Activity = act,
                UserList = launchUsers
            });
            return true;
        }

        /// <summary>
        /// 多个条件“,”隔开，一个条件键值“_隔开”，如：Days_4,UserId_1
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        [NonAction]
        private Dictionary<string, string> GetCondition(string conditions)
        {
            var condition = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(conditions))
            {
                return condition;
            }
            var conditionArray = conditions.Split(',');
            foreach (var conditionItem in conditionArray)
            {
                if (string.IsNullOrEmpty(conditionItem))
                {
                    continue;
                }
                var itemArray = conditionItem.Split('_');
                if (itemArray.Length != 2)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(itemArray[0])
                    && !string.IsNullOrEmpty(itemArray[1]))
                {
                    condition.Add(itemArray[0], itemArray[1]);
                }
            }

            return condition;
        }


        #endregion

        #region 回退步骤确认
        public async Task<ActionResult> BackStepSelect(long proInsId,
            long actInsId)
        {
            var result = new JsonModel();
            result.statusCode = 300;
            #region 检查数据完整性
            var processInstance = await WorkflowSvc.GetProcessInstanceAsync(proInsId);
            if (processInstance == null)
            {
                result.message = $"找不到Id为{proInsId}的流程实例！";
                return Json(result);
            }
            var process = await WorkflowSvc.GetProcessById(processInstance.WfProcessId);
            if (process == null)
            {
                result.message = $"找不到Id为{processInstance.WfProcessId}的流程";
                return Json(result);
            }
            var currActInstance = await WorkflowSvc.GetActivityAsync(processInstance.Id, actInsId);
            if (currActInstance == null)
            {
                result.message = $"在流程实例[{processInstance.ProcessName}({processInstance.Id})]中找不到id为{actInsId}的节点实例";
                return Json(result);
            }
            var currActivity = process.GetActivity(currActInstance.ActivityGuid);
            if (currActivity == null)
            {
                result.message = $"在流程[{process.Name}({process.Id})]中找不到key为{currActInstance.ActivityGuid}的节点";
                return Json(result);
            }
            #endregion

            #region 获取回退节点
            WfActivity backStep = null;
            switch (currActivity.backType)
            {
                case WfActivityBackType.PreStep:
                    var preActInstance = await WorkflowSvc.GetActivityAsync(processInstance.Id, currActInstance.PreActInstanceId);
                    backStep = process.GetActivity(preActInstance.ActivityGuid);
                    break;
                case WfActivityBackType.FirstStep:
                    backStep = process.GetFirstActivity();
                    break;
                case WfActivityBackType.OtherStep:
                    backStep = process.GetActivity(currActivity.backStep);
                    break;
                default:
                    break;
            }
            #endregion

            return View(backStep);
        }
        #endregion

        #region 查看流程节点信息列表
        /// <summary>
        /// 流程信息
        /// </summary>
        /// <param name="proInsId">流程实例Id</param>
        /// <returns></returns>
        public async Task<ActionResult> FlowActivityList(long proInsId)
        {
            var activityList = await WorkflowSvc.GetActivityListAsync(proInsId);
            return View(activityList);
        }

        #endregion

        #region 处理流程：回退、发送、完成

        /// <summary>
        /// 处理节点
        /// </summary>
        /// <param name="proInsId"></param>
        /// <param name="actInsId"></param>
        /// <returns></returns>
        public async Task<ActionResult> DealActivity(long proInsId, long actInsId)
        {
            var jsonModel = new JsonModel();
            jsonModel.statusCode = 300;
            #region //获取流程实例
            var processInstance = await WorkflowSvc.GetProcessInstanceAsync(proInsId);
            if (processInstance == null)
            {
                jsonModel.message = $"找不到id为{proInsId}的流程实例！";
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region  //判断显示哪些按钮：1：回退；2：发送；3：完成。

            var process = await WorkflowSvc.GetProcessById(processInstance.WfProcessId);
            if (process == null)
            {
                jsonModel.message = $"找不到id为{processInstance.WfProcessId}的流程！";
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }

            var currActIns = await WorkflowSvc.GetActivityAsync(proInsId, actInsId);
            if (currActIns == null)
            {
                jsonModel.message = $"流程{processInstance.ProcessName}({proInsId})中找不到id为{actInsId}的节点实例！";
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }

            if (currActIns.ActivityState != WfActivityState.Running)
            {
                jsonModel.message = $"该节点实例[{currActIns.ActivityName}(id:{currActIns.Id})]已经处理，状态为[{currActIns.ActivityState.GetDescriotion()}]！";
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }

            //如果是第一个节点，这跳转到对应的申请页面
            var firstAct = process.GetFirstActivity();
            if (firstAct.key == currActIns.ActivityGuid)
            {
                return RedirectToAction("Index", "WorkflowApply", new
                {
                    proInsId = processInstance.Id,
                    id = processInstance.SourceId,
                    currActInsId = currActIns.Id,
                    src = processInstance.TableSource
                });
            }

            WfActivityButton buttons = WfActivityButton.None;
            //是否有回退按钮
            if (currActIns.PreActInstanceId != 0)
            {
                buttons = buttons | WfActivityButton.FallBack;
            }
            var conditions = GetCondition(processInstance.Conditions);
            var nextList = process.GetNextActivityList(currActIns.ActivityGuid, conditions);

            if (nextList == null || nextList.Count == 0)
            {
                jsonModel.message = "可选节点为空！请重新编辑流程！";
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            if (nextList.Any(m => m.activityType == WfActivityType.End))
            {
                buttons = buttons | WfActivityButton.Complete;
            }
            if (nextList.Any(m => m.activityType == WfActivityType.Task))
            {
                buttons = buttons | WfActivityButton.Send;
            }
            #endregion

            #region 获取历史处理信息
            var dealActivityList = await WorkflowSvc.GetDealActivityListAsync(processInstance.Id);
            #endregion

            #region  //获取展示信息
            var src = await WorkflowSvc.GetSourceAsync(processInstance.TableSource, processInstance.SourceId);
            if (src == null)
            {
                jsonModel.message = $"找不到id为{processInstance.SourceId}的数据源{processInstance.TableSource}！";
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            #endregion

            var model = new DealActivityViewModel()
            {
                ActivityInstance = currActIns,
                Buttons = buttons,
                SourceData = src,
                TableSource = processInstance.TableSource,
                DealActivityList = dealActivityList,
                ProcessInstance = processInstance
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DealActivity(DealActivityModel model)
        {
            var jsonModel = await WorkflowSvc.DealProcess(model.ProcessInstanceId,
                model.ActivityInstanceId,
                model.ClickButton,
                model.Comment,
                model.NextStepId,
                model.AssignToUserIds,
                model.AssignToUserNames,
                model.BackStepId);

            return Json(jsonModel);
        }

        #endregion

        #region 我的申请

        public ActionResult MyApplyList(string name, string title, DateTime? startTime,
            DateTime? endTime, WfProcessState? state, string activityName, string orderField, string orderDirection, int pageSize = 30, int pageCurrent = 1)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            var list = WorkflowSvc.GetApplyList(true, name, title, startTime,
            endTime, state, activityName, orderField, orderDirection,pageSize, pageCurrent);
            return View(list);
        }

        public async Task<ActionResult> ApplyInfo(TableSource src, long srcId)
        {
            var jsonModel = new JsonModel();
            jsonModel.statusCode = 300;
            var source = await WorkflowSvc.GetSourceAsync(src, srcId);
            if (source == null)
            {
                jsonModel.message = $"找不到id为{src}的数据源{srcId}！";
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            var model = new DealActivityViewModel()
            {
                TableSource = src,
                SourceData = source
            };
            return View(model);
        }
        #endregion

        #region 待办事项
        public ActionResult MyTaskList(int pageSize = 30, int pageCurrent = 1)
        {
            var list = WorkflowSvc.GetMyTaskList(pageSize, pageCurrent);
            return View(list);
        }
        #endregion

        #region 已办事项

        public ActionResult MyCompleteTaskList(string keyWord, DateTime? createStartTime,DateTime? createEndTime,DateTime? dealStartTime,DateTime? dealEndTime, string orderField = "Id", string orderDirection = "desc", int pageSize = 30,int pageCurrent = 1)
        {
            ViewBag.KeyWord = keyWord;
            ViewBag.CreateStartTime = createStartTime;
            ViewBag.CreateEndTime = createEndTime;
            ViewBag.DealStartTime = dealStartTime;
            ViewBag.DealEndTime = dealEndTime;
            var list = WorkflowSvc.GetMyCompleteTaskList((keyWord??"").Trim(), createStartTime, createEndTime, dealStartTime, dealEndTime, orderField, orderDirection, pageSize, pageCurrent);
            return View(list);
        }

        #endregion

        #region 全部申请

        public ActionResult ApplyList(string name, string title, DateTime? startTime,
            DateTime? endTime, WfProcessState? state, string activityName, string orderField, string orderDirection, int pageSize = 30, int pageCurrent = 1)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            var list = WorkflowSvc.GetApplyList(false, name, title, startTime,
            endTime, state, activityName, orderField, orderDirection, pageSize, pageCurrent);
            return View(list);
        }
        #endregion

        #region // 查看流程图处理状态
        public async Task<ActionResult> FlowProcessing(long proInsId)
        {
            var flowInstance = await WorkflowSvc.GetProcessInstanceAsync(proInsId);
            bool isCompleted = (flowInstance != null && flowInstance.ProcessState == WfProcessState.Completed);
            ViewBag.IsCompleted = isCompleted.ToString().ToLower();

            var lstKeys = WorkflowSvc.GetActivityPathAsync(proInsId);
            ViewBag.FlowPathKeys = String.Join(",", lstKeys);
            ViewBag.CurrFlowData = await WorkflowSvc.GetProcessContentAsync(proInsId);

            return View();
        }
        #endregion
    }
}