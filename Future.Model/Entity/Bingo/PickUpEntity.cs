using Future.Model.Enum.Bingo;
using System;

namespace Future.Model.Entity.Bingo
{
    /// <summary>
    /// 漂流瓶被捡起实体
    /// </summary>
    public class PickUpEntity: BaseEntity
    {
        /// <summary>
        /// 互动Id
        /// </summary>
        public Guid PickUpId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 动态发布者Id(减少关联查询）
        /// </summary>
        public long MomentUId { get; set; }

        /// <summary>
        /// 参与评论的用户
        /// </summary>
        public long PickUpUId { get; set; }

        public PickUpFromPageEnum FromPage { get; set; }

        /// <summary>
        /// 漂流瓶列表中是否删除
        /// </summary>
        public bool IsPickUpDelete { get; set; }

        /// <summary>
        /// 动态发布者是否已删除
        /// </summary>
        public bool IsUserDelete { get; set; }

        /// <summary>
        /// 瓶子捡起者是否删除
        /// </summary>
        public bool IsPartnerDelete { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 匿名时候对应的昵称
        /// </summary>
        public string HidingNickName { get; set; }

        /// <summary>
        /// 动态发布者最后删除时间
        /// </summary>
        public DateTime? UserLastDeleteTime { get; set; }

        /// <summary>
        /// 瓶子捡起者最后删除时间
        /// </summary>
        public DateTime? PartnerLastDeleteTime { get; set; }
    }
}
