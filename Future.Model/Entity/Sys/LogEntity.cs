using Future.Model.Enum.Sys;
using System;

namespace Future.Model.Entity.Sys
{
    /// <summary>
    /// 日志实体类
    /// </summary>
    public class LogEntity
    {
        /// <summary>
        /// 唯一主键
        /// </summary>
        public Guid LogId { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevelEnum LogLevel { get; set; }

        /// <summary>
        /// 事务号
        /// </summary>
        public Guid TransactionID { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 平台（小程序miniApp，android,ios,浏览器browser,h5)
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 日志标题
        /// </summary>
        public string LogTitle { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }

        /// <summary>
        /// 服务名称（标示）
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 日志标签（供日志精准筛查）
    /// </summary>
    public class LogTag
    {
        /// <summary>
        /// 唯一主键
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        /// 日志主表Id
        /// </summary>
        public Guid LogId { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string LogKey { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string LogValue { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
