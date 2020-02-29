using Future.Model.Enum.Bingo;
using Future.Model.Enum.Sys;
using System;

namespace Future.Model.Entity.Bingo
{
    public class UserInfoEntity:BaseEntity
    {
        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 小程序端-用户唯一标示
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 用户注册渠道
        /// </summary>
        public PlatformEnum Platform { get; set; }

        /// <summary>
        /// 用户类别
        /// </summary>
        public UserTypeEnum UserType { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知,此时一般是用户未授权
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户类别
        /// </summary>
        public SchoolTypeEnum SchoolType { get; set; }

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
        public DateTime? BirthDate { get; set; }

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
        /// 用户所在国家
        /// </summary>
        public string Country { get; set; }        

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string WeChatNo { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadPhotoPath { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }

        public bool IsDelete { get; set; }

        /// <summary>
        /// 是否已经注册账户
        /// </summary>
        public bool IsRegister { get; set; }

        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
    }
}
