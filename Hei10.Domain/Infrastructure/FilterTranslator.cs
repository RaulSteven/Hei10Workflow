using Hei10.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Newtonsoft.Json;
using Hei10.Core.Utilities;

namespace Hei10.Domain.Infrastructure
{
    public class FilterTranslator
    {
        protected const char paramPrefixToken = '@';
        protected const char groupLeftToken = '(';
        protected const char groupRightToken = ')';
        protected const char likeToken = '%';
        private int paramCounter = 0;

        public FilterGroup Group { get; set; }
        public string CommandText { get;private set; }
        public List<object> ListParams { get; private set; }
        public IJRUser User { get; set; }
        public Type SourceType { get; set; }

        private readonly static List<string> currentParamMath = new List<string>
        {
            $"{FilterCurrent.CurrentUserId}",
            $"{FilterCurrent.CurrentRoleId}",
            $"{FilterCurrent.CurrentDeptId}"
        };

        public FilterTranslator(FilterGroup group,Type sourceType, IJRUser user)
        {
            Group = group;
            SourceType = sourceType;
            ListParams = new List<object>();
            User = user;
        }

        public void Translate()
        {
            this.CommandText = TranslateGroup(Group);
        }

        public string TranslateGroup(FilterGroup group)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (group == null)
            {
                return "1=1";
            }
            var appendOp = false;
            strBuilder.Append(groupLeftToken);
            if (group.ListRule != null && group.ListRule.Count> 0)
            {
                foreach (var rule in group.ListRule)
                {
                    if (appendOp)
                    {
                        strBuilder.Append(GetFilterGroupOpQueryText(group.Op));
                    }
                    strBuilder.Append(TranslateRule(rule));
                    appendOp = true;
                }
            }
            if (group.ListGroup != null && group.ListGroup.Count > 0)
            {
                foreach (var groupChild in group.ListGroup)
                {
                    if (appendOp)
                    {
                        strBuilder.Append(GetFilterGroupOpQueryText(group.Op));
                    }
                    strBuilder.Append(TranslateGroup(groupChild));
                    appendOp = true;
                }
            }
            strBuilder.Append(groupRightToken);
            if (!appendOp)
            {
                return " 1=1 ";
            }

            return strBuilder.ToString();
        }

        private string GetFilterGroupOpQueryText(FilterGroupOp op)
        {
            switch (op)
            {
                case FilterGroupOp.And:
                    return " and ";
                case FilterGroupOp.Or:
                    return " or ";
                default:
                    return " and ";
            }
        }

        private string TranslateRule(FilterRule rule)
        {
            var strBuilder = new StringBuilder();
            if (rule == null)
            {
                return " 1=1 ";
            }
            if (currentParamMath.Contains(rule.Field))
            {
                strBuilder.Append(GetCurrentRule(rule));
                return strBuilder.ToString();
            }

            switch (rule.Op)
            {
                case FilterRuleOp.Equal:
                case FilterRuleOp.Greater:
                case FilterRuleOp.GreaterOrEqual:
                case FilterRuleOp.Less:
                case FilterRuleOp.LessOrEqual:
                case FilterRuleOp.NotEqual:
                    strBuilder.Append(rule.Field);
                    strBuilder.Append(GetFilterRuleOpQueryText(rule.Op));
                    strBuilder.Append(GetParamToken());
                    AddParam(rule);
                    break;
                case FilterRuleOp.Like:
                    strBuilder.Append(rule.Field);
                    strBuilder.Append($".Contains({GetParamToken()})");
                    ListParams.Add(rule.Value);
                    break;
                case FilterRuleOp.StartWith:
                    strBuilder.Append(rule.Field);
                    strBuilder.Append($".StartsWith({GetParamToken()})");
                    ListParams.Add(rule.Value);
                    break;
                case FilterRuleOp.EndWith:
                    strBuilder.Append(rule.Field);
                    strBuilder.Append($".EndsWith({GetParamToken()})");
                    ListParams.Add(rule.Value);
                    break;
                case FilterRuleOp.In:
                    strBuilder.Append($"{GetParamToken()}.Contains(outerIt.{rule.Field})");
                    AddArrayParam(rule);
                    break;
                case FilterRuleOp.Notin:
                    strBuilder.Append($"!{GetParamToken()}.Contains(outerIt.{rule.Field})");
                    AddArrayParam(rule);
                    break;
                default:
                    break;
            }
            return strBuilder.ToString();
        }

        private string GetCurrentRule(FilterRule rule)
        {
            // true return 1=1
            //false return 1 != 1
            //当前用户的判断：等于 不等于 包含于 不包含与
            var listSource = new List<long>();
            var filterCurr = (FilterCurrent)Enum.Parse(typeof(FilterCurrent), rule.Field);
            switch (filterCurr)
            {
                case FilterCurrent.CurrentUserId:
                    listSource.Add(User.UserInfo.UserId);
                    break;
                case FilterCurrent.CurrentRoleId:
                    listSource = User.UserInfo.RoleIdList;
                    break;
                case FilterCurrent.CurrentDeptId:
                    listSource = User.UserInfo.SysApartIdList;
                    break;
                default:
                    break;
            }
            var listTarget = StringUtility.ConvertToBigIntArray(rule.Value, ',');

            var result = false;
            switch (rule.Op)
            {
                case FilterRuleOp.Equal:
                    result = listSource.Contains(listTarget[0]);
                    break;
                case FilterRuleOp.NotEqual:
                    result = !listSource.Contains(listTarget[0]);
                    break;
                case FilterRuleOp.In:
                    result = listSource.Intersect(listTarget).Any();
                    break;
                case FilterRuleOp.Notin:
                    result = !listSource.Intersect(listTarget).Any();
                    break;
                default:
                    break;
            }
            return result ? " 1=1 " : " 1<>1 ";
        }

        private void AddParam(FilterRule rule)
        {
            //如果是当前用户
            if (rule.Value.Equals(FilterCurrent.CurrentUserId.ToString()))
            {
                ListParams.Add(User.UserInfo.UserId);
                return;
            }
            var propertyInfo = SourceType.GetProperty(rule.Field);
            if (propertyInfo == null)
            {
                //默认是字符串类型
                ListParams.Add(rule.Value);
            }
            if (!propertyInfo.PropertyType.IsEnum)
            {
                var value = Convert.ChangeType(rule.Value, propertyInfo.PropertyType);
                ListParams.Add(value);
                return;
            }
            var eValue = Enum.Parse(propertyInfo.PropertyType, rule.Value);
            ListParams.Add(eValue);
        }

        private void AddArrayParam(FilterRule rule)
        {
            var propertyInfo = SourceType.GetProperty(rule.Field);
            if (propertyInfo == null)
            {
                //默认是字符串类型
                ListParams.Add(rule.Value);
            }
            var valueArr = rule.Value.Split(',');
            var arr = Array.CreateInstance(propertyInfo.PropertyType, valueArr.Length);
            if (propertyInfo.PropertyType.IsEnum)
            {
                for (int i = 0; i < valueArr.Length; i++)
                {
                    var enumValue = (Enum)Enum.Parse(propertyInfo.PropertyType, valueArr[i]);
                    arr.SetValue(enumValue, i);
                }
            }else
            {
                for (int i = 0; i < valueArr.Length; i++)
                {
                    var objValue = Convert.ChangeType(valueArr[i], propertyInfo.PropertyType);
                    arr.SetValue(objValue, i);
                }
            }

            ListParams.Add(arr);
        }

        private string GetParamToken()
        {
            return string.Format("{0}{1}", paramPrefixToken , paramCounter++);
        }

        private string GetFilterRuleOpQueryText(FilterRuleOp op)
        {
            switch (op)
            {
                case FilterRuleOp.Equal:
                    return " = ";
                case FilterRuleOp.Greater:
                    return " > ";
                case FilterRuleOp.GreaterOrEqual:
                    return " >= ";
                case FilterRuleOp.Less:
                    return " < ";
                case FilterRuleOp.LessOrEqual:
                    return " <= ";
                case FilterRuleOp.Like:
                case FilterRuleOp.StartWith:
                case FilterRuleOp.EndWith:
                case FilterRuleOp.NotEqual:
                    return " <> ";
                case FilterRuleOp.In:
                    return " in ";
                case FilterRuleOp.Notin:
                    return " not in";
                default:
                    return " = ";
            }
        }

    }

}
