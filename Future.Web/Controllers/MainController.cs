﻿using Future.Model.DTO.Sys;
using Future.Model.Enum.Bingo;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service.Implement;
using Future.Service.Interface;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Future.Web.Controllers
{
    public class MainController : BaseController
    {
        private readonly ISysBiz sysService = SingletonProvider<SysBiz>.Instance;

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
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetChildrenFunc", ex);
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
                return ErrorJsonResult(ErrCodeEnum.InnerError, "ChildrenFunc", ex);
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
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetModules", ex);
            }
        }
        
        public JsonResult GetFirstMenus()
        {
            try
            {
                var modules = sysService.GetModules(EnumFuncType.Module);
                if (modules.Count > 0)
                {
                    var res = sysService.GetFunctionsByParentId(modules[0].Id);
                    return new JsonResult(new ResponseContext<List<FunctionDTO>>(res));
                }
                return Json("");
            }
            catch(Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetFirstMenus", ex);
            }
        }

        public JsonResult GenderCombobox()
        {
            return new JsonResult(EnumHelper.ToSelectPicker(typeof(GenderEnum)));
        }

        public JsonResult RoleCombobox()
        {
            return new JsonResult(EnumHelper.ToSelectPicker(typeof(RoleEnum)));
        }

        public JsonResult SchoolTypeCombobox()
        {
            return new JsonResult(EnumHelper.ToSelectPicker(typeof(SchoolTypeEnum)));
        }

        public JsonResult LiveStateCombobox()
        {
            return new JsonResult(EnumHelper.ToSelectPicker(typeof(LiveStateEnum)));
        }

        public JsonResult PickUpTypeCombobox()
        {
            return new JsonResult(EnumHelper.ToSelectPicker(typeof(PickUpTypeEnum)));
        }

        public JsonResult MomentStateCombobox()
        {
            return new JsonResult(EnumHelper.ToSelectPicker(typeof(MomentStateEnum)));
        }

        public JsonResult MomentPickUpCombobox()
        {
            return new JsonResult(EnumHelper.ToSelectPicker(typeof(MomentPickUpEnum)));
        }
    }
}