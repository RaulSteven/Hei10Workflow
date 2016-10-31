using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using Hei10.Domain.Models.Workflow;
using Hei10.Domain.Enums;
using Hei10.Domain.ViewModels;
using Hei10.WebSite.Areas.Admin.Models;
using System.Threading.Tasks;
using AutoMapper;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class WorkflowApplyController : AdminController
    {
        public IWorkflowSvc WorkflowSvc { get; set; }
        public ILeaveInfoRepository LeaveInfoRepository { get; set; }
        public ISealInfoRepository SealInfoRepository { get; set; }
        public IMeetingRoomInfoRepository MeetingRoomInfoRepository { get; set; }

        #region 申请页面加载
        public async Task<ActionResult> Index(TableSource src, long proInsId = 0, long currActInsId = 0, long id = 0)
        {
            var result = new JsonModel();
            result.statusCode = 300;
            var process = await WorkflowSvc.GetProcessBySource(src);
            if (process == null)
            {
                result.message = $"请假流程还未创建，请先创建{src}流程！";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var model = await GetModel(src, id, result);
            if (model == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            model.Id = id;
            model.ProcessId = process.Id;
            model.ProcessInstanceId = proInsId;
            model.CurrActInsId = currActInsId;
            model.DealActivityList = new List<WfActivityInstance>();
            model.TableSource = src;

            if (proInsId != 0)
            {
                model.DealActivityList = await WorkflowSvc.GetDealActivityListAsync(proInsId);
            }
            return View(model);
        }

        [NonAction]
        private async Task<WfApplyBase> GetModel(TableSource src, long id, JsonModel result)
        {
            WfApplyBase model = null;
            switch (src)
            {
                case TableSource.LeaveInfo:
                    model = new LeaveInfoModel()
                    {
                        Days = 1,
                        StartTime = DateTime.Now.AddDays(1),
                        EndTime = DateTime.Now.AddDays(2)
                    };
                    break;
                case TableSource.SealInfo:
                    model = new SealInfoModel()
                    {
                        ApplyUserName = User.UserInfo.RealName,
                        ApplyTime = DateTime.Now
                    };
                    break;
                case TableSource.MeetingRoomInfo:
                    model = new MeetingRoomInfoModel()
                    {
                        UsedTime = DateTime.Now,
                        ApplyTime = DateTime.Now,
                        ApplyName = User.UserInfo.RealName
                    };
                    break;
                default:
                    break;
            }

            if (id != 0)
            {
                var sourceData = await WorkflowSvc.GetSourceAsync(src, id);
                if (sourceData == null)
                {
                    result.message = $"找不到Id为{model.Id}的{src}！";
                    return null;
                }
                Mapper.Map(sourceData, model);
            }

            return model;
        }

        #endregion

        #region 表单提交
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApplyLeaveInfo(LeaveInfoModel model)
        {
            var result = new JsonModel();
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            #region //插入业务数据
            LeaveInfo leaveInfo = null;
            if (model.Id != 0)
            {
                leaveInfo = await LeaveInfoRepository.GetByIdAsync(model.Id);
            }
            if (leaveInfo == null)
            {
                leaveInfo = new LeaveInfo();
            }
            Mapper.Map(model, leaveInfo);
            leaveInfo.CommonStatus = CommonStatus.Disabled;
            await LeaveInfoRepository.SaveAsync(leaveInfo);
            #endregion

            #region 处理流程
            await DealProcess(model, leaveInfo.Id, result, "Days_" + leaveInfo.Days);
            #endregion

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApplySealInfo(SealInfoModel model)
        {
            var result = new JsonModel();
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            #region //插入业务数据
            SealInfo sealInfo = null;
            if (model.Id != 0)
            {
                sealInfo = await SealInfoRepository.GetByIdAsync(model.Id);
            }
            if (sealInfo == null)
            {
                sealInfo = new SealInfo();
            }
            Mapper.Map(model, sealInfo);
            sealInfo.CommonStatus = CommonStatus.Disabled;
            await SealInfoRepository.SaveAsync(sealInfo);
            #endregion

            #region 处理流程
            await DealProcess(model, sealInfo.Id, result);
            #endregion

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApplyMeetingRoomInfo(MeetingRoomInfoModel model)
        {
            var result = new JsonModel();
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            #region //插入业务数据
            MeetingRoomInfo meetingInfo = null;
            if (model.Id != 0)
            {
                meetingInfo = await MeetingRoomInfoRepository.GetByIdAsync(model.Id);
            }
            if (meetingInfo == null)
            {
                meetingInfo = new MeetingRoomInfo();
            }
            Mapper.Map(model, meetingInfo);
            meetingInfo.CommonStatus = CommonStatus.Disabled;
            await MeetingRoomInfoRepository.SaveAsync(meetingInfo);
            #endregion

            #region 处理流程
            await DealProcess(model, meetingInfo.Id, result);
            #endregion

            return Json(result);
        }

        #endregion

        #region 处理流程
        [NonAction]
        private async Task DealProcess(WfApplyBase model, long srcId, JsonModel result, string conditions=null)
        {
            if (model.ProcessInstanceId != 0)
            {
                var processInstance = await WorkflowSvc.GetProcessInstanceAsync(model.ProcessInstanceId);
                if (processInstance == null)
                {
                    result.statusCode = 300;
                    result.message = $"找不到id为{model.ProcessInstanceId}的流程实例！";
                    return;
                }
                processInstance.Conditions = conditions;
                result = await WorkflowSvc.DealProcess(model.ProcessInstanceId,
                    model.CurrActInsId,
                    WfActivityButton.Send,
                    "重新提交信息",
                    model.NextStepId,
                    model.AssignToUserIds,
                    model.AssignToUserNames);
                return;
            }
            //创建流程
            result = await WorkflowSvc.StartProcess(model.ProcessId,
                srcId,
                conditions,
                model.NextStepId,
                model.AssignToUserIds,
                model.AssignToUserNames);
            if (result.statusCode == 200)
            {
                result.message = "申请成功，请等待审批！";
            }
        }

        #endregion
    }
}