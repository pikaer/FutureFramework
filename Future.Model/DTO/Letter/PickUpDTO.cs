using Future.Model.Enum.Letter;
using Future.Model.Enum.Sys;
using System;

namespace Future.Model.DTO.Letter
{
    public class PickUpDTO
    {
        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

        public Guid MomentId { get; set; }

        /// <summary>
        /// 最近一条评论内容
        /// </summary>
        public string TextContent { get; set; }
        

        public string ImgContent { get; set; }

        public long MomentUId { get; set; }

        /// <summary>
        /// 参与评论的用户
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 匿名时候对应的昵称
        /// </summary>
        public string HidingNickName { get; set; }

        /// <summary>
        /// 用户头像路径
        /// </summary>
        public string HeadPhotoPath { get; set; }

        public GenderEnum Gender { get; set; }

        public PlayTypeEnum PlayType { get; set; }

        /// <summary>
        /// 是否在线状态
        /// </summary>
        public bool IsOnLine { get; set; }

        /// <summary>
        /// 纬度，范围为 -90~90，负数表示南纬
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度，范围为 -180~180，负数表示西经
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 最近在线时间（离线时的时间）
        /// </summary>
        public DateTime? LastOnLineTime { get; set; }

        /// <summary>
        /// 生日（2018-08-20）
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public DateTime MomentCreateTime { get; set; }

        public DateTime PickUpCreateTime { get; set; }
    }
}
