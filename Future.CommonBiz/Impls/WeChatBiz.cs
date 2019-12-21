using Future.Model.DTO.Letter;
using Infrastructure;
using System;

namespace Future.CommonBiz.Impls
{
    public class WeChatBiz : IMiniAppBiz
    {
        public bool MsgIsOk(string msg)
        {
            var token = GetAccessToken();
            if (token == null || token.Access_token.IsNullOrEmpty())
            {
                return true;
            }
            string url = string.Format("https://api.weixin.qq.com/wxa/msg_sec_check?access_token={0}", token.Access_token);

            var request = new MsgSecCheckRequestDTO()
            {
                content = msg
            };
            var response = HttpHelper.HttpPost<MsgSecCheckRequestDTO, WeChatResponseDTO>(url, request, 20);
            return response != null && response.Errcode == 0;
        }

        public bool SendMomentDiscussNotify(string toUserOpenId, string title, string content)
        {
            var message = new WeChatMessageContext<DiscussNotifyData>()
            {
                touser = toUserOpenId,
                template_id = "GytyYcEW0BqLnACK9hFZMMXbvOZc2oq5DQjdJ65sRFI",
                page = "pages/discovery/discovery",
                data = new DiscussNotifyData()
                {
                    thing1 = new Value(title),
                    thing2 = new Value(content),
                    time3 = new Value(DateTime.Now.ToString("f"))
                }
            };

            var token = GetAccessToken();
            if (token == null || token.Errcode != 0)
            {
                return false;
            }

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={0}", token.Access_token);

            var response = HttpHelper.HttpPost<object, WeChatResponseDTO>(url, message, 20);
            return response != null && response.Errcode == 0;
        }

        public bool SendDiscussReplyNotify(string toUserOpenId, string title, string content)
        {
            var message = new WeChatMessageContext<DiscussReplyNotifyData>()
            {
                touser = toUserOpenId,
                template_id = "-zecjwuk6Z0uN1txUSwvgXKmaek081c1Y9t6mqAn6ck",
                page = "pages/discovery/discovery",
                data = new DiscussReplyNotifyData()
                {
                    thing1 = new Value(title),
                    thing2 = new Value(content),
                    date3 = new Value(DateTime.Now.ToString("f"))
                }
            };

            var token = GetAccessToken();
            if (token == null || token.Errcode != 0)
            {
                return false;
            }

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={0}", token.Access_token);

            var response = HttpHelper.HttpPost<object, WeChatResponseDTO>(url, message, 20);
            return response != null && response.Errcode == 0;
        }


        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据
        /// </summary>
        public AccessTokenDTO GetAccessToken()
        {
            string myAppid = JsonSettingHelper.AppSettings["LetterAppId"];
            string mySecret = JsonSettingHelper.AppSettings["LetterSecret"];
            string url =string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", myAppid, mySecret);
          
            return HttpHelper.HttpGet<AccessTokenDTO>(url);
        }

        public GetOpenIdDTO GetOpenId(string loginCode)
        {
            string myAppid = JsonSettingHelper.AppSettings["LetterAppId"];
            string mySecret = JsonSettingHelper.AppSettings["LetterSecret"];
            string url=string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, loginCode);
      
            return HttpHelper.HttpGet<GetOpenIdDTO>(url);
        }
    }
}
