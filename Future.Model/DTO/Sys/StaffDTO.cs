namespace Future.Model.DTO.Sys
{
    public class StaffDTO
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
        public string Gender { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTimeDesc { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifyTimeDesc { get; set; }
    }
}
