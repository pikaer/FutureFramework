using System;
using System.Collections.Generic;

namespace Future.Model.Utils
{
    #region PickUpList（互动列表）
    public class DiscussListRequest
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

    public class DiscussListResponse
    {
        /// <summary>
        /// 互动列表
        /// </summary>
        public List<DiscussType> DiscussList { get; set; }
    }

    public class DiscussType
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
        /// 最近一次交谈时间
        /// </summary>
        public string RecentChatTime { get; set; }
    }
    #endregion

    #region DiscussList（动态评论）
    public class DiscussDetailListRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid PickUpId { get; set; }
        
    }

    public class DiscussDetailListResponse
    {
        /// <summary>
        /// 互动列表
        /// </summary>
        public List<DiscussDetailType> DiscussDetailList { get; set; }
    }

    public class DiscussDetailType
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid PickUpId { get; set; }
        
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
        /// 最近一次交谈时间
        /// </summary>
        public string RecentChatTime { get; set; }
    }
    #endregion

    #region MomentList（动态评论）
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
        public List<PickUpType> PickUpList { get; set; }
    }

    public class PickUpType
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

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
        /// 创建时间
        /// </summary>
        public string CreateTime{ get; set; }
    }
    #endregion

    #region UpLoadImg
    public class UpLoadImgResponse
    {
        /// <summary>
        /// 文件存储名
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// 文件原图大小
        /// </summary>
        public long ImgLength { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string ImgMime { get; set; }
    }
    #endregion

    #region DeleteImg
    public class DeleteImgRequest
    {
        public string ImgPath { get; set; }
    }

    public class DeleteImgResponse
    {
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion
    #region PublishMoment
    /// <summary>
    /// 发布动态
    /// </summary>
    public class PublishMomentRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 图片内容
        /// </summary>
        public string ImgContent { get; set; }
    }

    public class PublishMomentResponse
    {
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region SetUserInfo
    public class SetUserInfoRequest
    {
        /// <summary>
        /// 小程序端-用户唯一标示
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
    }

    public class SetUserInfoResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }
    #endregion
}
