using System.Collections.Generic;

namespace Future.Model.Utils
{
    #region GetHomeInfo（首页内容）
    public class GetHomeInfoRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }
    }

    public class GetHomeInfoResponse
    {
        /// <summary>
        /// 今日份言论
        /// </summary>
        public List<TodayContentType> TodayTextList { get; set; }

        /// <summary>
        /// 今日份沙雕
        /// </summary>
        public List<TodayContentType> TodayImgList { get; set; }
    }
    

    /// <summary>
    /// 今日份言论/沙雕图
    /// </summary>
    public class TodayContentType
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
    }
    
    #endregion
}
