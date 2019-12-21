using Future.Model.DTO.Letter;

namespace Future.CommonBiz
{
    public interface IMiniAppBiz
    {
        /// <summary>
        /// 发送动态评论订阅消息
        /// </summary>
        /// <param name="toUserOpenId">接收者（用户）的 openid</param>
        /// <param name="title">动态标题</param>
        /// <param name="content">评论内容</param>
        bool SendMomentDiscussNotify(string toUserOpenId, string title, string content);

        /// <summary>
        /// 评论被回复后，若对方不在线则发送通知
        /// </summary>
        bool SendDiscussReplyNotify(string toUserOpenId, string title, string content);

        /// <summary>
        /// 检测文本内容是否合法
        /// </summary>
        bool MsgIsOk(string msg);

        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据
        /// </summary>
        AccessTokenDTO GetAccessToken();

        GetOpenIdDTO GetOpenId(string loginCode);
    }
}
