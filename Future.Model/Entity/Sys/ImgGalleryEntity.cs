using System;

namespace Future.Model.Entity.Sys
{
    /// <summary>
    /// 图片库
    /// </summary>
    public class ImgGalleryEntity
    {
        /// <summary>
        /// 图片唯一标示
        /// </summary>
        public long ImgId { get; set; }

        /// <summary>
        /// 图片名
        /// </summary>
        public string ImgName { get; set; }

        /// <summary>
        /// 短链接
        /// </summary>
        public string ShortUrl { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 图片创建人
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 图片修改人
        /// </summary>
        public long ModifyUserId { get; set; }

        /// <summary>
        /// 使用次数
        /// </summary>
        public int UseCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 图片修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
