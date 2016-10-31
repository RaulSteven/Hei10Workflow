using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Models.Workflow;
using Hei10.Domain.Repositories;
using Hei10.Domain.Enums;
using Newtonsoft.Json;
using System.Data.Entity;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Infrastructure;
using PagedList;

namespace Hei10.Domain.Services
{
    public class WorkflowSvc : BaseSvc, IWorkflowSvc
    {
        public IWfProcessRepository WfProcessRepository { get; set; }
        public IWfActivityInstanceRepository WfActivityInstanceRepository { get; set; }
        public IWfProcessInstanceRepository WfProcessInstanceRepository { get; set; }
        public ILeaveInfoRepository LeaveInfoRepository { get; set; }
        public ISealInfoRepository SealInfoRepository { get; set; }

        public IMeetingRoomInfoRepository MeetingRoomInfoRepository { get; set; }
        public async Task InitTestProcess()
        {
            //生成一条测试用流程,流程如下：
            //Start - 填写请假信息 - 部门经理审核 - 人事审核 - 结束。
            //                                    -(小于5天) 副总经理审核 - 人事审核 - 结束
            //                                    -(大于5天)总经理审核 - 人事审核 - 结束
            //                                               
            int roleIdDept = 11,
                roleIdHuRes = 12,
                roleIdDeputy = 13,
                roleIdManager = 14;
            var process = new WfProcess();
            process.CommonStatus = CommonStatus.Enabled;
            process.Name = "请假流程";
            process.Remark = "流程测试范例";
            process.TableSource = TableSource.LeaveInfo;

            #region 定义流程信息
            var processModel = new WfProcessModel();
            processModel.linkDataArray = new List<WfTransition>();
            processModel.nodeDataArray = new List<WfActivity>();
            #region start 节点
            //start节点
            var actStart = new WfActivity()
            {
                activityType = WfActivityType.Start,
                key = Guid.NewGuid().ToString(),
                text = "开始"
            };
            processModel.nodeDataArray.Add(actStart);

            #endregion

            #region //填写信息
            var actInput = new WfActivity()
            {
                activityType = WfActivityType.Task,
                key = Guid.NewGuid().ToString(),
                text = "填写信息"
            };
            processModel.nodeDataArray.Add(actInput);

            //link
            var linkStart2Input = new WfTransition()
            {
                from = actStart.key,
                to = actInput.key
            };
            processModel.linkDataArray.Add(linkStart2Input);

            #endregion

            #region 部门经理审核
            var actDept = new WfActivity()
            {
                activityType = WfActivityType.Task,
                key = Guid.NewGuid().ToString(),
                text = "部门经理审批",
                roleIds = roleIdDept.ToString()
            };
            processModel.nodeDataArray.Add(actDept);

            //link
            var linkInput2Dept = new WfTransition()
            {
                from = actInput.key,
                to = actDept.key
            };
            processModel.linkDataArray.Add(linkInput2Dept);
            #endregion

            #region 副总经理审核
            var actDeputy = new WfActivity()
            {
                activityType = WfActivityType.Task,
                key = Guid.NewGuid().ToString(),
                text = "副总经理审批",
                roleIds = roleIdDeputy.ToString()
            };
            processModel.nodeDataArray.Add(actDeputy);

            //link
            var linkDept2Deputy = new WfTransition()
            {
                from = actDept.key,
                to = actDeputy.key,
                condition = " Days<5 ",
            };
            processModel.linkDataArray.Add(linkDept2Deputy);
            #endregion

            #region 总经理审核
            var actManager = new WfActivity()
            {
                activityType = WfActivityType.Task,
                key = Guid.NewGuid().ToString(),
                text = "总经理审批",
                roleIds = roleIdManager.ToString()
            };
            processModel.nodeDataArray.Add(actManager);

            //link
            var linkDept2Manager = new WfTransition()
            {
                from = actDept.key,
                to = actManager.key,
                condition = " Days>5 ",
            };
            processModel.linkDataArray.Add(linkDept2Manager);
            #endregion

            #region 人事审核
            var actHuRes = new WfActivity()
            {
                activityType = WfActivityType.Task,
                key = Guid.NewGuid().ToString(),
                text = "人事审批",
                roleIds = roleIdHuRes.ToString()
            };
            processModel.nodeDataArray.Add(actHuRes);

            //link 部门-人事
            var linkDept2HuRes = new WfTransition()
            {
                from = actDept.key,
                to = actHuRes.key,
            };
            processModel.linkDataArray.Add(linkDept2HuRes);
            //link 副总-人事
            var linkDeputy2HuRes = new WfTransition()
            {
                from = actDeputy.key,
                to = actHuRes.key,
            };
            processModel.linkDataArray.Add(linkDeputy2HuRes);
            //link 部门-人事
            var linkManager2HuRes = new WfTransition()
            {
                from = actManager.key,
                to = actHuRes.key,
            };
            processModel.linkDataArray.Add(linkManager2HuRes);
            #endregion

            #region 结束
            var actEnd = new WfActivity()
            {
                activityType = WfActivityType.End,
                key = Guid.NewGuid().ToString(),
                text = "结束",
            };
            processModel.nodeDataArray.Add(actEnd);

            //link
            var linkHuRes2End = new WfTransition()
            {
                from = actHuRes.key,
                to = actEnd.key,
            };
            processModel.linkDataArray.Add(linkHuRes2End);
            #endregion
            #endregion

            process.ProcessContent = JsonConvert.SerializeObject(processModel);

            await WfProcessRepository.SaveAsync(process);
        }

        #region 流程实例


        public Task<WfProcessInstance> GetProcessInstanceAsync(long id)
        {
            return WfProcessInstanceRepository.GetEnableByIdAsync(id);
        }


        public IPagedList<WfProcessInstance> GetApplyList(bool isOwn, string name, string title, DateTime? startTime, DateTime? endTime, WfProcessState? state, string activityName, string orderField, string orderDirection, int pageSize = 30, int pageCurrent = 1)
        {
            return WfProcessInstanceRepository.GetApplyPagedList(isOwn, name, title, startTime,
            endTime, state, activityName, orderField, orderDirection, pageCurrent, pageSize);
        }
        #endregion

        #region 流程

        public Task<WfProcess> GetProcessBySource(TableSource src)
        {
            return WfProcessRepository.GetBySource(src);
        }

        public async Task<WfProcess> GetProcessById(long id)
        {
            var process = await WfProcessRepository.GetEnableByIdAsync(id);
            if (process != null)
            {
                process.InitNodeLink();
            }
            return process;
        }

        public async Task<JsonModel> StartProcess(long processId,
            long sourceId,
            string condition,
            string nextStepId,
            string assignToUserIds,
            string assignToUserNames)
        {
            var result = new JsonModel();
            //创建流程实例
            var process = await GetProcessById(processId);
            if (result == null)
            {
                result.statusCode = 300;
                result.message = "流程不存在！processId:" + processId;
                return result;
            }
            var processInstance = new WfProcessInstance()
            {
                CommonStatus = CommonStatus.Enabled,
                ProcessName = process.Name,
                ProcessState = WfProcessState.Running,
                SourceId = sourceId,
                TableSource = process.TableSource,
                WfProcessId = process.Id,
                Conditions = condition
            };
            await WfProcessInstanceRepository.SaveAsync(processInstance);

            //创建流程节点
            var firstAct = process.GetFirstActivity();
            if (firstAct == null)
            {
                result.statusCode = 300;
                result.message = $"流程 {process.Name}的第一个节点找不到！";
                return result;
            }
            WfActivityInstance firstActInstance = CreateActivityInstance(process,
                processInstance,
                firstAct,
                WfActivityState.Completed,
                User.UserInfo.UserId,
                User.UserInfo.RealName);
            firstActInstance.DealTime = DateTime.Now;
            firstActInstance.DealUserId = User.UserInfo.UserId;
            firstActInstance.DealUserName = User.UserInfo.RealName;
            await WfActivityInstanceRepository.SaveAsync(firstActInstance);

            //创建下一步节点
            var nextAct = process.GetActivity(nextStepId);
            if (nextAct == null)
            {
                result.statusCode = 300;
                result.message = $"流程 {process.Name}的节点 {nextStepId} 找不到！";
                return result;
            }
            var nextActInstance = CreateActivityInstance(process,
                processInstance,
                nextAct,
                WfActivityState.Running,
                assignToUserIds,
                assignToUserNames);
            nextActInstance.PreActInstanceId = firstActInstance.Id;
            await WfActivityInstanceRepository.SaveAsync(nextActInstance);

            processInstance.CurrActivityName = nextActInstance.ActivityName;
            await WfProcessInstanceRepository.SaveAsync(processInstance);

            return result;
        }



        /// <summary>
        /// 创建节点实例
        /// </summary>
        /// <param name="process"></param>
        /// <param name="processInstance"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        private WfActivityInstance CreateActivityInstance(WfProcess process,
            WfProcessInstance processInstance,
            WfActivity activity,
            WfActivityState state,
            string assignToUserId,
            string assignToUserName)
        {
            return new WfActivityInstance()
            {
                ActivityGuid = activity.key,
                ActivityName = activity.text,
                ActivityState = state,
                ActivityType = activity.activityType,
                CommonStatus = CommonStatus.Enabled,
                ProcessId = process.Id,
                ProcessInstanceId = processInstance.Id,
                ProcessName = process.Name,
                AssignToUserIds = assignToUserId,
                AssignToUserNames = assignToUserName
            };
        }

        private WfActivityInstance CreateActivityInstance(WfProcess process,
            WfProcessInstance processInstance,
            WfActivity activity,
            WfActivityState state,
            long assignToUserId,
            string assignToUserName)
        {
            return CreateActivityInstance(process,
                processInstance,
                activity,
                state,
                $"[{assignToUserId}]",
                assignToUserName);
        }

        public async Task<JsonModel> DealProcess(long proInsId,
            long currActInsId,
            WfActivityButton clickBtn,
            string comment,
            string nextStepId,
            string assignToUserIds,
            string assignToUserNames,
            string backStepId)
        {
            var jsonModel = new JsonModel();
            jsonModel.statusCode = 300;
            #region //获取流程数据
            var processInstance = await GetProcessInstanceAsync(proInsId);
            if (processInstance == null)
            {
                jsonModel.message = $"找不到id为{proInsId}的流程实例！";
                return jsonModel;
            }

            var process = await GetProcessById(processInstance.WfProcessId);
            if (process == null)
            {
                jsonModel.message = $"找不到id为{processInstance.WfProcessId}的流程！";
                return jsonModel;
            }

            var currActIns = await GetActivityAsync(proInsId, currActInsId);
            if (currActIns == null)
            {
                jsonModel.message = $"流程{processInstance.ProcessName}({processInstance.Id})中找不到id为{currActInsId}的节点实例！";
                return jsonModel;
            }
            #endregion

            #region 当前节点处理
            currActIns.Comment = comment;
            currActIns.DealTime = DateTime.Now;
            currActIns.DealUserId = User.UserInfo.UserId;
            currActIns.DealUserName = User.UserInfo.RealName;
            currActIns.ActivityState = WfActivityState.Completed;
            await WfActivityInstanceRepository.SaveAsync(currActIns);
            #endregion

            switch (clickBtn)
            {
                case WfActivityButton.FallBack:
                    await FallBackProgress(processInstance, process, currActIns, backStepId);
                    jsonModel.statusCode = 200;
                    break;
                case WfActivityButton.Send:
                    await SendProgress(nextStepId, assignToUserIds, assignToUserNames, jsonModel, processInstance, process, currActIns);
                    jsonModel.statusCode = 200;
                    break;
                case WfActivityButton.Complete:
                    await CompleteProcess(processInstance);
                    jsonModel.statusCode = 200;
                    break;
                default:
                    jsonModel.message = $"错误的按钮:{clickBtn}！";
                    break;
            }

            return jsonModel;
        }

        private async Task CompleteProcess(WfProcessInstance processInstance)
        {
            processInstance.ProcessState = WfProcessState.Completed;
            processInstance.CurrActivityName = "结束";

            //TODO:更新数据源的状态
            var src = await GetSourceAsync(processInstance.TableSource, processInstance.SourceId);
            src.CommonStatus = CommonStatus.Enabled;
            await WfProcessInstanceRepository.SaveAsync(processInstance);
        }

        private async Task SendProgress(string nextStepId,
            string assignToUserIds,
            string assignToUserNames,
            JsonModel jsonModel,
            WfProcessInstance processInstance,
            WfProcess process,
            WfActivityInstance currActIns)
        {
            var nextAct = process.GetActivity(nextStepId);
            if (nextAct == null)
            {
                jsonModel.statusCode = 300;
                jsonModel.message = $"流程 {process.Name}的节点 {nextStepId} 找不到！";
                return;
            }

            var nextActInstance = CreateActivityInstance(process,
               processInstance,
               nextAct,
               WfActivityState.Running,
               assignToUserIds,
               assignToUserNames);
            nextActInstance.PreActInstanceId = currActIns.Id;
            processInstance.CurrActivityName = nextActInstance.ActivityName;
            await WfActivityInstanceRepository.SaveAsync(nextActInstance);
        }

        /// <summary>
        /// 生成回退节点
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="process"></param>
        /// <param name="currActIns"></param>
        /// <returns></returns>
        private async Task FallBackProgress(WfProcessInstance processInstance,
            WfProcess process,
            WfActivityInstance currActIns,
            string backStepId)
        {

            var preActInstance = await WfActivityInstanceRepository.GetLatestActivityAsync(processInstance.Id, backStepId);
            var backActivity = process.GetActivity(backStepId);
            var backActInstance = CreateActivityInstance(process,
                processInstance,
                backActivity,
                WfActivityState.Running,
                preActInstance.AssignToUserIds,
                preActInstance.AssignToUserNames);
            backActInstance.PreActInstanceId = preActInstance.PreActInstanceId;
            backActInstance.BackActivityInstanceId = currActIns.Id;
            processInstance.CurrActivityName = backActInstance.ActivityName;
            currActIns.ActivityState = WfActivityState.Sendbacked;
            await WfActivityInstanceRepository.SaveAsync(backActInstance);
        }

        #endregion

        #region 节点实例  

        public IPagedList<WfActivityInstance> GetMyTaskList(int pageSize = 30, int pageCurrent = 1)
        {
            var userId = $"[{User.UserInfo.UserId}]";
            var queryProcess = WfProcessInstanceRepository.AdminQueryEnable()
                .Where(m => m.ProcessState == WfProcessState.Running);

            var queryActivity = WfActivityInstanceRepository
                .AdminQueryEnable()
                .Where(m => m.ActivityState == WfActivityState.Running
                    && m.ActivityType == WfActivityType.Task
                    && !string.IsNullOrEmpty(m.AssignToUserIds)
                    && m.AssignToUserIds.Contains(userId));
            queryActivity = queryActivity.Join(
                    queryProcess,
                    act => act.ProcessInstanceId,
                    pro => pro.Id,
                    (act, pro) => act);
            var list = queryActivity
                .OrderByDescending(m => m.Id)
                .ToPagedList(pageCurrent, pageSize);
            return list;
        }

        public IPagedList<WfActivityInstance> GetMyCompleteTaskList(string keyWord, DateTime? createStartTime, DateTime? createEndTime, DateTime? dealStartTime, DateTime? dealEndTime, string orderField, string orderDirection, int pageSize = 30, int pageCurrent = 1)
        {
            var query = WfActivityInstanceRepository.AdminQueryEnable().Where(m => m.ActivityType == WfActivityType.Task && m.DealUserId == User.UserInfo.UserId);
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(d => d.ProcessName.Contains(keyWord) ||
                                         d.CreateUserName.Contains(keyWord) ||
                                         d.ActivityName.Contains(keyWord));
            }
            if (createStartTime.HasValue)
            {
                query = query.Where(d => d.CreateTime >= createStartTime.Value);
            }
            if (createEndTime.HasValue)
            {
                query = query.Where(d => d.CreateTime <= createEndTime.Value);
            }
            if (dealStartTime.HasValue)
            {
                query = query.Where(d => d.DealTime >= dealStartTime.Value);
            }
            if (dealEndTime.HasValue)
            {
                query = query.Where(d => d.DealTime <= dealEndTime.Value);
            }
            return ToPageList(query, orderField, orderDirection, m => m.Id, pageCurrent, pageSize); ;
        }

        public Task<List<WfActivityInstance>> GetActivityListAsync(long prosInsId)
        {
            var list = WfActivityInstanceRepository
                .AdminQueryEnable()
                .Where(m => m.ProcessInstanceId == prosInsId)
                .OrderBy(m => m.Id)
                .ToListAsync();
            return list;
        }

        public List<string> GetActivityPathAsync(long prosInsId)
        {
            var list = WfActivityInstanceRepository
                .AdminQueryEnable()
                .Where(m => m.ProcessInstanceId == prosInsId &&
                            (m.ActivityState == WfActivityState.Completed || m.ActivityState == WfActivityState.Running))
                .OrderBy(m => m.Id)
                .Select(m => m.ActivityGuid)
                .ToListAsync();

            // 流程图路径
            var pathKeys = new List<string>();

            foreach (var guid in list.Result)
            {
                // 如果被退回
                if (pathKeys.Contains(guid))
                {
                    // 移除该步骤之后的路径
                    var index = pathKeys.IndexOf(guid) + 1;
                    pathKeys.RemoveRange(index, pathKeys.Count - index);

                    continue;
                }

                pathKeys.Add(guid);
            }

            return pathKeys;
        }

        public Task<List<WfActivityInstance>> GetDealActivityListAsync(long prosInsId)
        {
            var list = WfActivityInstanceRepository
                .AdminQueryEnable()
                .Where(m => m.ProcessInstanceId == prosInsId
                    && m.DealTime.HasValue)
                .OrderBy(m => m.Id)
                .ToListAsync();
            return list;
        }

        public Task<WfActivityInstance> GetActivityAsync(long prosInsId, long id)
        {
            var obj = WfActivityInstanceRepository
                .AdminQueryEnable()
                .FirstOrDefaultAsync(m => m.ProcessInstanceId == prosInsId
                    && m.Id == id);
            return obj;
        }

        public Task<WfActivityInstance> GetRunningActivity(long proInsId)
        {
            var obj = WfActivityInstanceRepository
                .AdminQueryEnable()
                .FirstOrDefaultAsync(m => m.ProcessInstanceId == proInsId
                    && m.ActivityState == WfActivityState.Running);
            return obj;
        }
        #endregion

        // 获取流程图数据
        public async Task<string> GetProcessContentAsync(long proInsId)
        {
            var flowInstance = await GetProcessInstanceAsync(proInsId);
            if (flowInstance == null) return "";

            var flow = await GetProcessById(flowInstance.WfProcessId);
            return flow == null ? "" : flow.ProcessContent;
        }


        public async Task<AggregateRoot> GetSourceAsync(TableSource src, long srcId)
        {
            switch (src)
            {
                case TableSource.LeaveInfo:
                    return (AggregateRoot)(await LeaveInfoRepository.GetByIdAsync(srcId));
                case TableSource.SealInfo:
                    return (AggregateRoot)(await SealInfoRepository.GetByIdAsync(srcId));
                case TableSource.MeetingRoomInfo:
                    var model = await MeetingRoomInfoRepository.GetByIdAsync(srcId);
                    return (AggregateRoot)(model);
                default:
                    break;
            }
            return null;
        }

    }
}
