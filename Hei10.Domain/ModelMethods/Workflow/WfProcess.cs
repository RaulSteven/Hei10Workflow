using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hei10.Domain.Enums;
using Hei10.Core.Utilities;
using System.Linq.Expressions;

namespace Hei10.Domain.Models.Workflow
{
    public partial class WfProcess
    {
        #region Properties
        [NotMapped]
        public List<WfActivity> ActivityLst { get; set; }
        [NotMapped]
        public List<WfTransition> TransitionLst { get; set; }
        #endregion

        #region 初始化节点及链接
        //{ "class": "go.GraphLinksModel",
        //  "modelData": {"position":"-5 -5"},
        //  "nodeDataArray": [
        //{"key":"d69f6b52-3862-43f2-8d5a-b4871562f1e8", "text":"填写表单 ", "figure":"RoundedRectangle", "fill":"#FFAD00", "loc":"292 157", "roleIds":"10,7", "roleNames":"研发部门,管理员管理", "remark":"测试备注"},
        //{"key":"d69f6b52-3862-43f2-8d5a-b4871562f1e82", "text":"分管领导审核 ", "figure":"RoundedRectangle", "fill":"#FFAD00", "loc":"590 160", "remark":"分管领导的备注", "roleIds":"1,3,5", "roleNames":"超级管理员,编辑,测试"}
        // ],
        //  "linkDataArray": [ {"from":"d69f6b52-3862-43f2-8d5a-b4871562f1e8", "to":"d69f6b52-3862-43f2-8d5a-b4871562f1e82", "key":"72aaf67a-5d74-40cc-8bbd-8e8ff5822828", "text":"大于3天 ", "remark":"条件的备注", "condition":"days>3"} ]}
        public void InitNodeLink()
        {
            if (!string.IsNullOrEmpty(ProcessContent))
            {
                var processModel = JsonConvert.DeserializeObject<WfProcessModel>(ProcessContent);
                if (processModel != null)
                {
                    ActivityLst = processModel.nodeDataArray;
                    TransitionLst = processModel.linkDataArray;
                }
            }

            ActivityLst = ActivityLst ?? new List<WfActivity>();
            TransitionLst = TransitionLst ?? new List<WfTransition>();
        }

        #endregion

        #region 活动节点基本方法和流转规则处理

        #region 活动节点基本方法

        /// <summary>
        /// 获取开始节点
        /// </summary>
        /// <returns></returns>
        public WfActivity GetStartActivity()
        {
            var startActivity = ActivityLst.FirstOrDefault(m => m.activityType == WfActivityType.Start);
            return startActivity;
        }

        /// <summary>
        /// 获取结束节点
        /// </summary>
        /// <returns></returns>
        public WfActivity GetEndActivity()
        {
            var endActivity = ActivityLst.FirstOrDefault(m => m.activityType == WfActivityType.End);
            return endActivity;
        }

        /// <summary>
        /// 获取任务类型的节点列表
        /// </summary>
        /// <returns></returns>
        public List<WfActivity> GetTaskActivityList()
        {
            var list = ActivityLst.Where(m => m.activityType == WfActivityType.Task)
                .ToList();
            return list;
        }

        /// <summary>
        /// 获取当前节点的下一个节点信息，适用于明确知道当前节点仅有一个下级节点，比如说开始节点后的起始节点
        /// </summary>
        /// <param name="activityGuid"></param>
        /// <returns></returns>
        public WfActivity GetNextActivity(string activityGuid)
        {
            var transition = TransitionLst.FirstOrDefault(m => m.from == activityGuid);
            if (transition ==null)
            {
                return null;
            }
            var activity = GetActivity(transition.to);
            return activity;
        }

        /// <summary>
        /// 获取流程的第一个可办理节点
        /// </summary>
        /// <returns></returns>
        public WfActivity GetFirstActivity()
        {
            var start = GetStartActivity();
            var first = GetNextActivity(start.key);
            return first;
        }

        #endregion

        #region 流程流转解析，处理流程下一步流转条件等规则

        /// <summary>
        /// 获取当前节点的下一步节点列表，伴随运行时的条件信息
        /// </summary>
        /// <param name="currGuid"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public List<WfActivity> GetNextActivityList(string currGuid,Dictionary<string,string> conditions = null)
        {
            var transitionLst = GetForwardTransitionList(currGuid);
            if (transitionLst == null || transitionLst.Count == 0 )
            {
                return null;
            }
            var activiyLst = new List<WfActivity>();
            foreach (var transition in transitionLst)
            {
                var isValidTransition = IsValidTransition(transition, conditions);
                if (isValidTransition)
                {
                    activiyLst.Add(GetActivity(transition.to));
                }
            }
            return activiyLst;
        }

        /// <summary>
        /// 是否满足Transition的条件
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private bool IsValidTransition(WfTransition transition,Dictionary<string,string> condition)
        {
            var isValid = false;
            if (string.IsNullOrEmpty(transition.condition))
            {
                isValid = true;
                return isValid;
            }
            if (condition == null || condition.Count == 0)
            {
                return isValid;
            }
            isValid = ParseCondition(transition, condition);

            return isValid;
        }

        /// <summary>
        /// 用LINQ解析条件表达式
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private bool ParseCondition(WfTransition transition,Dictionary<string,string> condition)
        {
            var expression = ReplaceParameterToValue(transition.condition, condition);
            Expression expr = System.Linq.Dynamic.DynamicExpression.Parse(typeof(Boolean), expression);
            LambdaExpression lamb = Expression.Lambda(expr);
            var result = (Func<bool>)lamb.Compile();
            return result();
        }

        /// <summary>
        /// 替换条件表达式中的条件
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private string ReplaceParameterToValue(string expression,Dictionary<string,string> condition)
        {
            foreach (var item in condition)
            {
                if (!RegexUtility.IsNumeric(item.Value))
                {
                    var str = "\"" + item.Value + "\"";
                    expression = expression.Replace(item.Key, str);
                }
                else
                {
                    expression = expression.Replace(item.Key, item.Value);
                }
            }
            return expression;
        }

        #endregion
        #endregion

        #region 活动节点操作

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public WfActivity GetActivity(string guid)
        {
            var activity = ActivityLst.FirstOrDefault(m => m.key == guid);
            return activity;
        }

        #endregion

        #region 连接线操作

        /// <summary>
        /// 获取节点的前驱连线
        /// </summary>
        /// <param name="fromActivityGuid"></param>
        /// <returns></returns>
        internal List<WfTransition> GetForwardTransitionList(string fromActivityGuid)
        {
            var list = TransitionLst.Where(m => m.from == fromActivityGuid)
                .ToList();
            return list;
        }
        #endregion
    }
}
