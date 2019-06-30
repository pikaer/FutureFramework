using System.ComponentModel;

namespace Future.Model.Enum.Letter
{
    public enum UserTypeEnum
    {
        [Description("正常注册用户/默认")]
        Default = 0,

        [Description("客服")]
        ServiceUser = 1,

        [Description("模拟用户")]
        SimulationUser = 2
    }
}
