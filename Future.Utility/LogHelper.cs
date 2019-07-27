using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Repository;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Future.Utility
{
    public static class LogHelper
    {
        private readonly static LogRepository logDal = SingletonProvider<LogRepository>.Instance;
        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public static void Debug(string title, string content,Dictionary<string, string> keyValuePairs = null, RequestHead head = null)
        {
            WriteLog(LogLevelEnum.Debug, title, content, head, keyValuePairs);
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public static void DebugAsync(string title, string content, Dictionary<string, string> keyValuePairs = null, RequestHead head = null)
        {
            WriteLogAsync(LogLevelEnum.Debug, title, content, head, keyValuePairs);
        }

        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public static void Info(string title, string content, Dictionary<string, string> keyValuePairs = null, RequestHead head = null)
        {
            WriteLog(LogLevelEnum.Info, title, content, head, keyValuePairs);
        }

        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public static void InfoAsync(string title, string content, Dictionary<string, string> keyValuePairs = null, RequestHead head = null)
        {
            WriteLogAsync(LogLevelEnum.Info, title, content, head, keyValuePairs);
        }

        /// <summary>
        /// Warn日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public static void Warn(string title, string content, Dictionary<string, string> keyValuePairs = null,RequestHead head = null)
        {
            WriteLog(LogLevelEnum.Warn, title, content, head, keyValuePairs);
        }

        /// <summary>
        /// Warn日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public static void WarnAsync(string title, string content, Dictionary<string, string> keyValuePairs = null, RequestHead head = null)
        {
            WriteLogAsync(LogLevelEnum.Warn, title, content, head, keyValuePairs);
        }

        /// <summary>
        /// Error异常日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="ex">Exception</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public static void Error(string title, string content, Exception ex = null, Dictionary<string, string> keyValuePairs = null, RequestHead head = null)
        {
            WriteLog(LogLevelEnum.Error, title, content, head, keyValuePairs, ex);
        }

        public static void ErrorAsync(string title, string content, Exception ex = null, Dictionary<string, string> keyValuePairs = null, RequestHead head = null)
        {
            WriteLogAsync(LogLevelEnum.Error, title, content, head, keyValuePairs, ex);
        }

        /// <summary>
        /// Fatal致命错误日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="ex">Exception</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public static void Fatal(string title, string content, Exception ex = null, Dictionary<string, string> keyValuePairs = null, RequestHead head = null)
        {
            WriteLog(LogLevelEnum.Fatal, title, content, head, keyValuePairs, ex);
        }

        /// <summary>
        /// 异步写入日志到数据库
        /// </summary>
        private static void WriteLogAsync(LogLevelEnum level, string title, string content, RequestHead head = null, Dictionary<string, string> keyValuePairs = null, Exception ex = null)
        {
            Task.Factory.StartNew(() =>
            {
                WriteLog(level, title, content, head, keyValuePairs);
            });
        }

        private static void WriteLog(LogLevelEnum level, string title, string content, RequestHead head = null, Dictionary<string, string> keyValuePairs = null, Exception ex = null)
        {
            try
            {
                string desc = string.Empty;
                string platform = string.Empty;
                Guid tid = Guid.Empty;
                long uid = 0;

                if (ex == null)
                {
                    desc = string.Format("Content:{0}", content);
                }
                else
                {
                    desc = string.Format("Content:{0},Exception:{1}", content, ex.ToString());
                }
                if (head != null)
                {
                    tid = head.TransactionId;
                    uid = head.UId;
                    platform = head.Platform;
                }
                WriteLog(level, tid, uid, platform, title, desc, keyValuePairs);
            }
            catch
            {
                return;
            }
        }

        private static void WriteLog(LogLevelEnum logLevel, Guid tid, long uid, string platform,string title, string content,Dictionary<string, string> keyValuePairs = null)
        {
            //日志开关
            if (!ConfigHelper.GetBool("LogIsOpen"))
            {
                return;
            }

            //日志级别开关
            var level = ConfigHelper.GetInt("DefaultLogLevel", 0);
            if ((int)logLevel < level)
            {
                return;
            }
            
            var logEntity = new LogEntity()
            {
                LogId = Guid.NewGuid(),
                LogLevel = logLevel,
                TransactionID = tid,
                UId = uid,
                Platform = platform,
                LogTitle = title,
                LogContent = content,
                ServiceName = ConfigHelper.GetString("ServiceName"),
                CreateTime = DateTime.Now
            };
            logDal.InsertLogs(logEntity);

            if (keyValuePairs != null && keyValuePairs.Count > 0)
            {
                foreach (var item in keyValuePairs)
                {
                    var tagEntity = new LogTag()
                    {
                        TagId = Guid.NewGuid(),
                        LogId = logEntity.LogId,
                        LogKey = item.Key,
                        LogValue = item.Value,
                        CreateTime = DateTime.Now
                    };
                    logDal.InsertTags(tagEntity);
                }
            }

            //发送报警邮件
            if (ConfigHelper.GetBool("ErrorLogSendEmail") && (logLevel == LogLevelEnum.Error || logLevel == LogLevelEnum.Fatal))
            {
                string body = string.Format("UId={0},TransactionID={1},Platform={2},LogId={3},Content={4}",
                    uid.ToString(), tid.ToString(), platform, logEntity.LogId.ToString(), content);

                MailHelper.Send(title, body);
            }
        }

        public static void WriteServiceLog(ServiceLogEntity serviceLogEntity)
        {
            //日志开关
            if (!ConfigHelper.GetBool("ServiceLogIsOpen"))
            {
                return;
            }

            Task.Factory.StartNew(() =>
            {
                serviceLogEntity.ServiceLogId = Guid.NewGuid();
                serviceLogEntity.CreateTime = DateTime.Now;
                serviceLogEntity.ServiceName = ConfigHelper.GetString("ServiceName");
                logDal.WriteServiceLog(serviceLogEntity);
            });
        }
    }
}
