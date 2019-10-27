using Future.Model.DTO.Letter;
using Future.Model.Enum.Letter;
using Infrastructure;

namespace Future.Utility
{
    public static class WeChatHelper
    {
        public static bool MsgIsOk(ChannelEnum platform,string msg)
        {
            var token = GetAccessToken(platform);
            if(token == null|| token.Access_token.IsNullOrEmpty())
            {
                return true;
            }
            string url;
            switch (platform)
            {
                case ChannelEnum.QQ_MiniApp:
                    url = string.Format("https://api.q.qq.com/wxa/msg_sec_check?access_token={0}", token.Access_token);
                    break;
                case ChannelEnum.WX_MiniApp:
                default:
                    url = string.Format("https://api.weixin.qq.com/wxa/msg_sec_check?access_token={0}", token.Access_token);
                    break;
            }
            var request = new MsgSecCheckRequestDTO()
            {
                Content = msg
            };
            var response = HttpHelper.HttpPost<MsgSecCheckRequestDTO,MsgSecCheckResponseDTO>(url, request,20);
            return response != null && response.Errcode == 0;
        }

        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据
        /// </summary>
        public static AccessTokenDTO GetAccessToken(ChannelEnum platform)
        {
            string url;
            string myAppid = GetAppId(platform);
            string mySecret = GetSecret(platform);
            switch (platform)
            {
                case ChannelEnum.QQ_MiniApp:
                    url = string.Format("https://api.q.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", myAppid, mySecret);
                    break;
                case ChannelEnum.WX_MiniApp:
                default:
                    url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", myAppid, mySecret);
                    break;
            }
            return HttpHelper.HttpGet<AccessTokenDTO>(url);
        }

        public static GetOpenIdDTO GetOpenId(ChannelEnum platform,string loginCode)
        {
            string url;
            string myAppid= GetAppId(platform);
            string mySecret= GetSecret(platform);
            switch (platform)
            {
                case ChannelEnum.QQ_MiniApp:
                    url = string.Format("https://api.q.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, loginCode);
                    break;
                case ChannelEnum.WX_MiniApp:
                default:
                    url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, loginCode);
                    break;
            }
            return HttpHelper.HttpGet<GetOpenIdDTO>(url);
        }

        private static string GetAppId(ChannelEnum platform)
        {
            switch (platform)
            {
                case ChannelEnum.QQ_MiniApp:
                    return JsonSettingHelper.AppSettings["BingoAppId"];
                case ChannelEnum.WX_MiniApp:
                default:
                    return JsonSettingHelper.AppSettings["LetterAppId"]; 
            }
        }

        private static string GetSecret(ChannelEnum platform)
        {
            switch (platform)
            {
                case ChannelEnum.QQ_MiniApp:
                    return JsonSettingHelper.AppSettings["BingoSecret"];
                case ChannelEnum.WX_MiniApp:
                default:
                    return JsonSettingHelper.AppSettings["LetterSecret"];
            }
        }
    }
}
