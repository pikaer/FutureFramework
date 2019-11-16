using System;

namespace Future.Model.Entity.Hubs
{
    public class OnlineNotifyEntity:BaseEntity
    {
        public Guid OnlineNotifyId { get; set; }

        /// <summary>
        /// 订阅者UId
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 对方UId
        /// </summary>
        public long PartnerUId { get; set; }

    }
}
