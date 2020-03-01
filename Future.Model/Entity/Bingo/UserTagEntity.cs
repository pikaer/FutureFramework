using Future.Model.Enum.Bingo;
using System;

namespace Future.Model.Entity.Bingo
{
    /// <summary>
    /// 用户标签
    /// </summary>
    public class UserTagEntity : BaseEntity
    {
        public Guid TagId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 标签枚举
        /// </summary>
        public TagTypeEnum TagType { get; set; }

        /// <summary>
        /// 标签内容
        /// </summary>
        public string Tag { get; set; }
    }
}
