namespace Future.Model.DTO.Today
{
    public class ImgGalleryDTO
    {
        /// <summary>
        /// 图片唯一标示
        /// </summary>
        public long ImgId { get; set; }

        /// <summary>
        /// 图片名
        /// </summary>
        public string ImgName { get; set; }

        /// <summary>
        /// 完整Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 使用次数
        /// </summary>
        public int UseCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyUser { get; set; }

        public string CreateTimeDesc { get; set; }

        public string ModifyTimeDesc { get; set; }
    }
}
