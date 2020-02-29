using Future.Model.Enum.Bingo;
using Future.Model.Enum.Sys;

namespace Future.Model.DTO.Today
{
    /// <summary>
    /// 模拟用户
    /// </summary>
    public class SimulateUserDTO
    {
        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户的性别
        /// </summary>
        public string GenderDesc { get; set; }

        /// <summary>
        /// 用户类别
        /// </summary>
        public SchoolTypeEnum SchoolType { get; set; }

        /// <summary>
        /// 学校类别
        /// </summary>
        public string SchoolTypeDesc { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string LiveStateDesc { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public LiveStateEnum LiveState { get; set; }

        /// <summary>
        /// 入学日期（2017-09）
        /// </summary>
        public string EntranceDate { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 生日（2018-08-20）
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 用户所在区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadPhotoPath { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }

        public string CreateTimeDesc { get; set; }
    }
}
