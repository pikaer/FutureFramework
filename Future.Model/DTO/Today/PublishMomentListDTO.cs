﻿using System;

namespace Future.Model.DTO.Today
{
    public class PublishMomentListDTO
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
        
        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 图片内容（图片最多支持一张）
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 主人是否已删除
        /// </summary>
        public bool IsDelete { get; set; }
        
        /// <summary>
        /// 被捡起次数
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool CanEdit { get; set; }
    }
}
