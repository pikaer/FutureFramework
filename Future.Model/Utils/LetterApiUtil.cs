using System;
using System.Collections.Generic;

namespace Future.Model.Utils
{
    #region ExchangeList（互动列表）
    public class PickUpListRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 页码（分页传递数据）
        /// </summary>
        public int PageIndex { get; set; }
    }

    public class PickUpListResponse
    {
        /// <summary>
        /// 互动列表
        /// </summary>
        public List<PickUpListType> ExchangeList { get; set; }
    }

    public class PickUpListType
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid PickUpId { get; set; }

        /// <summary>
        /// 动态发布者Id
        /// </summary>
        public long MomentUId { get; set; }

        /// <summary>
        /// 参与评论的用户
        /// </summary>
        public long PickUpUId { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadImgPath { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        
        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 图片内容（图片最多支持一张）
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 最近一次交谈时间
        /// </summary>
        public string RecentChatTime { get; set; }
    }
    #endregion
}
