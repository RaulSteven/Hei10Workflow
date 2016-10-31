﻿using System.ComponentModel;

namespace Hei10.Domain.Enums
{
    public enum Target
    {
        [Description("新窗口")]
        Blank = 0,
        [Description("原窗口")]
        Self = 1
    }
}