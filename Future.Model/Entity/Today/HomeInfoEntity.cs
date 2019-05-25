using System;

namespace Future.Model.Entity.Today
{
    public class HomeInfoEntity
    {
        /// <summary>
        /// 唯一标示Id
        /// </summary>
        public long HomeInfoId { get; set; }

        /// <summary>
        /// 展示日期
        /// </summary>
        public DateTime DisplayDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 最近修改人
        /// </summary>
        public long ModifyUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
