namespace Future.Model.DTO.Today
{
    public class HomeTextDTO
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public long HomeTextId { get; set; }

        /// <summary>
        /// 首页Id
        /// </summary>
        public long HomeInfoId { get; set; }

        /// <summary>
        /// 文本唯一标示
        /// </summary>
        public long TextId { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 文本来源
        /// </summary>
        public string TextSource { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 文本创建人
        /// </summary>
        public string CreateUser { get; set; }

        public string CreateTimeDesc { get; set; }
    }
}
