using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Utility;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Future.TodayApi.Controllers
{
    public class BaseController : Controller
    {
        private readonly LogHelper LogHelper = SingletonProvider<LogHelper>.Instance;
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
            if (string.IsNullOrEmpty(title) || ex != null)
            {
                LogHelper.Error(title, code.ToDescription(), ex);
            }

            return new JsonResult(new ResponseContext<object>(false, code, null));
        }

        /// <summary>
        /// 写入服务接口调用日志到数据库,用作接口性能检测
        /// </summary>
        protected void WriteServiceLog(string module, string method, RequestHead reqHead, ErrCodeEnum code, string msg, object request, object response)
        {
            LogHelper.WriteServiceLog(new ServiceLogEntity()
            {
                Module = module,
                Method = method,
                Request = request.SerializeToString(),
                Response = response.SerializeToString(),
                UId = reqHead.UId,
                Code = (int)code,
                Msg = msg,
                Platform = reqHead.Platform,
                TransactionId = reqHead.TransactionId,
                CreateTime = DateTime.Now
            });
        }
    }
}