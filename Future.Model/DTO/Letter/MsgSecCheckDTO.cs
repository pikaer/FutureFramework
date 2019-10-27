namespace Future.Model.DTO.Letter
{
    /// <summary>
    /// 文本内容安全检测
    /// </summary>
    public class MsgSecCheckRequestDTO
    {
        /// <summary>
        /// 小程序全局唯一后台接口调用凭据
        /// </summary>
        public string Access_token { get; set; }

        /// <summary>
        /// 要检测的文本内容，长度不超过 500KB
        /// </summary>
        public string Content { get; set; }
    }

    public class MsgSecCheckResponseDTO
    {
        /// <summary>
        /// 错误码 0:内容正常	87014:内容含有违法违规内容	
        /// </summary>
        public int Errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }

}
