using System.ComponentModel;

namespace Future.Model.Enum.Letter
{
    public enum CoinChangeEnum
    {
        [Description("金币充值")]
        Recharge = 0,

        [Description("发布动态奖励")]
        PublishReward= 1,

        [Description("动态被收藏奖励")]
        CollectedReward = 2,

        [Description("日常签到奖励")]
        SignReward = 3,

        [Description("分享奖励")]
        ShareReward = 4,

        [Description("运营活动发放")]
        ActivityReward = 5,

        [Description("首次注册赠送金币")]
        FirstLoginReward = 6,

        [Description("捡瓶子消耗")]
        PickUpDeducted = 100,

        [Description("被举报扣除")]
        ReportedDeducted = 101,
    }
}
