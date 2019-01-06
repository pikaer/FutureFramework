using Future.Model.DTO.Sys;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service;
using Future.Utility;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Future.Web.Controllers
{
    public class SysTreeController : BaseController
    {
        private readonly SysService sysService = SysService.Instance;
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

        

        //public JsonResult ExChangeOrder(string aId, string bId)
        //{
        //    var a = UnitOfWork.GetByKey<Func>(aId);
        //    var b = UnitOfWork.GetByKey<Func>(bId);
        //    var orderIndex = a.OrderIndex;
        //    a.OrderIndex = b.OrderIndex;
        //    b.OrderIndex = orderIndex;

        //    return Json(UnitOfWork.Commit());
        //}

        //public JsonResult DeleteFunc(string id)
        //{
        //    UnitOfWork.Delete(GetAllChildren(id));
        //    UnitOfWork.Delete(id);
        //    return Json(UnitOfWork.Commit());
        //}

        //public JsonResult Save()
        //{
        //    var dic = Request.Form["formData"].JsonToObject<Dictionary<string, object>>();
        //    Func cForm = ConvertHelper.ConvertToObj<Func>(dic);

        //    if (string.IsNullOrEmpty(cForm.Id))
        //    {
        //        cForm.Id = GuidHelper.CreateTimeOrderID();
        //        UnitOfWork.Add(cForm);
        //    }
        //    else
        //    {
        //        UnitOfWork.UpdateEntity(cForm);
        //    }

        //    return Json(UnitOfWork.Commit());
        //}

        //private List<Func> GetAllChildren(string funcId)
        //{
        //    List<Func> temps = new List<Func>();
        //    List<Func> children = UnitOfWork.Get<Func>(a => a.ParentId == funcId);
        //    foreach (var child in children)
        //    {
        //        temps.Add(child);
        //        temps.AddRange(GetAllChildren(child.Id));
        //    }
        //    return temps;
        //}
    }
}
