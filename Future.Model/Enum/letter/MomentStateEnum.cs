using System.ComponentModel;

namespace Future.Model.Enum.letter
{
    /// <summary>
    /// 动态状态
    /// </summary>
    public enum MomentStateEnum
    {
        [Description("正常状态")]
        Default = 0,
        [Description("主人已删除")]
        IsDelete = 1,
        [Description("被举报")]
        IsReport = 2
    }
}
