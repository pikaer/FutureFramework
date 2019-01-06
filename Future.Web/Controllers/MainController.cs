using Future.Model.DTO.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service;
using Future.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Future.Web.Controllers
{
    public class MainController : BaseController
    {
        private readonly SysService sysService = SysService.Instance;

        public IActionResult Index()
        {
            return View();
        }
        
        public JsonResult GetChildrenFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<CommonRequest>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.GetFunctionsByParentId(request.Id);
                var response = new ResponseContext<List<FunctionDTO>>(res);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError,ex);
            }
        }
        
        public JsonResult ChildrenFunc(string data)
        {
            try
            {
                var res = sysService.GetFunctionsByParentId(Convert.ToInt16(data));
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }

        public JsonResult GetModules()
        {
            try
            {
                var res = sysService.GetModules(EnumFuncType.Module);
                return new JsonResult(new ResponseContext<List<FunctionDTO>>(res));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }
        
        public JsonResult GetFirstMenus()
        {
            try
            {
                var modules = sysService.GetModules(EnumFuncType.Module);
                if (modules.Count > 0)
                {
                    var res = sysService.GetFunctionsByParentId(modules[0].FuncId);
                    return new JsonResult(new ResponseContext<List<FunctionDTO>>(res));
                }
                return Json("");
            }
            catch(Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }
    }
}