using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service;
using Future.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Future.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : BaseController
    {
        private readonly SysService sysService = SysService.Instance;

        public IActionResult Index()
        {
            return View();
        }
        
        public JsonResult GetChildrenFunc()
        {
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                var request = json.JsonToObject<RequestContext<CommonRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Content == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                var res = sysService.GetFunctionsByPara(request.Content.Id, false);
                var response = new ResponseContext<List<Function>>(res);
                return new JsonResult(response);
            }
            catch
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError);
            }
        }

        public JsonResult GetModules()
        {
            try
            {
                var res = sysService.GetModules(EnumFuncType.Module);
                return new JsonResult(new ResponseContext<List<Function>>(res));
            }
            catch
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError);
            }
        }
        
        public JsonResult GetFirstMenus()
        {
            try
            {
                var modules = sysService.GetModules(EnumFuncType.Module);
                if (modules.Count > 0)
                {
                    var res = sysService.GetFunctionsByPara(modules[0].FuncId, false);
                    return new JsonResult(new ResponseContext<List<Function>>(res));
                }
                return Json("");
            }
            catch
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError);
            }
        }

        public JsonResult GetFunc()
        {
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                var request = json.JsonToObject<RequestContext<CommonRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Content == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                var res = sysService.GetFunctionsByPara(request.Content.Id, true)[0];
                var response = new ResponseContext<Function>(res);
                return new JsonResult(response);
            }
            catch
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError);
            }
        }
    }
}