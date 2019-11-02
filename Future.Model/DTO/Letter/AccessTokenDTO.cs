namespace Future.Model.DTO.Letter
{
    public class AccessTokenDTO
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string Access_token { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒。目前是7200秒之内的值。
        /// </summary>
        public long Expires_in { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int Errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Errmsg { get; set; }
    }

    public class CheckSignatureResponse
    {
        public string echostr { get; set; }
    }
}
