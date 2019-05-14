using Future.Model.Enum.Sys;
using System;

namespace Future.Model.Entity.Sys
{
    public class FunctionEntity
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// 排序字段
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public int? ModifyUserId { get; set; }
    }
}
