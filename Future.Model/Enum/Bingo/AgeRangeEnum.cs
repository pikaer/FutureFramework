using System.ComponentModel;

namespace Future.Model.Enum.Bingo
{
    public enum AgeRangeEnum
    {
        [Description("无")]
        Default = 0,
        [Description("80前")]
        Before80,
        [Description("80后")]
        After80,
        [Description("85后")]
        After85,
        [Description("90后")]
        After90,
        [Description("95后")]
        After95,
        [Description("00后")]
        After00,
        [Description("05后")]
        After05
    }
}
