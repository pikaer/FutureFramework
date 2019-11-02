using System;
using System.Collections.Generic;

namespace Future.Model.DTO.Letter
{
    /// <summary>
    /// 文本内容安全检测
    /// </summary>
    [Serializable]
    public class MsgSecCheckRequestDTO
    {
        /// <summary>
        /// 要检测的文本内容，长度不超过 500KB
        /// </summary>
        public string content { get; set; }
    }

    /// <summary>
    /// 模板消息
    /// </summary>
    public class MessageTemplateDTO
    {
        /// <summary>
        /// 接收者（用户）的 openid
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 所需下发的模板消息的id
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
        /// </summary>
        public string page { get; set; }

        /// <summary>
        /// 表单提交场景下，为 submit 事件带上的 formId；支付场景下，为本次支付的 prepay_id
        /// </summary>
        public string form_id { get; set; }

        /// <summary>
        /// 模板需要放大的关键词，不填则默认无放大
        /// </summary>
        public string emphasis_keyword { get; set; }

        /// <summary>
        /// 模板内容，不填则下发空模板。具体格式请参考示例。
        /// </summary>
        public Dictionary<string, string> data { get; set; }
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

}
