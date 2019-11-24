using System;

namespace Future.Model.Entity.Letter
{
    /// <summary>
    /// 关注用户列表
    /// </summary>
    public class AttentionEntity : BaseEntity
    {
        /// <summary>
        /// 关注映射Id
        /// </summary>
        public Guid AttentionId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 被关注好友的UId
        /// </summary>
        public long PartnerUId { get; set; }
    }
}
