using Future.Model.Entity.Letter;
using Future.Model.Utils;
using System;

namespace Future.Service.Interface
{
    /// <summary>
    /// 漂流瓶小程序接口中心
    /// </summary>
    public interface IBottleBiz
    {
        /// <summary>
        /// 捡到的回复过的瓶子列表
        /// </summary>
        ResponseContext<DiscussListResponse> DiscussList(RequestContext<DiscussListRequest> request);

        /// <summary>
        /// 某个瓶子评论详情
        /// </summary>
        ResponseContext<DiscussDetailResponse> DiscussDetail(RequestContext<DiscussDetailRequest> request);

        /// <summary>
        /// 评论某个瓶子
        /// </summary>
        ResponseContext<DiscussResponse> Discuss(RequestContext<DiscussRequest> request);

        /// <summary>
        /// 捡起的漂流瓶列表
        /// </summary>
        ResponseContext<PickUpListResponse> PickUpList(RequestContext<PickUpListRequest> request);

        /// <summary>
        /// 捡一个瓶子
        /// </summary>
        ResponseContext<PickUpResponse> PickUp(RequestContext<PickUpRequest> request);

        /// <summary>
        /// 扔一个瓶子
        /// </summary>
        ResponseContext<PublishMomentResponse> PublishMoment(RequestContext<PublishMomentRequest> request);

        /// <summary>
        /// 我扔出去的所有动态
        /// </summary>
        ResponseContext<MyMomentListResponse> MyMomentList(RequestContext<MyMomentListRequest> request);

        /// <summary>
        /// 动态详情
        /// </summary>
        ResponseContext<MomentDetailResponse> MomentDetail(RequestContext<MomentDetailRequest> request);

        /// <summary>
        /// 获取用户小程序端唯一标示
        /// </summary>
        ResponseContext<BasicUserInfoResponse> UserLogin(RequestContext<UserLoginRequest> request);

        /// <summary>
        /// 删除单个瓶子
        /// </summary>
        ResponseContext<DeleteBottleResponse> DeleteBottle(RequestContext<DeleteBottleRequest> request);

        /// <summary>
        /// 举报某个瓶子
        /// </summary>
        ResponseContext<ReportBottleResponse> ReportBottle(RequestContext<ReportBottleRequest> request);

        /// <summary>
        /// 清空未回复的所有瓶子
        /// </summary>
        ResponseContext<ClearAllBottleResponse> ClearAllBottle(RequestContext<ClearAllBottleRequest> request);

        /// <summary>
        /// 清空单个瓶子未读数量
        /// </summary>
        ResponseContext<ClearUnReadCountResponse> ClearUnReadCount(RequestContext<ClearUnReadCountRequest> request);

        /// <summary>
        /// 未读总数量
        /// </summary>
        ResponseContext<UnReadTotalCountResponse> UnReadTotalCount(RequestContext<UnReadTotalCountRequest> request);

        /// <summary>
        /// 删除回复过的所有瓶子
        /// </summary>
        ResponseContext<DeleteAllBottleResponse> DeleteAllBottle(RequestContext<DeleteAllBottleRequest> request);

        /// <summary>
        /// 所有未读消息标为已读
        /// </summary>
        ResponseContext<ClearAllUnReadCountResponse> ClearAllUnReadCount(RequestContext<ClearAllUnReadCountRequest> request);

        /// <summary>
        /// 用户基础信息
        /// </summary>
        ResponseContext<BasicUserInfoResponse> BasicUserInfo(RequestContext<BasicUserInfoRequest> request);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        ResponseContext<UpdateUserInfoResponse> UpdateUserInfo(RequestContext<UpdateUserInfoRequest> request);

        /// <summary>
        /// 获取用户完整信息
        /// </summary>
        ResponseContext<GetUserInfoResponse> GetUserInfo(RequestContext<GetUserInfoRequest> request);

        /// <summary>
        /// 删除动态
        /// </summary>
        ResponseContext<DeleteMomentResponse> DeleteMoment(RequestContext<DeleteMomentRequest> request);

        /// <summary>
        /// 删除所有动态列表
        /// </summary>
        ResponseContext<DeleteAllMomentResponse> DeleteAllMoment(RequestContext<DeleteAllMomentRequest> request);

        /// <summary>
        /// 更新头像
        /// </summary>
        ResponseContext<UpdateAvatarUrlResponse> UpdateAvatarUrl(RequestContext<UpdateAvatarUrlRequest> request);

        /// <summary>
        /// 获取收藏列表
        /// </summary>
        ResponseContext<GetCollectListResponse> GetCollectList(RequestContext<GetCollectListRequest> request);

        /// <summary>
        /// 删除一个收藏
        /// </summary>
        ResponseContext<DeleteCollectResponse> DeleteCollect(RequestContext<DeleteCollectRequest> request);

        /// <summary>
        /// 新增一个收藏
        /// </summary>
        ResponseContext<AddCollectResponse> AddCollect(RequestContext<AddCollectRequest> request);

        /// <summary>
        /// 保存用户信息
        /// </summary>
        ResponseContext<SetUserInfoResponse> SetUserInfo(RequestContext<SetUserInfoRequest> request);

        /// <summary>
        /// 删除所有收藏
        /// </summary>
        ResponseContext<DeleteAllCollectResponse> DeleteAllCollect(RequestContext<DeleteAllCollectRequest> request);

        /// <summary>
        /// 获取用户金币信息
        /// </summary>
        ResponseContext<UserCoinInfoResponse> UserCoinInfo(RequestContext<UserCoinInfoRequest> request);

        /// <summary>
        /// 金币明细
        /// </summary>
        ResponseContext<CoinDetailResponse> CoinDetail(RequestContext<CoinDetailRequest> request);

        PickUpEntity GetPickUpEntity(Guid pickUpId);

        /// <summary>
        /// 文本内容安全检测
        /// </summary>
        ResponseContext<MsgSecCheckResponse> MsgSecCheck(RequestContext<MsgSecCheckRequest> request);

        /// <summary>
        /// 一键转发动态
        /// </summary>
        ResponseContext<ForwardMomentResponse> ForwardMoment(RequestContext<ForwardMomentRequest> request);

        /// <summary>
        /// 上线时通知我
        /// </summary>
        ResponseContext<OnlineNotifyResponse> OnlineNotify(RequestContext<OnlineNotifyRequest> request);

        /// <summary>
        /// 关注的用户发布的所有动态列表
        /// </summary>
        ResponseContext<AttentionListResponse> AttentionList(RequestContext<AttentionListRequest> request);

        /// <summary>
        /// 取消关注某人
        /// </summary>
        ResponseContext<CancelAttentionResponse> CancelAttention(RequestContext<CancelAttentionRequest> request);

        /// <summary>
        /// 添加关注
        /// </summary>
        ResponseContext<AddAttentionResponse> AddAttention(RequestContext<AddAttentionRequest> request);
    }
}
