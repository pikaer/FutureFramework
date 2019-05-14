using System.ComponentModel;

namespace Future.Model.Enum.Sys
{
    public enum RoleEnum
    {
        [Description("游客")]
        Default =0,
        [Description("系统管理员")]
        Administrator = 1,
        [Description("普通成员")]
        Staff=2
    }
}
