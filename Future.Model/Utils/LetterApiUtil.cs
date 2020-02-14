using Future.Model.Enum.Letter;
using Future.Model.Enum.Sys;
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

        public long UId { get; set; }

        public Guid MomentId { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadImgPath { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 最近一次交谈时间
        /// </summary>
        public string RecentChatTime { get; set; }

        /// <summary>
        /// 在线状态描述
        /// </summary>
        public string OnLineDesc { get; set; }

        /// <summary>
        /// 距离描述
        /// </summary>
        public string DistanceDesc { get; set; }

        /// <summary>
        /// 未读数量
        /// </summary>
        public string UnReadCount { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 短昵称
        /// </summary>
        public string ShortNickName { get; set; }
    }
    #endregion

    #region MyMomentList（我扔出去的没有被评论的动态）
    public class MyMomentListRequest
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

    public class MyMomentListResponse
    {
        /// <summary>
        /// 互动列表
        /// </summary>
        public List<MomentType> MomentList { get; set; }
    }

    public class MomentType
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
        /// 图片内容
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 匿名时候对应的昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 短昵称
        /// </summary>
        public string ShortNickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadImgPath { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知,此时一般是用户未授权
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 最近一次交谈时间
        /// </summary>
        public string PublishTime { get; set; }
    }
    #endregion

    #region MomentDetail（动态详情）
    public class MomentDetailRequest
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
    }

    public class MomentDetailResponse
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 动态发布用户Id
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
        /// 图片内容
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 最近一次交谈时间
        /// </summary>
        public string CreateTime { get; set; }
    }
   
    #endregion

    #region DiscussList（动态评论）
    public class DiscussDetailRequest
    {
        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid? PickUpId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid? MomentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class DiscussDetailResponse
    {
        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

        public MomentDetailType MomentDetail { get; set; }
        /// <summary>
        /// 参与评论的另一半信息
        /// </summary>
        public PartnerDetailType PartnerDetail { get; set; }

        /// <summary>
        /// 自己的详情
        /// </summary>
        public MyDetailType MyDetail { get; set; }
    }

    public class MomentDetailType
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
        /// 是自己发布的动态
        /// </summary>
        public bool IsMyMoment { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

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

    }
    public class PartnerDetailType
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long PartnerUId { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadImgPath { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 短昵称
        /// </summary>
        public string ShortNickName { get; set; }

        /// <summary>
        /// 距离描述
        /// </summary>
        public string DistanceDesc { get; set; }

        /// <summary>
        /// 在线状态描述
        /// </summary>
        public string OnLineDesc { get; set; }
    }

    public class MyDetailType
    {
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadImgPath { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }


        /// <summary>
        /// 分身状态（0：不展示，1：展示切换为匿名 2：展示切换为真身）
        /// </summary>
        public int ShowHideStatus { get; set; }

        /// <summary>
        /// 短昵称
        /// </summary>
        public string ShortNickName { get; set; }
    }

    public class DiscussDetailType
    {
        /// <summary>
        /// 是否是自己的评论
        /// </summary>
        public bool IsMyReply { get; set; }

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
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 短昵称
        /// </summary>
        public string ShortNickName { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 图片内容
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 距离描述
        /// </summary>
        public string DistanceDesc { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string RecentChatTime { get; set; }

        /// <summary>
        /// 时间是否展示
        /// </summary>
        public bool IsTimeVisible { get; set; }
    }
    #endregion

    #region ChatDetailList
    public class ChatDetailListRequest
    {
        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid? PickUpId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid? MomentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class ChatDetailListResponse
    {
        /// <summary>
        /// 互动列表
        /// </summary>
        public List<DiscussDetailType> DiscussDetailList { get; set; }
    }

    #endregion

    #region PickUp
    public class PickUpRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 动态类别
        /// </summary>
        public MomentTypeEnum MomentType { get; set; }
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
        /// 自己发布的动态
        /// </summary>
        public bool IsMyMoment { get; set; }

        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

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
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 在线状态描述
        /// </summary>
        public string OnLineDesc { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 距离描述
        /// </summary>
        public string DistanceDesc { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
    #endregion

    #region PlayTogetherList
    public class PlayTogetherListRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        public PlayTypeEnum PlayType { get; set; }
    }

    public class PlayTogetherListResponse
    {
        /// <summary>
        /// 一起玩
        /// </summary>
        public List<PlayTogetherType> PlayTogetherList { get; set; }
    }

    public class PlayTogetherType
    {
        /// <summary>
        /// 自己发布的动态
        /// </summary>
        public bool IsMyMoment { get; set; }

        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

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
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 在线状态描述
        /// </summary>
        public string OnLineDesc { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 距离描述
        /// </summary>
        public string DistanceDesc { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        public PlayTypeEnum PlayType { get; set; }
    }

    #endregion


    #region AttentionList
    public class AttentionListRequest
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

    public class AttentionListResponse
    {
        /// <summary>
        /// 互动列表
        /// </summary>
        public List<PickUpType> AttentionList { get; set; }
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

        /// <summary>
        /// 动态来源
        /// </summary>
        public MomentSourceEnum SourceFlag { get; set; }

        public PlayTypeEnum PlayType { get; set; }

        public bool SubscribeMessageOpen { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 匿名时候对应的昵称
        /// </summary>
        public string HidingNickName { get; set; }
    }

    public class PublishMomentResponse
    {
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsExecuteSuccess { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
    }
    #endregion

    #region SetUserInfo
    public class SetUserInfoRequest
    {
        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 微信头像地址
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
    }

    public class SetUserInfoResponse
    {
        /// <summary>
        /// 用户金币数
        /// </summary>
        public int TotalCoin { get; set; }
    }
    #endregion

    #region UserLogin
    public class UserLoginRequest
    {
        /// <summary>
        /// 用来获取小程序OpenId
        /// </summary>
        public string LoginCode { get; set; }
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
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

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

        /// <summary>
        /// 删除类别：0=默认只修改状态，1=删除所有聊天内容
        /// </summary>
        public int DeleteType { get; set; }
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

    #region ClearAllUnReadCount(清除所有未读数量）
    public class ClearAllUnReadCountRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class ClearAllUnReadCountResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region BasicUserInfo(用户基础信息）
    public class BasicUserInfoRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        ///0： 默认只获取用户信息，1：获取金币信息
        /// </summary>
        public int Type { get; set; }
    }

    public class BasicUserInfoResponse
    {
        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadPhotoPath { get; set; }

        /// <summary>
        /// 基础信息
        /// </summary>
        public string BasicUserInfo { get; set; }

        /// <summary>
        /// 位置信息
        /// </summary>
        public string PlaceInfo { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 星座
        /// </summary>
        public string Constellation { get; set; }

        /// <summary>
        /// 如 90后、00后
        /// </summary>
        public string AgeYear { get; set; }

        /// <summary>
        /// 是否已经注册账户
        /// </summary>
        public bool IsRegister { get; set; }

        /// <summary>
        /// 账户金币余额
        /// </summary>
        public int TotalCoin { get; set; }
    }
    #endregion

    #region GetUserInfo(用户信息）
    public class GetUserInfoRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class GetUserInfoResponse
    {
        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户类别
        /// </summary>
        public SchoolTypeEnum SchoolType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public LiveStateEnum LiveState { get; set; }

        /// <summary>
        /// 入学日期（2017-09）
        /// </summary>
        public string EntranceDate { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 生日（2018-08-20）
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 用户所在区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }
    }
    #endregion

    #region UpdateUserInfo(更新用户信息）
    public class UpdateUserInfoRequest
    {
        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户类别
        /// </summary>
        public SchoolTypeEnum SchoolType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public LiveStateEnum LiveState { get; set; }

        /// <summary>
        /// 入学日期（2017-09）
        /// </summary>
        public string EntranceDate { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 生日（2018-08-20）
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 用户所在区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }
    }

    public class UpdateUserInfoResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }

    #endregion

    #region DeleteMoment
    public class DeleteMomentRequest
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
    }

    public class DeleteMomentResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region CancelAttention
    public class CancelAttentionRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 被关注好友的UId
        /// </summary>
        public long PartnerUId { get; set; }
    }

    public class CancelAttentionResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region AddAttention
    public class AddAttentionRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 被关注好友的UId
        /// </summary>
        public long PartnerUId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
    }

    public class AddAttentionResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region DeleteAllMoment
    public class DeleteAllMomentRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class DeleteAllMomentResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region UpdateAvatarUrl
    public class UpdateAvatarUrlRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string AvatarUrl { get; set; }
    }

    public class UpdateAvatarUrlResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region GetCollectList
    public class GetCollectListRequest
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

    public class GetCollectListResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public List<CollectType> CollectList{ get; set; }
    }

    public class CollectType
    {
        /// <summary>
        /// 收藏动态Id
        /// </summary>
        public Guid CollectId { get; set; }
        
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadImgPath { get; set; }

        /// <summary>
        /// 短昵称
        /// </summary>
        public string ShortNickName { get; set; }

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
        public string CreateTime { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 在线状态描述
        /// </summary>
        public string OnLineDesc { get; set; }

        /// <summary>
        /// 距离描述
        /// </summary>
        public string DistanceDesc { get; set; }
    }
    #endregion

    #region AddCollect
    public class AddCollectRequest
    {
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
        /// 收藏来源
        /// </summary>
        public string FromPage { get; set; }
    }

    public class AddCollectResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region DeleteCollect
    public class DeleteCollectRequest
    {
        /// <summary>
        /// 收藏动态Id
        /// </summary>
        public Guid CollectId { get; set; }
    }

    public class DeleteCollectResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region DeleteAllCollect
    public class DeleteAllCollectRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class DeleteAllCollectResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool IsExecuteSuccess { get; set; }
    }
    #endregion

    #region UserCoinInfo
    public class UserCoinInfoRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class UserCoinInfoResponse
    {
        /// <summary>
        /// 总金币数
        /// </summary>
        public int TotalCoin { get; set; }
    }
    #endregion

    #region CoinDetail
    public class CoinDetailRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class CoinDetailResponse
    {
        /// <summary>
        /// 收入明细
        /// </summary>
        public List<CoinDetailType> IncomeDetailList { get; set; }

        /// <summary>
        /// 支出明细
        /// </summary>
        public List<CoinDetailType> ExpendDetailList { get; set; }
    }

    public class CoinDetailType
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 金币改变值
        /// </summary>
        public string ChangeValue { get; set; }
    }
    #endregion

    #region MsgSecCheck
    public class MsgSecCheckRequest
    {
        /// <summary>
        /// 待检测文本内容
        /// </summary>
        public string TextContent { get; set; }
    }

    public class MsgSecCheckResponse
    {
        /// <summary>
        /// 是否校验通过
        /// </summary>
        public bool IsOK { get; set; }
    }
    #endregion

    #region CollectPushToken
    public class CollectPushTokenRequest
    {
        public long UId { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string PushToken { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string FromPage { get; set; }
    }

    public class CollectPushTokenResponse
    {
        /// <summary>
        /// 是否收藏成功
        /// </summary>
        public bool Success { get; set; }
    }
    #endregion

    #region ForwardMoment
    public class ForwardMomentRequest
    {
        public long UId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
    }

    public class ForwardMomentResponse
    {
        /// <summary>
        /// 是否收藏成功
        /// </summary>
        public bool Success { get; set; }
    }
    #endregion

    #region OnlineNotify
    public class OnlineNotifyRequest
    {
        /// <summary>
        /// 订阅者UId
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 对方UId
        /// </summary>
        public long PartnerUId { get; set; }
    }

    public class OnlineNotifyResponse
    {
        /// <summary>
        /// 是否收藏成功
        /// </summary>
        public bool Success { get; set; }
    }
    #endregion

    #region OnlineNotify
    public class UpdateUserLocationRequest
    {
        /// <summary>
        /// 订阅者UId
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 纬度，范围为 -90~90，负数表示南纬
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度，范围为 -180~180，负数表示西经
        /// </summary>
        public double Longitude { get; set; }
    }

    public class UpdateUserLocationResponse
    {
        /// <summary>
        /// 是否收藏成功
        /// </summary>
        public bool Success { get; set; }
    }
    #endregion

    #region AttentionMomentCount
    public class AttentionMomentCountRequest
    {
        /// <summary>
        /// 订阅者UId
        /// </summary>
        public long UId { get; set; }
    }

    public class AttentionMomentCountResponse
    {
        /// <summary>
        /// 未读数量
        /// </summary>
        public string UnReadCountStr { get; set; }
    }
    #endregion

    #region UpdateLastScanMomentTime
    public class UpdateLastScanMomentTimeRequest
    {
        /// <summary>
        /// 订阅者UId
        /// </summary>
        public long UId { get; set; }
    }

    public class UpdateLastScanMomentTimeResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
    }
    #endregion

    #region UpdateHiding
    public class UpdateHidingRequest
    {
        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }


        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 匿名时候对应的昵称
        /// </summary>
        public string HidingNickName { get; set; }
    }

    public class UpdateHidingResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool Success { get; set; }
    }
    #endregion

}
