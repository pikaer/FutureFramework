using System.ComponentModel;

namespace Future.Model.Enum.Letter
{
    /// <summary>
    /// 一起玩类别枚举
    /// </summary>
    public enum PlayTypeEnum
    {
        [Description("其它")]
        Other =0,

        [Description("王者")]
        WangZhe =1,

        [Description("吃鸡")]
        ChiJi = 2,

        [Description("连麦")]
        LianMai,

        [Description("游戏")]
        Game,

        [Description("学习")]
        Learn,

        [Description("追剧")]
        TVTracker,

        [Description("早起")]
        Earlybird,

        [Description("散步")]
        Walk,

        [Description("看电影")]
        Movie
    }
}
