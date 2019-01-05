using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Model.Utils
{
    public class RequestContext<T>
    {
        /// <summary>
        /// 请求体
        /// </summary>
        public T Content { get; set; }
    }
}
