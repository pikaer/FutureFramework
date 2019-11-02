using Future.Model.DTO.Letter;
using Future.Model.Enum.Letter;
using Infrastructure;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Future.Utility
{
    public static class WeChatHelper
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="platform">渠道</param>
        /// <returns></returns>
        public static WeChatResponseDTO SendTemplateMessage(MessageTemplateDTO message, PlatformEnum platform)
        {
            if (message == null)
            {
                return null;
            }
            var token = GetAccessToken(platform);
            if(token==null|| token.Errcode != 0)
            {
                return null;
            }
            string url;
            if (platform== PlatformEnum.QQ_MiniApp)
            {
                url = string.Format("https://api.q.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}", token.Access_token);
            }
            else
            {
                url = string.Format("https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}", token.Access_token);
            }
            return HttpHelper.HttpPost<MessageTemplateDTO, WeChatResponseDTO>(url, message, 20);
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        public static bool CheckSignature(string signature, string timestamp, string nonce)
        {
            try
            {
                var token = JsonSettingHelper.AppSettings["BingoToken"];

                string[] arrTmp = { token, timestamp, nonce };

                Array.Sort(arrTmp);

                string tmpStr = string.Join("", arrTmp);

                var sha1 = new SHA1CryptoServiceProvider();//创建SHA1对象
                var data = sha1.ComputeHash(Encoding.UTF8.GetBytes(tmpStr));//Hash运算
                
                var sb = new StringBuilder();
                foreach (var t in data)
                {
                    sb.Append(t.ToString("X2"));
                }

                sha1.Dispose();

                return sb.ToString().ToLower().Equals(signature);
            }
            catch(Exception ex)
            {
                LogHelper.ErrorAsync("CheckSignature", ex);
                return false;
            }
        }

        public static bool MsgIsOk(PlatformEnum platform,string msg)
        {
            var token = GetAccessToken(platform);
            if(token == null|| token.Access_token.IsNullOrEmpty())
            {
                return true;
            }
            string url;
            switch (platform)
            {
                case PlatformEnum.QQ_MiniApp:
                    url = string.Format("https://api.q.qq.com/wxa/msg_sec_check?access_token={0}", token.Access_token);
                    break;
                case PlatformEnum.WX_MiniApp:
                default:
                    url = string.Format("https://api.weixin.qq.com/wxa/msg_sec_check?access_token={0}", token.Access_token);
                    break;
            }
           
            var request = new MsgSecCheckRequestDTO()
            {
                content = msg
            };
            var response = HttpHelper.HttpPost<MsgSecCheckRequestDTO, WeChatResponseDTO>(url, request, 20);
            return response != null && response.Errcode == 0;
        }

        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据
        /// </summary>
        public static AccessTokenDTO GetAccessToken(PlatformEnum platform)
        {
            string url;
            string myAppid = GetAppId(platform);
            string mySecret = GetSecret(platform);
            switch (platform)
            {
                case PlatformEnum.QQ_MiniApp:
                    url = string.Format("https://api.q.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", myAppid, mySecret);
                    break;
                case PlatformEnum.WX_MiniApp:
                default:
                    url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", myAppid, mySecret);
                    break;
            }
            return HttpHelper.HttpGet<AccessTokenDTO>(url);
        }

        public static GetOpenIdDTO GetOpenId(PlatformEnum platform,string loginCode)
        {
            string url;
            string myAppid= GetAppId(platform);
            string mySecret= GetSecret(platform);
            switch (platform)
            {
                case PlatformEnum.QQ_MiniApp:
                    url = string.Format("https://api.q.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, loginCode);
                    break;
                case PlatformEnum.WX_MiniApp:
                default:
                    url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, loginCode);
                    break;
            }
            return HttpHelper.HttpGet<GetOpenIdDTO>(url);
        }

        private static string GetAppId(PlatformEnum platform)
        {
            switch (platform)
            {
                case PlatformEnum.QQ_MiniApp:
                    return JsonSettingHelper.AppSettings["BingoAppId"];
                case PlatformEnum.WX_MiniApp:
                default:
                    return JsonSettingHelper.AppSettings["LetterAppId"]; 
            }
        }

        private static string GetSecret(PlatformEnum platform)
        {
            switch (platform)
            {
                case PlatformEnum.QQ_MiniApp:
                    return JsonSettingHelper.AppSettings["BingoSecret"];
                case PlatformEnum.WX_MiniApp:
                default:
                    return JsonSettingHelper.AppSettings["LetterSecret"];
            }
        }
    }
}
