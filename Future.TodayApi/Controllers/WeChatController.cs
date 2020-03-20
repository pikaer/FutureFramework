using Future.Utility;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Future.TodayApi.Controllers
{
    /// <summary>
    /// 微信交互控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeChatController : BaseController
    {
        /// <summary>
        ///  微信小程序模板校验签名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string VerifySignature(string signature, string timestamp, string nonce, string echostr)
        {
            bool success = CheckSignature(signature, timestamp, nonce);
            return success ? echostr : "校验失败";
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        private bool CheckSignature(string signature, string timestamp, string nonce)
        {
            try
            {
                var token = JsonSettingHelper.AppSettings["BingoToken"];

                string[] arrTmp = { token, timestamp, nonce };

                Array.Sort(arrTmp);

                string tmpStr = string.Join("", arrTmp);

                var sha1 = new SHA1CryptoServiceProvider();
                var data = sha1.ComputeHash(Encoding.UTF8.GetBytes(tmpStr));

                var sb = new StringBuilder();
                foreach (var t in data)
                {
                    sb.Append(t.ToString("X2"));
                }

                sha1.Dispose();

                return sb.ToString().ToLower().Equals(signature);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorAsync("CheckSignature", ex);
                return false;
            }
        }
    }
}
