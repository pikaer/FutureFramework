﻿using Future.CommonBiz;
using Future.Model.Entity.Bingo;
using Future.Model.Entity.Hubs;
using Future.Model.Enum.Bingo;
using Future.Model.Utils;
using Future.Repository;
using Future.Service.Interface;
using Future.Utility;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Future.Service.Implement
{
    public class UserBiz : IUserBiz
    {
        private readonly BingoRepository letterDal = SingletonProvider<BingoRepository>.Instance;

        public void CoinChangeAsync(long uId, CoinChangeEnum coinChangeType, string remark = "", int changeValue = 0, string operateUser = "")
        {
            Task.Factory.StartNew(() =>
            {
                CoinChange(uId, coinChangeType, remark, changeValue, operateUser);
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

            return letterDal.GetCoinDetailListByUId(uId, 1, pageSize);
        }

        public UserInfoEntity LetterUserByUId(long uId)
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

        public bool CoinChange(long uId, CoinChangeEnum coinChangeType, string remark = "", int changeValue = 0, string operateUser = "")
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
                if (UserTotalCoin(uId) <= 0 && changeValue < 0)
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

            LogHelper.Info("CoinChangeAsync", "用户金币变动", new Dictionary<string, string>()
            {
                { "uId",uId.ToString()},
                { "coinChangeType",coinChangeType.ToString()},
                { "changeValue",changeValue.ToString()},
                { "remark",remark.ToString()}
            });
            return true;
        }

        public OnLineUserHubEntity OnLineUser(long uId)
        {
            return letterDal.GetOnLineUser(uId);
        }

        public void InsertOnLineUserAsync(OnLineUserHubEntity onLineUser)
        {
            Task.Factory.StartNew(() =>
            {
                letterDal.InsertOnLineUser(onLineUser);
            });
        }

        public void UpdateOnLineUserAsync(OnLineUserHubEntity onLineUser)
        {
            Task.Factory.StartNew(() =>
            {
                letterDal.UpdateOnLineUser(onLineUser);
            });
        }

        public ChatListHubEntity ChatListHub(long uId)
        {
            return letterDal.ChatListHub(uId);
        }

        public void InsertChatListHubAsync(ChatListHubEntity chatListHub)
        {
            Task.Factory.StartNew(() =>
            {
                letterDal.InsertChatListHub(chatListHub);
            });
        }

        public void UpdateChatListHubAsync(ChatListHubEntity chatListHub)
        {
            Task.Factory.StartNew(() =>
            {
                letterDal.UpdateChatListHub(chatListHub);
            });
        }

        public OnChatHubEntity OnChatHub(long uId)
        {
            return letterDal.OnChatHub(uId);
        }

        public void InsertOnChatHubAsync(OnChatHubEntity userHub)
        {
            Task.Factory.StartNew(() =>
            {
                letterDal.InsertOnChatHub(userHub);
            });
        }

        public void UpdateOnChatHubAsync(OnChatHubEntity userHub)
        {
            Task.Factory.StartNew(() =>
            {
                letterDal.UpdateOnChatHub(userHub);
            });
        }

        public void SendMomentDiscussNotify(Guid momentId,string discussContent)
        {
            Task.Factory.StartNew(() =>
            {
                var moment = letterDal.GetMoment(momentId);
                if (moment == null)
                {
                    return;
                }
                var userInfo = LetterUserByUId(moment.UId);
                if (userInfo == null)
                {
                    return;
                }
                var onlineInfo = OnLineUser(moment.UId);
                if(onlineInfo==null|| !onlineInfo.IsOnLine)
                {
                    string title = moment.TextContent;
                    if (string.IsNullOrEmpty(title))
                    {
                        //2016年8月2日 20:23
                        title = string.Format("{0}发布的图片动态", moment.CreateTime.ToString("f"));
                    }
                    MiniAppFactory.Factory(userInfo.Platform).SendMomentDiscussNotify(userInfo.OpenId, title, discussContent);
                }
            });
        }

        public void SendDiscussReplyNotify(Guid momentId, long partnerUid, string discussContent)
        {
            Task.Factory.StartNew(() =>
            {
                var userInfo = LetterUserByUId(partnerUid);
                if (userInfo == null)
                {
                    return;
                }
                var onlineInfo = OnLineUser(userInfo.UId);
                if (onlineInfo == null || !onlineInfo.IsOnLine)
                {
                    var moment= letterDal.GetMoment(momentId);
                    if (moment == null)
                    {
                        return;
                    }
                    string title = moment.TextContent;
                    if (string.IsNullOrEmpty(title))
                    {
                        //2016年8月2日 20:23
                        title = string.Format("{0}发布的图片动态", moment.CreateTime.ToString("f"));
                    }
                    MiniAppFactory.Factory(userInfo.Platform).SendDiscussReplyNotify(userInfo.OpenId, title, discussContent);
                }
            });
        }

        public List<TagType> GetTagList(List<UserTagEntity>userTagList, TagTypeEnum tagType)
        {
            List<string> defaultTagList = null;
            switch (tagType)
            {
                case TagTypeEnum.个性标签:
                    defaultTagList = ConstUtil.CHARACTER_TAG_LIST;
                    break;
                case TagTypeEnum.运动标签:
                    defaultTagList = ConstUtil.SPORT_TAG_LIST;
                    break;
                case TagTypeEnum.音乐标签:
                    defaultTagList = ConstUtil.MUSIC_TAG_LIST;
                    break;
                case TagTypeEnum.食物标签:
                    defaultTagList = ConstUtil.FOOD_TAG_LIST;
                    break;
                case TagTypeEnum.电影标签:
                    defaultTagList = ConstUtil.MOVIE_TAG_LIST;
                    break;
                case TagTypeEnum.旅行标签:
                    defaultTagList = ConstUtil.TRAVEL_TAG_LIST;
                    break;
                default:
                    return null;
            }
            var rtnList = new List<TagType>();
            if (userTagList.NotEmpty())
            {
                var selectedTagList = userTagList.Where(a => a.TagType == tagType).ToList();
                if (selectedTagList.NotEmpty())
                {
                    foreach(var item in selectedTagList.OrderByDescending(a => a.CreateTime))
                    {
                        rtnList.Add(new TagType
                        {
                            Tag = item.Tag,
                            Checked = true
                        });
                    }
                    AddDefaultTag(defaultTagList, rtnList);
                }
                else
                {
                    AddDefaultTag(defaultTagList, rtnList);
                }
            }
            else
            {
                AddDefaultTag(defaultTagList, rtnList);
            }
            return rtnList;
        }

        private void AddDefaultTag(List<string> defaultTagList, List<TagType> tagList)
        {
            foreach (var item in defaultTagList)
            {
                if (tagList.Any(a => a.Tag.Equals(item)))
                {
                    continue;
                }
                else
                {
                    tagList.Add(new TagType
                    {
                        Tag = item,
                        Checked = false
                    });
                }
            }
        }
    }
}
