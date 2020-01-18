using System;

namespace Future.Model.Entity.Hubs
{
    /// <summary>
    /// 用户在线状态
    /// </summary>
    public class OnLineUserHubEntity: BaseEntity
    {
        /// <summary>
        /// 用户在线Id
        /// </summary>
        public Guid OnLineId { get; set; }

        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 网络连接标识
        /// </summary>
        public string ConnectionId { get; set; }

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
        /// 最近浏览关注好友的动态时间
        /// </summary>
        public DateTime? LastScanMomentTime { get; set; }
    }
}
