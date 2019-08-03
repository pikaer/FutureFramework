using Future.Model.Entity.Letter;
using Future.Model.Enum.Letter;
using Future.Repository;
using Future.Utility;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Future.Service
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserBiz
    {
        /// <summary>
        /// 通过UId获取用户信息
        /// </summary>
        LetterUserEntity LetterUserByUId(long uId);

        /// <summary>
        /// 获取用户账户总金币数量
        /// </summary>
        int UserTotalCoin(long uId);

        /// <summary>
        /// 用户金币奖励和扣除异步操作
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="coinChangeType">改变类别</param>
        /// <param name="remark">备注信息</param>
        /// <param name="changeValue">改变金额（为0则系统取默认配置）</param>
        /// <param name="operateUser">操作人（默认为系统）</param>
        void CoinChangeAsync(long uId, CoinChangeEnum coinChangeType, string remark = "", int changeValue = 0, string operateUser = "");

        /// <summary>
        /// 通过UId获取用户金币明细列表
        /// </summary>
        List<CoinDetailEntity> CoinDetailListByUId(long uId);
    }

    #region Implement
    
    public class UserBiz : IUserBiz
    {
        private readonly LetterRepository letterDal = SingletonProvider<LetterRepository>.Instance;

        public void CoinChangeAsync(long uId, CoinChangeEnum coinChangeType, string remark = "", int changeValue = 0, string operateUser = "")
        {
            Task.Factory.StartNew(() =>
            {
                OnCoinChange(uId, coinChangeType, remark, changeValue, operateUser);

                LogHelper.Info("CoinChangeAsync", "用户金币变动", new Dictionary<string, string>()
                {
                    { "uId",uId.ToString()},
                    { "coinChangeType",coinChangeType.ToString()},
                    { "changeValue",changeValue.ToString()},
                    { "remark",remark.ToString()}
                });
            });
        }
        
        public List<CoinDetailEntity> CoinDetailListByUId(long uId)
        {
            int pageSize = 200;
            string coinPageSize = JsonSettingHelper.AppSettings["CoinDetailListSize"];
            if (!coinPageSize.IsNullOrEmpty())
            {
                pageSize = Convert.ToInt32(coinPageSize);
            }

            return letterDal.GetCoinDetailListByUId(uId,1, pageSize);
        }
        
        public LetterUserEntity LetterUserByUId(long uId)
        {
            if (uId <= 0)
            {
                return null;
            }
            return letterDal.LetterUser(uId);
        }

        public int UserTotalCoin(long uId)
        {
            var coin = letterDal.GetCoinByUId(uId);
            if (coin != null)
            {
                return coin.TotalCoin;
            }
            return 0;
        }

        private bool OnCoinChange(long uId, CoinChangeEnum coinChangeType, string remark = "", int changeValue = 0, string operateUser = "")
        {
            var coin = letterDal.GetCoinByUId(uId);
            if (coin == null)
            {
                coin = new CoinEntity()
                {
                    UId = uId,
                    TotalCoin = 0,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now               
                };
                bool success = letterDal.InsertCoin(coin);
                if (success)
                {
                    coin.CoinId = letterDal.GetCoinByUId(uId).CoinId;
                }
            }

            if (changeValue <= 0)
            {
                string config = "";
                switch (coinChangeType)
                {
                    case CoinChangeEnum.PublishReward:
                        config = "PublishRewardValue";
                        break;
                    case CoinChangeEnum.CollectedReward:
                        config = "CollectedRewardValue";
                        break;
                    case CoinChangeEnum.SignReward:
                        config = "SignRewardValue";
                        break;
                    case CoinChangeEnum.ShareReward:
                        config = "ShareRewardValue";
                        break;
                    case CoinChangeEnum.ActivityReward:
                        config = "ActivityRewardValue";
                        break;
                    case CoinChangeEnum.FirstLoginReward:
                        config = "FirstLoginRewardValue";
                        break;
                    case CoinChangeEnum.PickUpDeducted:
                        config = "PickUpDeductedValue";
                        break;
                    case CoinChangeEnum.ReportedDeducted:
                        config = "ReportedDeductedValue";
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrWhiteSpace(config))
                {
                    string configStr = JsonSettingHelper.AppSettings[config];
                    if (!string.IsNullOrEmpty(configStr))
                    {
                        changeValue = Convert.ToInt16(configStr);
                    }
                }
            }
            if (changeValue != 0)
            {
                //金币余额为0时不再继续扣除
                if (UserTotalCoin(uId) <= 0&& changeValue<0)
                {
                    return true;
                }
                var coinDetail = new CoinDetailEntity()
                {
                    CoinDetailId = Guid.NewGuid(),
                    UId = uId,
                    CoinId = coin.CoinId,
                    CoinChangeType = coinChangeType,
                    OperateUser = string.IsNullOrWhiteSpace(operateUser) ? "system" : operateUser,
                    Remark = remark,
                    ChangeValue = changeValue,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                bool insertCoinDetailSuccess = letterDal.InsertCoinDetail(coinDetail);
                if (insertCoinDetailSuccess)
                {
                    return letterDal.UpdateUserTotalCoin(coin.CoinId, coin.UId, changeValue);
                }
            }
            return true;
        }
    }

    #endregion

}
