using System;

namespace Future.Model.DTO.Letter
{
    public class WeChatMessageContext<T>
    {
        /// <summary>
        /// 接收者（用户）的 openid
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 所需下发的模板消息的id
        /// </summary>
        public string template_id { get; set; }

        public string access_token { get; set; }

        /// <summary>
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
        /// </summary>
        public string page { get; set; }

        public T data { get; set; }
    }

    public class Value
    {
        public Value(string text)
        {
            value = text;
        }
        public string value { get; set; }
    }

    /// <summary>
    /// 文本内容安全检测
    /// </summary>
    [Serializable]
    public class MsgSecCheckRequestDTO
    {
        public string access_token { get; set; }

        public string appid { get; set; }

        /// <summary>
        /// 要检测的文本内容，长度不超过 500KB
        /// </summary>
        public string content { get; set; }
    }

    public class WeChatResponseDTO
    {
        /// <summary>
        /// 错误码 0:内容正常	87014:内容含有违法违规内容	
        /// </summary>
        public int Errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }

    /// <summary>
    /// 模板消息
    /// </summary>
    public class DiscussNotifyData
    {
        public Value thing1 { get; set; }

        public Value thing2 { get; set; }

        public Value time3 { get; set; }
    }


    /// <summary>
    /// 模板消息
    /// </summary>
    public class DiscussReplyNotifyData
    {
        public Value thing1 { get; set; }

        public Value thing2 { get; set; }

        public Value date3 { get; set; }
    }

}
