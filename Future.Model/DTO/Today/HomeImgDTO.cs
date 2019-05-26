namespace Future.Model.DTO.Today
{
    public class HomeImgDTO
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public long HomeImgId { get; set; }

        /// <summary>
        /// 首页Id
        /// </summary>
        public long HomeInfoId { get; set; }

        /// <summary>
        /// 图片唯一标示
        /// </summary>
        public long ImgId { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 图片来源
        /// </summary>
        public string ImgSource { get; set; }

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
