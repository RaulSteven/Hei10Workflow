using System.ComponentModel;

namespace Hei10.Domain.Enums
{
    public enum AdvertType
    {
        [Description("图片")]
        Img = 1,
        [Description("文字")]
        Text = 2,
        [Description("富文本框")]
        Html = 3
    }
}