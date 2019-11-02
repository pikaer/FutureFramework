using Future.Model.Enum.Letter;
using System;

namespace Future.Model.Utils
{

    public class RequestContext<T>
    {
        /// <summary>
        /// 请求头
        /// </summary>

        public RequestHead Head { get; set; }

        /// <summary>
        /// 请求体
        /// </summary>

        public T Content { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public RequestContext()
        {
            Head = new RequestHead();
            Content = default;
        }
    }

    public class RequestHead
    {
        public RequestHead()
        {
            TransactionId = Guid.NewGuid();
        }

        /// <summary>
        /// Token信息
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public PlatformEnum Platform { get; set; }

        /// <summary>
        /// 事务号
        /// </summary>
        public Guid TransactionId { get; set; }
    }
}
