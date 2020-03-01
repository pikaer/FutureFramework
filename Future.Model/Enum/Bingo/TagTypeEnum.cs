using System.ComponentModel;

namespace Future.Model.Enum.Bingo
{
    public enum TagTypeEnum
    {
        [Description("个性标签")]
        个性标签 = 1,

        [Description("喜欢的运动")]
        运动标签 = 2,

        [Description("音乐爱好")]
        音乐标签,

        [Description("喜欢的食物")]
        食物标签,

        [Description("喜欢的电影")]
        电影标签,

        [Description("想去旅行的标签")]
        旅行标签,
    }
}
