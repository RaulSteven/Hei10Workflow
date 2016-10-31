using Hei10.Domain.Enums;
using Hei10.Domain.Models.Workflow;
using Hei10.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using PagedList;

namespace Hei10.Domain.Services
{
    /// <summary>
    /// 工作流引擎
    /// </summary>
    public interface IWorkflowSvc
    {
        //发起流程 StartProcess
        //下一个节点 GetNextActivity
        //放弃申请 WithdrawProcess
        //退回上一步 SendBackProcess


        Task InitTestProcess();

        #region 节点实例
        IPagedList<WfActivityInstance> GetMyTaskList(int pageSize = 30, int pageCurrent = 1);
        IPagedList<WfActivityInstance> GetMyCompleteTaskList(string keyWord, DateTime? createStartTime, DateTime? createEndTime, DateTime? dealStartTime, DateTime? dealEndTime, string orderField, string orderDirection,int pageSize = 30, int pageCurrent = 1);
        Task<List<WfActivityInstance>> GetActivityListAsync(long prosInsId);
        List<string> GetActivityPathAsync(long prosInsId);
        Task<List<WfActivityInstance>> GetDealActivityListAsync(long prosInsId);
        Task<WfActivityInstance> GetActivityAsync(long prosInsId, long id);

        Task<WfActivityInstance> GetRunningActivity(long proInsId);
        #endregion

        #region 流程
        Task<WfProcess> GetProcessBySource(TableSource src);

        Task<WfProcess> GetProcessById(long id);
        Task<JsonModel> StartProcess(long processId,
            long sourceId,
            string condition,
            string nextStepId,
            string assignToUserIds,
            string assignToUserNames);

        Task<JsonModel> DealProcess(long proInsId, 
            long currActInsId, 
            WfActivityButton clickBtn, 
            string comment, 
            string nextStepId,
            string assignToUserIds, 
            string assignToUserNames,
            string backStepId = "");

        #endregion

        #region 流程实例


        Task<WfProcessInstance> GetProcessInstanceAsync(long id);

        IPagedList<WfProcessInstance> GetApplyList(bool isOwn, string name, string title, DateTime? startTime,
            DateTime? endTime, WfProcessState? state, string activityName, string orderField, string orderDirection,
            int pageSize = 30, int pageCurrent = 1);
        #endregion



        Task<AggregateRoot> GetSourceAsync(TableSource src, long srcId);

        // 获取流程图数据
        Task<string> GetProcessContentAsync(long proInsId);
    }
}
