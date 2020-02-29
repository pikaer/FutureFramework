using Future.Model.Enum.Bingo;
using System;

namespace Future.Model.Entity.Bingo
{
    /// <summary>
    /// 用户金币明细表
    /// </summary>
    public class CoinDetailEntity: BaseEntity
    {
        /// <summary>
        /// 唯一标示Id
        /// </summary>
        public Guid CoinDetailId { get; set; }

        /// <summary>
        /// 用户唯一标示
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 用户金币账户Id
        /// </summary>
        public long CoinId { get; set; }

        /// <summary>
        /// 金币改变值（正数为增加金币，负数为减少金币）
        /// </summary>
        public int ChangeValue { get; set; }

        /// <summary>
        /// 金币变动类别
        /// </summary>
        public CoinChangeEnum CoinChangeType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 金币操作者（默认系统system）
        /// </summary>
        public string OperateUser { get; set; }
    }
}
