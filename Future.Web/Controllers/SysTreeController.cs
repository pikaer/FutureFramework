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

        #region 登录

        [LoginCheckFilter(IsCheck = false)]
        public IActionResult LoginIndex()
        {
            var mobile = GetCookies(UserKey);
            if (!mobile.IsNullOrEmpty())
            {
                var currentUser = sysService.StaffByMobile(mobile);

                //记录Session
                SetSession(UserKey, currentUser);

                return new RedirectResult("/Main/Index");
            }
            return View();
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        [LoginCheckFilter(IsCheck=false)]
        public IActionResult Login(string data,bool rememberMe)
        {
            try
            {
                var response = new ResponseContext<bool>();
                var request = data.JsonToObject<StaffEntity>();
                string pwd = string.IsNullOrEmpty(request.PassWord) ? "" : Md5Helper.GetMd5Str32(request.PassWord);
                var currentUser = sysService.StaffByMobile(request.Mobile);
                if (currentUser != null && currentUser.PassWord.ToLower().Trim().Equals(pwd))
                {
                    //记录Session
                    SetSession(UserKey, currentUser);
                    if (rememberMe)
                    {
                        SetCookies(UserKey, currentUser.Mobile);
                    }
                    else
                    {
                        if (!GetCookies(UserKey).IsNullOrEmpty())
                        {
                            DeleteCookies(UserKey);
                        }
                    }
                }
                else
                {
                    response = new ResponseContext<bool>(false, ErrCodeEnum.UserNoExist, false, "用户名或登录密码有误!");
                }
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "Login", ex);
            }
        }
        #endregion

        #region 用户管理
        
        public IActionResult UserManageIndex()
        {
            return View();
        }

        public JsonResult GetStaffList()
        {
            try
            {
                int page = Convert.ToInt16(Request.Form["page"]);
                int rows = Convert.ToInt16(Request.Form["rows"]);
                string staffName = Request.Form["StaffName"];
                string mobile = Request.Form["Mobile"];
                var rtn = sysService.GetStaffList(page, rows, staffName, mobile);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetHomeTextList", ex);
            }
        }

        public JsonResult AddOrUpdateStaff(string data)
        {
            try
            {
                var request = data.JsonToObject<StaffEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = sysService.AddOrUpdateStaff(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddOrUpdateStaff", ex);
            }
        }

        public JsonResult DeleteStaff(string data)
        {
            try
            {
                long staffId = Convert.ToInt16(data);
                var res = sysService.DeleteStaff(staffId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteStaff", ex);
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
        #endregion
    }
}
