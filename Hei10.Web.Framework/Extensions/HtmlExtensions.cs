using Hei10.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Hei10.Web.Framework.Extensions
{
    public static class HtmlExtensions
    {
        #region RadioButtonList

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<string> items, string selectedValue)
        {
            var selectList = new SelectList(items);
            return helper.RadioButtonList(name, selectList, selectedValue, "", "");
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            return helper.RadioButtonList(name, items, null, "", "");
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, Enum enumValue)
        {
            return helper.RadioButtonList(name, items, enumValue.ToString(), "", "");
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, string selectedValue)
        {
            return helper.RadioButtonList(name, items, selectedValue, "", "");
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, string selectedValue, string optionText, string optionValue)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(optionText))
            {
                string rbValue = optionValue ?? optionText;
                string rbName = name + "_" + rbValue;

                var radioTag = helper.RadioButton(name, rbValue, string.IsNullOrEmpty(selectedValue), new { id = rbName, @data_toggle = "icheck", @data_rule = "checked", @data_label = optionText });
                
                sb.Append(radioTag).Append("&nbsp");
            }
            foreach (var item in items)
            {
                var rbValue = item.Value ?? item.Text;
                var rbName = name + "_" + rbValue;
                var isCheck = (null != item.Value && (item.Selected || item.Value == selectedValue));
                var radioTag = helper.RadioButton(name, rbValue, isCheck, new { id = rbName, @data_toggle = "icheck", @data_rule = "checked", @data_label = item.Text });
                sb.Append(radioTag).Append("&nbsp");
            }
            return MvcHtmlString.Create(sb.ToString());
        }


        #endregion

        #region 计算文件大小函数(保留两位小数)
        public static string CountSize(this long size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }
        #endregion

        #region advert

        public static void ShowAdvert(this HtmlHelper helper, AdPosKey key, AdvertType? type = null, int takeSize = 0)
        {
            helper.RenderAction("AdPosDisplay", "Advert", new { key, type, takeSize });
        }
        #endregion
    }
}
