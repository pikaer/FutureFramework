using System;

namespace Future.Model.Entity
{
    /// <summary>
    /// 抽象实体类
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public int? ModifyUserId { get; set; }
    }
}
