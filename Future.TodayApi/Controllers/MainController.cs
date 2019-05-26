using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Future.TodayApi.Controllers
{
    /// <summary>
    /// 主接口集合
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : BaseController
    {
        private readonly string MODULE = "MainController";
        private readonly TodayService api = SingletonProvider<TodayService>.Instance;

        /// <summary>
        /// 获取聊天列表
        /// </summary>
        [HttpPost]
        public JsonResult GetHomeInfo()
        {
            RequestContext<GetHomeInfoRequest> request = null;
            ResponseContext<GetHomeInfoResponse> response = null;
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                request = json.JsonToObject<RequestContext<GetHomeInfoRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Content == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                response = api.GetHomeInfo(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetHomeInfo", ex);
            }
            finally
            {
                WriteServiceLog(MODULE, "GetHomeInfo", request?.Head, response == null ? ErrCodeEnum.Failure : response.Code, response?.ResultMessage, request, response);
            }
        }

    }
}
