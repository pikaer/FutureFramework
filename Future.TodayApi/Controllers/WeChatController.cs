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
        public JsonResult CheckSignature(string signature, string timestamp, string nonce, string echostr)
        {
            var response = new CheckSignatureResponse();
            if (WeChatHelper.CheckSignature(signature,timestamp,nonce))
            {
                response.echostr = echostr;
            }
            else
            {
                response.echostr = "校验失败";
            }
            LogHelper.InfoAsync("CheckSignature", string.Format("{0}_{1}_{2}_{3}", signature, timestamp, nonce, echostr)); 
            return new JsonResult(response);
        }
    }
}