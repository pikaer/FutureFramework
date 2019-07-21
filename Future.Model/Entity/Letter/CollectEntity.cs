using System;

namespace Future.Model.Entity.Letter
{
    /// <summary>
    /// 用户收藏
    /// </summary>
    public class CollectEntity: BaseEntity
    {
        /// <summary>
        /// 收藏动态Id
        /// </summary>
        public Guid CollectId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

        /// <summary>
        /// 收藏来源
        /// </summary>
        public string FromPage { get; set; }
    }
}
