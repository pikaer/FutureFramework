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
    public class LogHelper
    {
        private readonly LogRepository logDal = SingletonProvider<LogRepository>.Instance;
        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="head">公共请求头</param>
        /// <param name="keyValuePairs">附件标签</param>
        public void Debug(string title, string content, RequestHead head = null, Dictionary<string, string> keyValuePairs = null)
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
        public void Info(string title, string content, RequestHead head = null, Dictionary<string, string> keyValuePairs = null)
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
        public void Warn(string title, string content, RequestHead head = null, Dictionary<string, string> keyValuePairs = null)
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
        public void Error(string title, string content, Exception ex = null, RequestHead head = null, Dictionary<string, string> keyValuePairs = null)
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
        public void Fatal(string title, string content, Exception ex = null, RequestHead head = null, Dictionary<string, string> keyValuePairs = null)
        {
            WriteLogAsync(LogLevelEnum.Fatal, title, content, head, keyValuePairs, ex);
        }

        /// <summary>
        /// 异步写入日志到数据库
        /// </summary>
        private void WriteLogAsync(LogLevelEnum level, string title, string content, RequestHead head = null, Dictionary<string, string> keyValuePairs = null, Exception ex = null)
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
                Task.Factory.StartNew(() =>
                {
                    WriteLog(level, tid, uid, platform, title, desc, "FutureLetter.Api", keyValuePairs);
                });
            }
            catch
            {
                return;
            }
        }

        private void WriteLog(LogLevelEnum logLevel, Guid tid, long uid, string platform, string title, string content, string serverName, Dictionary<string, string> keyValuePairs = null)
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
                ServiceName = serverName,
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

        public void WriteServiceLog(ServiceLogEntity serviceLogEntity)
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
