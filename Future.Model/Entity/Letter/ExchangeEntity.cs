using System;

namespace Future.Model.Entity.Letter
{
    public class ExchangeEntity: BaseEntity
    {
        /// <summary>
        /// 互动列表
        /// </summary>
        public Guid ExchangeId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 参与评论的用户
        /// </summary>
        public long ExchangeUId { get; set; }
    }
}
