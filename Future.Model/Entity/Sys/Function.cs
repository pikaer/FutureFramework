using Future.Model.Enum.Sys;

namespace Future.Model.Entity.Sys
{
    public class Function: BaseEntity
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public int FuncId { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 文本描述
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 可作为url，亦可作为控件id
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string IconCls { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public EnumFuncType EnumFuncType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
