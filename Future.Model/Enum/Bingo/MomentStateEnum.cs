using System.ComponentModel;

namespace Future.Model.Enum.Bingo
{
    /// <summary>
    /// 动态目前生效状态
    /// </summary>
    public enum MomentStateEnum
    {
        [Description("所有")]
        All = 0,
        
        [Description("有互动的")]
        HasDiscuss = 1,
        
        [Description("已生效的")]
        CanUse = 2,

        [Description("未生效的")]
        CanNotUse = 3,

        [Description("从未被捡的")]
        CanNotPickUp = 4,
    }
}
