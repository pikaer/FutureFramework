using System;

namespace Future.Model.Entity.Today
{
    public class HomeTextEntity
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public long HomeTextId { get; set; }

        /// <summary>
        /// 首页Id
        /// </summary>
        public long HomeInfoId { get; set; }

        /// <summary>
        /// 文本唯一标示
        /// </summary>
        public long TextId { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int SortNum { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
    }
}
