using System.ComponentModel;

namespace Future.Model.Enum.Letter
{
    /// <summary>
    /// 动态获取来源
    /// </summary>
    public enum PickUpFromPageEnum
    {
        [Description("默认捡起的")]
        Default = 0,

        [Description("关注页面")]
        AttentionPage =1
    }
}
