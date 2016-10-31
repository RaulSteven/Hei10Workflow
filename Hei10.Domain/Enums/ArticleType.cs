using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum ArticleListType
    {
        [Description("标题+日期")]
        Text = 1,
        [Description("标题+图片+简介")]
        Image
    }

    public enum ArticleDetailType
    {
        [Description("类似关于我们")]
        Single = 1,
        [Description("显示标题、内容、来源、时间等")]
        More,
        [Description("只显示标题、内容等")]
        Simple
    }
}
