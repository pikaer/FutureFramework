﻿using Future.Model.Entity.Letter;
using Future.Model.Enum.Letter;
using System.Collections.Generic;

namespace Future.Service.Interface
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
        /// 用户金币奖励和扣除同步操作
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="coinChangeType">改变类别</param>
        /// <param name="remark">备注信息</param>
        /// <param name="changeValue">改变金额（为0则系统取默认配置）</param>
        /// <param name="operateUser">操作人（默认为系统）</param>
        bool CoinChange(long uId, CoinChangeEnum coinChangeType, string remark = "", int changeValue = 0, string operateUser = "");

        /// <summary>
        /// 通过UId获取用户金币明细列表
        /// </summary>
        List<CoinDetailEntity> CoinDetailListByUId(long uId);
    }
}