using System.ComponentModel;

namespace Future.Model.Enum.Letter
{
    /// <summary>
    /// 捡起的动态类别
    /// </summary>
    public enum PickUpTypeEnum
    {
        [Description("所有")]
        All = 0,

        [Description("没互动的")]
        NoDiscuss = 1,
        
        [Description("有互动的")]
        HasDiscuss = 2,

        [Description("已删除的")]
        IsDelete = 3,

        [Description("未删除的")]
        NoDelete = 4
    }
}
