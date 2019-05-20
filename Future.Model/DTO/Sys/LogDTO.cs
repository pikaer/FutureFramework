using System;

namespace Future.Model.DTO.Sys
{
    public class LogDTO
    {
        /// <summary>
        /// 唯一主键
        /// </summary>
        public Guid LogId { get; set; }

        /// <summary>
        /// 日志标题
        /// </summary>
        public string LogTitle { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
