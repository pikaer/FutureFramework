namespace Future.Model.Entity.Bingo
{
    /// <summary>
    /// 用户画像
    /// </summary>
    public class UserPortraitEntity
    {
        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 获取动态数量
        /// </summary>
        public long PickUpCount { get; set; }

        /// <summary>
        /// 该用户发布动态数量
        /// </summary>
        public int MomentCount { get; set; }

        /// <summary>
        /// 发布纯文本动态数量
        /// </summary>
        public int TextMomentCount { get; set; }

        /// <summary>
        /// 发布带图片动态数量
        /// </summary>
        public int ImgMomentCount { get; set; }

        /// <summary>
        /// 收藏的动态数量
        /// </summary>
        public int CollectMomentCount { get; set; }

        /// <summary>
        /// 关注的好友数量
        /// </summary>
        public int AttentionFriendsCount { get; set; }

        /// <summary>
        /// 发布动态被举报次数
        /// </summary>
        public int ReportCount { get; set; }

        /// <summary>
        /// 举报别人次数
        /// </summary>
        public int ReportOthersCount { get; set; }

        /// <summary>
        /// 发布动态内容违规被拦截次数
        /// </summary>
        public int MomentIsValidCount { get; set; }

        /// <summary>
        /// 互动内容违规被拦截次数
        /// </summary>
        public int DiscussIsValidCount { get; set; }

        /// <summary>
        /// 活跃天数
        /// </summary>
        public int ActiveDays { get; set; }

        /// <summary>
        /// 聊天内容主动要求添加微信，QQ 次数
        /// </summary>
        public int AddFriendCount { get; set; }
    }
}
