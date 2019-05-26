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
        /// 内容合集
        /// </summary>
        public List<HomeInfoType> HomeInfoList { get; set; }
    }

    public class HomeInfoType
    {
        /// <summary>
        /// 大标题（今日份言论，今日份沙雕）
        /// </summary>
        public string HeadLine { get; set; }

        /// <summary>
        /// 内容集合
        /// </summary>
        public List<ContentType> ContentList { get; set; }
    }

    public class ContentType
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 图片内容集合
        /// </summary>
        public List<string> ImgList { get; set; }
    }
    #endregion
}
