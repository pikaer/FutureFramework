using System;

namespace Future.Model.DTO.Letter
{
    public class CollectDTO
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
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 图片内容（图片最多支持一张）
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
