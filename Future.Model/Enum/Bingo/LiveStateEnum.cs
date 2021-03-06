﻿using System.ComponentModel;

namespace Future.Model.Enum.Bingo
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum LiveStateEnum
    {
        [Description("未设置")]
        Default = 0,

        [Description("学生党")]
        Student = 1,

        [Description("工作党")]
        Working = 2
    }
}
