using System;

namespace Future.Model.Entity.Letter
{
    /// <summary>
    /// 用户发布的动态库
    /// </summary>
    public class MomentEntity:BaseEntity
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 图片内容（图片最多支持一张）
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 主人是否已删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 是否被举报
        /// </summary>
        public bool IsReport { get; set; }

        /// <summary>
        /// 被捡起次数
        /// </summary>
        public int ReplyCount { get; set; }

        public bool SubscribeMessageOpen { get; set; }
    }
}
