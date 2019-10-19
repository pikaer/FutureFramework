using System;

namespace Future.Model.Entity.Hubs
{
    public class OnChatHubEntity : BaseEntity
    {
        public Guid OnChatHubId { get; set; }

        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 正在与谁聊天
        /// </summary>
        public long PartnerUId { get; set; }

        /// <summary>
        /// 网络连接标识
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 是否在线状态
        /// </summary>
        public bool IsOnLine { get; set; }

        /// <summary>
        /// 最近在线时间（离线时的时间）
        /// </summary>
        public DateTime? LastOnLineTime { get; set; }
    }
}
