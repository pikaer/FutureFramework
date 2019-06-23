using System;

namespace Future.Model.Entity.Letter
{
    public class DiscussEntity:BaseEntity
    {
        public Guid DiscussId { get; set; }

        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

        /// <summary>
        /// 评论用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string DiscussContent { get; set; }

        /// <summary>
        /// 对方是否已读
        /// </summary>
        public bool HasRead { get; set; }
    }
}
