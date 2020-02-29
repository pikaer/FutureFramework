namespace Future.Model.Entity.Bingo
{
    /// <summary>
    /// 用户金币账户
    /// </summary>
    public class CoinEntity: BaseEntity
    {
        /// <summary>
        /// 唯一标示Id
        /// </summary>
        public long CoinId { get; set; }

        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户总金币
        /// </summary>
        public int TotalCoin { get; set; }
    }
}
