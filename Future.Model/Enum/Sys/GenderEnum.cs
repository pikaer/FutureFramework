using System.ComponentModel;

namespace Future.Model.Enum.Sys
{
    public enum GenderEnum
    {
        [Description("未知")]
        Default = 0,
        [Description("男性")]
        Man = 1,
        [Description("女性")]
        Woman=2,
        [Description("全部")]
        All = 3
    }
}
