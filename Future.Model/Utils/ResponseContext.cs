using Future.Model.Enum.Sys;
using Infrastructure;

namespace Future.Model.Utils
{
    public class ResponseContext<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseContext(T data)
        {
            Success = true;
            Code = ErrCodeEnum.Success;
            ResultMessage = ErrCodeEnum.Success.ToDescription();
            Content = data;
        }

        public ResponseContext(bool success, ErrCodeEnum err, T data)
        {
            Success = success;
            Code = err;
            ResultMessage = err.ToDescription();
            Content = data;
        }
        
        /// <summary>
        /// 返回值
        /// true:成功
        /// false:失败
        /// </summary>

        public bool Success { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>

        public ErrCodeEnum Code { get; set; }

        /// <summary>
        /// 返回文本
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// 响应体
        /// </summary>
        public T Content { get; set; }
    }
}
