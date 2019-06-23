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

        /// <summary>
        /// 当前未读总条数
        /// </summary>
        public string CurrentTotalUnReadCount { get; set; }
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
        public long PartnerUId { get; set; }

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
        ///用于排序的时间
        /// </summary>
        public DateTime SortChatTime { get; set; }

        /// <summary>
        /// 最近一次交谈时间
        /// </summary>
        public string RecentChatTime { get; set; }

        /// <summary>
        /// 未读数量
        /// </summary>
        public string UnReadCount { get; set; }
    }
    #endregion

    #region DiscussList（动态评论）
    public class DiscussDetailRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid PickUpId { get; set; }
        
    }

    public class DiscussDetailResponse
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 动态发布用户Id
        /// </summary>
        public long MomentUId { get; set; }

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
        /// 图片内容
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 创建时间（动态创建时间，而非捡起时间）
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 互动列表
        /// </summary>
        public List<DiscussDetailType> DiscussDetailList { get; set; }
    }

    public class DiscussDetailType
    {
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

    #region PickUp
    public class PickUpRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class PickUpResponse
    {
        /// <summary>
        /// 互动列表
        /// </summary>
        public List<PickUpType> PickUpList { get; set; }
    }
    
    #endregion

    #region PickUpList
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
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

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

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 微信头像地址
        /// </summary>
        public string AvatarUrl { get; set; }
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

    #region GetOpenId
    public class GetOpenIdRequest
    {
        /// <summary>
        /// 用来获取小程序OpenId
        /// </summary>
        public string LoginCode { get; set; }
    }

    public class GetOpenIdResponse
    {
        /// <summary>
        /// 小程序端-用户唯一标示
        /// </summary>
        public string OpenId { get; set; }

        public string Session_key { get; set; }
    }
    #endregion

    #region Discuss
    public class DiscussRequest
    {
        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

    }

    public class DiscussResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region DeleteBottle
    public class DeleteBottleRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

    }

    public class DeleteBottleResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }

        /// <summary>
        /// 当前未读总条数
        /// </summary>
        public string CurrentTotalUnReadCount { get; set; }
    }
    #endregion

    #region ReportBottle
    public class ReportBottleRequest
    {
        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

    }

    public class ReportBottleResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region ClearAllBottle
    public class ClearAllBottleRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

    }

    public class ClearAllBottleResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region ClearUnReadCount(清除未读数量）
    public class ClearUnReadCountRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }
    }

    public class ClearUnReadCountResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }

        /// <summary>
        /// 当前未读总条数
        /// </summary>
        public string CurrentTotalUnReadCount { get; set; }
    }
    #endregion

    #region DeleteAllBottle(删除瓶子列表）
    public class DeleteAllBottleRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }
    }

    public class DeleteAllBottleResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }

        /// <summary>
        /// 当前未读总条数
        /// </summary>
        public string CurrentTotalUnReadCount { get; set; }
    }
    #endregion

    #region UnReadTotalCount(未读消息总数量）
    public class UnReadTotalCountRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class UnReadTotalCountResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public string UnReadCount { get; set; }
    }
    #endregion
}
