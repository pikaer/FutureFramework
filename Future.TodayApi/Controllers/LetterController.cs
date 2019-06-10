using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Future.TodayApi.Controllers
{
    /// <summary>
    /// 漂流瓶控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LetterController : BaseController
    {
        private readonly string MODULE = "MainController";
        private readonly LetterService api = SingletonProvider<LetterService>.Instance;

        /// <summary>
        /// 获取互动列表
        /// </summary>
        [HttpPost]
        public JsonResult PickUpList()
        {
            RequestContext<PickUpListRequest> request = null;
            ResponseContext<PickUpListResponse> response = null;
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                request = json.JsonToObject<RequestContext<PickUpListRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Content == null|| request.Content.UId<=0|| request.Content.PageIndex<=0)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                response = api.PickUpList(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "PickUpList", ex);
            }
            finally
            {
                WriteServiceLog(MODULE, "PickUpList", request?.Head, response == null ? ErrCodeEnum.Failure : response.Code, response?.ResultMessage, request, response);
            }
        }
    }
}