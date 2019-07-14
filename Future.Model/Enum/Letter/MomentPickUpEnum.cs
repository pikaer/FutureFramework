using System.ComponentModel;

namespace Future.Model.Enum.Letter
{
    /// <summary>
    /// 动态目前生效状态
    /// </summary>
    public enum MomentPickUpEnum
    {
        [Description("所有")]
        All = 0,

        [Description("无互动的")]
        NoDiscuss = 1,

        [Description("有互动的")]
        HasDiscuss = 2,

        [Description("对方已删除的")]
        IsDelete = 3,

        [Description("对方未删除的")]
        NoDelete = 4
    }
}
