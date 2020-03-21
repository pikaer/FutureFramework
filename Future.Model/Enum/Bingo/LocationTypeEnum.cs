using System.ComponentModel;

namespace Future.Model.Enum.Bingo
{
    public enum LocationTypeEnum
    {
        [Description("地区不限")]
        Default = 0,

        [Description("接受同省")]
        RecieveProvince = 1,

        [Description("只要同城")]
        OnlyCommonCity = 2
    }
}
