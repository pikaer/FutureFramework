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
        /// 展示开始时间
        /// </summary>
        public DateTime DisplayStartDateTime { get; set; }

        /// <summary>
        /// 展示结束时间
        /// </summary>
        public DateTime DisplayEndDateTime { get; set; }

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
