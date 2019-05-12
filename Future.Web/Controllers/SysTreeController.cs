using Future.Model.DTO.Sys;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Future.Web.Controllers
{
    public class SysTreeController : BaseController
    {
        private readonly SysService sysService = SingletonProvider<SysService>.Instance;
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Item()
        {
            return View();
        }

        public JsonResult GetFuncTreeJson()
        {
            try
            {
                var res = sysService.GetMenus();
                var rtn = new PageResult<FunctionDTO>(res);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }

        public JsonResult GetFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<CommonRequest>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.GetFunctionByFuncId(request.Id);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }

        public JsonResult AddEqFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<Function>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.AddEqFunc(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }

        public JsonResult AddSubFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<Function>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.AddSubFunc(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }

        public JsonResult DeleteFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<Function>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.DeleteFuncs(request.Id);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }

        public JsonResult UpdateFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<Function>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.UpdateFunc(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }

        public JsonResult ExChangeOrder(string data)
        {
            try
            {
                var request = data.JsonToObject<Function>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.UpdateFunc(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }
    }
}
