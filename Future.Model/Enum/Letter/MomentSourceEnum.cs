using System.ComponentModel;

namespace Future.Model.Enum.Letter
{
    /// <summary>
    /// 动态来源
    /// </summary>
    public enum MomentSourceEnum
    {
        [Description("闪聊动态")]
        Default = 0,

        [Description("一起玩动态")]
        PlayTogether =1,

        [Description("九宫格动态")]
        NineMoment = 2
    }
}
