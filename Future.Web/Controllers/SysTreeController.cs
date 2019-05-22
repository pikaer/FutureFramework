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

        #region 权限树
        public IActionResult List()
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
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetFuncTreeJson", ex);
            }
        }

        public JsonResult GetFunc(string data)
        {
            try
            {
                var id = Convert.ToInt16(data);
                var res = sysService.GetFunctionByFuncId(id);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetFunc", ex);
            }
        }

        public JsonResult AddEqFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<FunctionEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.AddEqFunc(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddEqFunc", ex);
            }
        }

        public JsonResult AddSubFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<FunctionEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.AddSubFunc(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddSubFunc", ex);
            }
        }

        public JsonResult DeleteFunc(string data)
        {
            try
            {
                int id = Convert.ToInt16(data);
                var res = sysService.DeleteFuncs(id);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteFunc", ex);
            }
        }

        public JsonResult UpdateFunc(string data)
        {
            try
            {
                var request = data.JsonToObject<FunctionEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.UpdateFunc(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UpdateFunc", ex);
            }
        }

        public JsonResult ExChangeOrder(string data)
        {
            try
            {
                var request = data.JsonToObject<FunctionEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.UpdateFunc(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "ExChangeOrder", ex);
            }
        }
        #endregion

        #region 日志系统
        public IActionResult SysLogIndex()
        {
            return View();
        }

        public JsonResult GetLogList(int page = 1, int rows = 10)
        {
            try
            {
                var res = sysService.GetLogList(page, rows);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetLogList", ex);
            }
        }
        #endregion
    }
}
