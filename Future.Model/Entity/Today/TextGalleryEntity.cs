using System;

namespace Future.Model.Entity.Today
{
    /// <summary>
    /// 文本库
    /// </summary>
    public class TextGalleryEntity
    {
        /// <summary>
        /// 文本唯一标示
        /// </summary>
        public long TextId { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 文本来源
        /// </summary>
        public string TextSource { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 文本创建人
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 文本修改人
        /// </summary>
        public long ModifyUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
