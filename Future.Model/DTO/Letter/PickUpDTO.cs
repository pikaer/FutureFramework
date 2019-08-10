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

        /// <summary>
        /// 参与评论的用户
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像路径
        /// </summary>
        public string HeadPhotoPath { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
