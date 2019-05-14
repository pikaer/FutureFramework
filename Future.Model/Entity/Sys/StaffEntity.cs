using Future.Model.Enum.Sys;
using System;

namespace Future.Model.Entity.Sys
{
    /// <summary>
    /// 员工实体类
    /// </summary>
    public class StaffEntity
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public long StaffId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public RoleEnum Role { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 系统登录密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
