namespace Future.Model.DTO.Today
{
    public class HomeInfoDTO
    {
        /// <summary>
        /// 唯一标示Id
        /// </summary>
        public long HomeInfoId { get; set; }

        /// <summary>
        /// 展示开始时间
        /// </summary>
        public string DisplayStartDateTime { get; set; }

        /// <summary>
        /// 展示结束时间
        /// </summary>
        public string DisplayEndDateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 文本创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 文本修改人
        /// </summary>
        public string ModifyUser { get; set; }

        public string CreateTimeDesc { get; set; }

        public string ModifyTimeDesc { get; set; }
    }
}
