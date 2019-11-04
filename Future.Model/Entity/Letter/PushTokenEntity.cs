using System;

namespace Future.Model.Entity.Letter
{
    public class PushTokenEntity: BaseEntity
    {
        
        /// <summary>
        /// 推送Id
        /// </summary>
        public Guid PushTokenId { get; set; }

        public long UId { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string PushToken { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string FromPage { get; set; }
    }
}
