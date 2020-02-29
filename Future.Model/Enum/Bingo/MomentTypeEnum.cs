using System.ComponentModel;

namespace Future.Model.Enum.Bingo
{
    /// <summary>
    /// 动态类别
    /// </summary>
    public enum MomentTypeEnum
    {
        [Description("全部动态")]
        Default = 0,

        [Description("文本动态")]
        TextMoment = 1,

        [Description("图片动态")]
        ImgMoment = 2
    }
}
