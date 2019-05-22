using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Utility;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Future.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 处理请求数据流
        /// </summary>
        /// <returns></returns>
        protected string GetInputString()
        {
            Stream req = Request.Body;
            string json = new StreamReader(req).ReadToEnd();

            if (!string.IsNullOrEmpty(json))
            {
                while (json.IndexOf("\\/", StringComparison.Ordinal) != -1) json = json.Replace("\\/", "/");
            }

            return json;
        }
        
        /// <summary>
        /// 获取错误的返回
        /// </summary>
        protected JsonResult ErrorJsonResult(ErrCodeEnum code, string title = null, Exception ex = null)
        {
            //todo 记录ex异常日志
            if (string.IsNullOrEmpty(title) || ex != null)
            {
                Log.Error(title, code.ToDescription(), ex);
            }
            return new JsonResult(new ResponseContext<object>(false,code,null));
        }
        
    }
}