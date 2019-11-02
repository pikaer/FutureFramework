using Future.Model.DTO.Letter;
using Future.Utility;
using Microsoft.AspNetCore.Mvc;

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
        public string SendTemplateMessage(string signature, string timestamp, string nonce, string echostr)
        {
            bool success = WeChatHelper.CheckSignature(signature, timestamp, nonce);
            LogHelper.InfoAsync("CheckSignature", string.Format("signature={0}_timestamp={1}_nonce={2}_echostr={3}_result={4}", signature, timestamp, nonce, echostr, success.ToString())); 
            return success? echostr:"校验失败";
        }
    }
}