using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Utility;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Reflection;

namespace Future.Web.Controllers
{
    public class BaseController : Controller
    {
        protected const string UserKey = "CurrentUser";
        

        #region Base
        /// <summary>
        /// 当前登录的用户属性
        /// </summary>
        public StaffEntity CurrentUserInfo
        {
            get
            {
                HttpContext.Session.TryGetValue(UserKey, out byte[] result);
                return result.Bytes2Object<StaffEntity>();
            }
        }
        
        /// <summary>
        /// 获取错误的返回
        /// </summary>
        protected JsonResult ErrorJsonResult(ErrCodeEnum code, string title = null, Exception ex = null)
        {
            //todo 记录ex异常日志
            if (string.IsNullOrEmpty(title) || ex != null)
            {
                LogHelper.ErrorAsync(title, code.ToDescription(), ex);
            }
            return new JsonResult(new ResponseContext<object>(false,code,null));
        }

        /// <summary>
        /// 请求过滤处理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            var attrs = (LoginCheckFilter)((ControllerActionDescriptor)filterContext.ActionDescriptor)
                .MethodInfo.GetCustomAttribute(typeof(LoginCheckFilter), true);

            if (attrs!=null&&!attrs.IsCheck)
            {
                return;
            }
            else
            {
                if (GetSession<StaffEntity>(UserKey) == null)
                {
                    filterContext.Result = new RedirectResult("/SysTree/LoginIndex");
                    return;
                }
            }
        }

        #endregion

        #region Cookies
        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：分钟</param>      
        protected void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
        #endregion

        #region Session
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>   
        protected void SetSession<T>(string key, T value)
        {
            //记录Session
            HttpContext.Session.Set(key, value.Object2Bytes());
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteSession(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected T GetSession<T>(string key)
        {
            HttpContext.Session.TryGetValue(key, out byte[] value);
            if (value == null)
            {
                return default(T);
            }
            else
            {
                return value.Bytes2Object<T>();
            }
        }
        #endregion
    }


    /// <summary>
    /// 默认校验会话Session
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class LoginCheckFilter : Attribute
    {
        public bool IsCheck { get; set; } = true;
    }
}