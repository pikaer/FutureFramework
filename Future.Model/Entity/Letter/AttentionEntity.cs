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
        /// 通过那个动态Id关注的此好友
        /// </summary>
        public Guid AttentionMomentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 被关注好友的UId
        /// </summary>
        public long PartnerUId { get; set; }

        /// <summary>
        /// 开始订阅时间
        /// </summary>
        public DateTime StartDateTime { get; set; }
    }
}
