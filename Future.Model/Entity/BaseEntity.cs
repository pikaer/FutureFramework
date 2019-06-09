using System;

namespace Future.Model
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最新修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
