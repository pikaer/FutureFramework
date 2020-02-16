using Future.CommonBiz;
using Future.Model.DTO.Letter;
using Future.Model.Entity.Hubs;
using Future.Model.Entity.Letter;
using Future.Model.Enum.Letter;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Repository;
using Future.Service.Interface;
using Future.Utility;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Future.Service.Implement
{
    public class BottleBiz: IBottleBiz
    {
        #region public Method

        private readonly LetterRepository letterDal = SingletonProvider<LetterRepository>.Instance;

        private readonly IUserBiz userBiz = SingletonProvider<UserBiz>.Instance;

        public ResponseContext<DiscussListResponse> DiscussList(RequestContext<DiscussListRequest> request)
        {
            var response = new ResponseContext<DiscussListResponse>()
            {
                Content = new DiscussListResponse()
                {
                    DiscussList=new List<DiscussType>()
                }
            };

            int pageSize = 20;
            string pickUpPageSize = JsonSettingHelper.AppSettings["PickUpPageSize"];
            if (!pickUpPageSize.IsNullOrEmpty())
            {
                pageSize = Convert.ToInt32(pickUpPageSize);
            }

            var pickUpList = letterDal.PickUpDTOs(request.Content.UId, request.Content.PageIndex, pageSize);
            if (pickUpList.NotEmpty())
            {
                var userOnline = letterDal.GetOnLineUser(request.Content.UId);
                foreach (var item in pickUpList)
                {
                    var dto = new DiscussType()
                    {
                        PickUpId = item.PickUpId,
                        UId= item.UId,
                        MomentId = item.MomentId,
                        IsHide=item.IsHide,
                        Gender=item.Gender,
                        OnLineDesc= item.LastOnLineTime.GetOnlineDesc(item.IsOnLine),
                        DistanceDesc = LocationHelper.GetDistanceDesc(item.Latitude, item.Longitude, userOnline != null ? userOnline.Latitude : 0, userOnline != null ? userOnline.Longitude : 0),
                        HeadImgPath = item.HeadPhotoPath.GetImgPath(),
                        NickName = item.NickName,
                        TextContent = item.TextContent.TextCut(15),
                        UnReadCount=UnReadCount(item.PickUpId,request.Content.UId),
                        RecentChatTime = item.CreateTime.GetDateDesc()
                    };

                    if (item.IsHide)
                    {
                        dto.NickName = item.HidingNickName;
                        dto.ShortNickName = item.HidingNickName.Substring(0, 1);
                    }
                    else
                    {
                        dto.NickName = item.NickName;
                    }

                    response.Content.DiscussList.Add(dto);
                }
                response.Content.CurrentTotalUnReadCount= UnReadTotalCount(request.Content.UId);
            }
            return response;
        }
        
        public ResponseContext<DiscussDetailResponse> DiscussDetail(RequestContext<DiscussDetailRequest> request)
        {
            var response = new ResponseContext<DiscussDetailResponse>();

            MomentEntity moment;
            PickUpEntity pickUp;
            if (request.Content.PickUpId != null && request.Content.PickUpId != Guid.Empty)
            {
                pickUp = letterDal.PickUp(request.Content.PickUpId.Value);
                moment = letterDal.GetMoment(pickUp.MomentId);
            }
            else
            {
                moment = letterDal.GetMoment(request.Content.MomentId.Value);
                pickUp = letterDal.PickUpByMomentId(request.Content.MomentId.Value, request.Content.UId);
                if (pickUp == null)
                {
                    pickUp = new PickUpEntity()
                    {
                        PickUpId = Guid.NewGuid(),
                        MomentId = moment.MomentId,
                        MomentUId = moment.UId,
                        PickUpUId = request.Content.UId,
                        FromPage = PickUpFromPageEnum.AttentionPage,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };
                    letterDal.InsertPickUp(pickUp);
                }
            }
            if (moment == null)
            {
                return response;
            }

            var momentUser= userBiz.LetterUserByUId(moment.UId);
            var resuestUser = userBiz.LetterUserByUId(request.Content.UId);
            string nickName = GetHidingNickName(request.Content.UId, moment, pickUp, resuestUser);
            response.Content = new DiscussDetailResponse()
            {
                PickUpId = pickUp.PickUpId,
                MomentDetail =new MomentDetailType()
                {
                    MomentId = moment.MomentId,
                    IsMyMoment = moment.UId == request.Content.UId,
                    MomentUId = moment.UId,
                    HeadImgPath = momentUser.HeadPhotoPath.GetImgPath(),
                    NickName = momentUser.NickName.Trim().TextCut(15),
                    Gender = momentUser.Gender,
                    TextContent = moment.TextContent.Trim(),
                    ImgContent = moment.ImgContent.IsNullOrEmpty() ? "" : moment.ImgContent.Trim().GetImgPath(),
                    CreateTime = moment.CreateTime.GetDateDesc(true),
                },
                MyDetail=new MyDetailType() 
                { 
                    Gender = resuestUser.Gender,
                    HeadImgPath= resuestUser.HeadPhotoPath.GetImgPath(),
                    IsHide = moment.UId==request.Content.UId?moment.IsHide:pickUp.IsHide,
                    NickName= nickName,
                    ShortNickName=nickName.Substring(0,1)
                }
            };
            var userOnline = letterDal.GetOnLineUser(request.Content.UId);
            if (request.Content.UId == moment.UId)
            {
                var partnerUser = userBiz.LetterUserByUId(pickUp.PickUpUId);
                var partnerOnline = userBiz.OnLineUser(pickUp.PickUpUId);
                response.Content.PartnerDetail = new PartnerDetailType()
                {
                    PartnerUId = partnerUser.UId,
                    Gender = partnerUser.Gender,
                    IsHide = pickUp.IsHide,
                    NickName = pickUp.IsHide ? pickUp.HidingNickName : partnerUser.NickName,
                    ShortNickName = pickUp.IsHide ? pickUp.HidingNickName.Substring(0, 1) : "",
                    HeadImgPath = partnerUser.HeadPhotoPath.GetImgPath(),
                    OnLineDesc = partnerOnline.LastOnLineTime.GetOnlineDesc(partnerOnline.IsOnLine),
                    DistanceDesc = LocationHelper.GetDistanceDesc(userOnline.Latitude, userOnline.Longitude, partnerOnline.Latitude, partnerOnline.Longitude),
                };
            }
            else
            {
                var partnerUser = userBiz.LetterUserByUId(moment.UId);
                var partnerOnline = userBiz.OnLineUser(moment.UId);
                response.Content.PartnerDetail = new PartnerDetailType()
                {
                    PartnerUId = partnerUser.UId,
                    Gender = partnerUser.Gender,
                    IsHide = moment.IsHide,
                    NickName = moment.IsHide ? moment.HidingNickName : partnerUser.NickName,
                    ShortNickName = moment.IsHide ? moment.HidingNickName.Substring(0, 1) : "",
                    HeadImgPath = partnerUser.HeadPhotoPath.GetImgPath(),
                    OnLineDesc = partnerOnline.LastOnLineTime.GetOnlineDesc(partnerOnline.IsOnLine),
                    DistanceDesc = LocationHelper.GetDistanceDesc(userOnline.Latitude, userOnline.Longitude, partnerOnline.Latitude, partnerOnline.Longitude),
                };
            }

            DateTime? deleteTime;
            if (moment.UId == request.Head.UId)
            {
                deleteTime = pickUp.UserLastDeleteTime;
            }
            else
            {
                deleteTime = pickUp.PartnerLastDeleteTime;
            }

            var discussList = letterDal.DiscussList(pickUp.PickUpId, deleteTime);
            if (discussList.IsNullOrEmpty()&&moment.UId != request.Content.UId)
            {
                if (pickUp.IsHide)
                {
                    response.Content.MyDetail.ShowHideStatus = 2;
                }
                else
                {
                    response.Content.MyDetail.ShowHideStatus = 1;
                }
            }
            return response;
        }

        private string GetHidingNickName(long uid,MomentEntity moment, PickUpEntity pickUp,LetterUserEntity user)
        {
            if (moment.UId == uid)
            {
                if (moment.IsHide)
                {
                    return moment.HidingNickName;
                }
                else
                {
                    return user.NickName;
                }
            }
            else
            {
                if (pickUp.IsHide)
                {
                    return pickUp.HidingNickName;
                }
                else
                {
                    return user.NickName;
                }
            }
        }

        private PickUpDTO BuildPickUpDTO(long uid, MomentEntity moment, PickUpEntity pickUp)
        {
            var user = userBiz.LetterUserByUId(uid);
            var online = userBiz.OnLineUser(uid);
            string nickName = GetHidingNickName(uid, moment, pickUp, user);
            var dto= new PickUpDTO()
            {
                NickName = nickName,
                UId = uid,
                IsHide = moment.UId == uid ? moment.IsHide : pickUp.IsHide,
                HidingNickName = moment.UId == uid ? moment.HidingNickName : pickUp.HidingNickName,
                HeadPhotoPath = user.HeadPhotoPath,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                IsOnLine = online.IsOnLine,
                LastOnLineTime = online.LastOnLineTime,
                Latitude = online.Latitude,
                Longitude = online.Longitude,
            };
            return dto;
        }

        public ResponseContext<DiscussResponse> Discuss(RequestContext<DiscussRequest> request)
        {
            var response = new ResponseContext<DiscussResponse>()
            {
                Content = new DiscussResponse()
            };
            var moment = letterDal.GetMoment(request.Content.MomentId);
            PickUpEntity pickUp;
            if(request.Content.PickUpId!=null&& request.Content.PickUpId != Guid.Empty)
            {
                pickUp = letterDal.PickUp(request.Content.PickUpId);
            }
            else
            {
                pickUp = letterDal.PickUpByMomentId(request.Content.MomentId, request.Content.UId);
                if (pickUp == null)
                {
                    pickUp = new PickUpEntity() 
                    {
                        PickUpId=Guid.NewGuid(),
                        MomentId= moment.MomentId,
                        MomentUId= moment.UId,
                        PickUpUId=request.Content.UId,
                        FromPage= PickUpFromPageEnum.AttentionPage,
                        CreateTime=DateTime.Now,
                        UpdateTime=DateTime.Now
                    };
                    letterDal.InsertPickUp(pickUp);
                }
            }

            if(pickUp.MomentUId== request.Head.UId)
            {
                letterDal.UpdatePickUpUserDelete(pickUp.PickUpId);
            }
            else
            {
                letterDal.UpdatePickUpPartnerDelete(pickUp.PickUpId);
            }
            var discuss = new DiscussEntity()
            {
                DiscussId = Guid.NewGuid(),
                PickUpId = pickUp.PickUpId,
                UId = request.Content.UId,
                DiscussContent = request.Content.TextContent,
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now
            };
            response.Content.IsExecuteSuccess= letterDal.InsertDiscuss(discuss);

            if (pickUp.MomentUId != request.Head.UId)
            {
                //通知动态发布者，有人评论了他
                userBiz.SendMomentDiscussNotify(pickUp.MomentId, request.Content.TextContent);
            }
            else
            {
                //通知评论者，发布动态的人回复了他
                userBiz.SendDiscussReplyNotify(pickUp.MomentId,pickUp.PickUpUId, request.Content.TextContent);
            }
            return response;
        }

        public ResponseContext<PickUpListResponse> PickUpList(RequestContext<PickUpListRequest> request)
        {
            var response = new ResponseContext<PickUpListResponse>()
            {
                Content = new PickUpListResponse()
                {
                    PickUpList=new List<PickUpType>()
                }
            };

            int pageSize = 20;
            string pickUpPageSize = JsonSettingHelper.AppSettings["MomentPageSize"];
            if (!pickUpPageSize.IsNullOrEmpty())
            {
                pageSize = Convert.ToInt32(pickUpPageSize);
            }
            var pickUpList = letterDal.PickUpListByPageIndex(request.Content.UId,request.Content.PageIndex, pageSize, MomentSourceEnum.Default);
            if (pickUpList.NotEmpty())
            {
                var userOnline = letterDal.GetOnLineUser(request.Content.UId);
                foreach (var item in pickUpList)
                {
                    DateTime? datetime = null;
                    bool isonline = false;
                    var online = userBiz.OnLineUser(item.UId);
                    if (online != null)
                    {
                        datetime = online.LastOnLineTime;
                        isonline = online.IsOnLine;
                    }
                    var dto = new PickUpType()
                    {
                        IsMyMoment=request.Content.UId== item.MomentUId,
                        PickUpId = item.PickUpId,
                        MomentId= item.MomentId,
                        UId = item.UId,
                        OnLineDesc = datetime.GetOnlineDesc(isonline),
                        Gender =item.Gender,
                        Age= item.BirthDate.IsNullOrEmpty()?18:Convert.ToDateTime(item.BirthDate).GetAgeByBirthdate(),
                        HeadImgPath = item.HeadPhotoPath.GetImgPath(),
                        IsHide=item.IsHide,
                        TextContent = item.TextContent,
                        ImgContent = item.ImgContent.GetImgPath(),
                        DistanceDesc = LocationHelper.GetDistanceDesc(userOnline.Latitude, userOnline.Longitude, online != null ? online.Latitude : 0, online != null ? online.Longitude : 0),
                        CreateTime = item.CreateTime.GetDateDesc(true)
                    };

                    if (item.IsHide)
                    {
                        dto.NickName = CommonHelper.CutNickName(item.HidingNickName, 8);
                    }
                    else
                    {
                        dto.NickName = CommonHelper.CutNickName(item.NickName, 8);
                    }
                    response.Content.PickUpList.Add(dto);
                }
            }
            return response;
        }

        public ResponseContext<PlayTogetherListResponse> PlayTogetherList(RequestContext<PlayTogetherListRequest> request)
        {
            var response = new ResponseContext<PlayTogetherListResponse>()
            {
                Content = new PlayTogetherListResponse()
            };

            int pageSize = 20;
            string pickUpPageSize = JsonSettingHelper.AppSettings["PlayTogetherPageSize"];
            if (!pickUpPageSize.IsNullOrEmpty())
            {
                pageSize = Convert.ToInt32(pickUpPageSize);
            }
            var pickUpList = letterDal.PickUpListByPageIndex(request.Content.UId, 1, pageSize, MomentSourceEnum.PlayTogether);
            if (pickUpList.NotEmpty())
            {
                var recentMomentImgs = RecentImgMomentImgs(pickUpList);
                response.Content.PlayTogetherList_Other=PlayTogetherList(recentMomentImgs,pickUpList, PlayTypeEnum.Other, request.Content.UId);
                response.Content.PlayTogetherList_WangZhe = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.WangZhe, request.Content.UId);
                response.Content.PlayTogetherList_ChiJi = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.ChiJi, request.Content.UId);
                response.Content.PlayTogetherList_LianMai = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.LianMai, request.Content.UId);
                response.Content.PlayTogetherList_Game = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.Game, request.Content.UId);
                response.Content.PlayTogetherList_Learn = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.Learn, request.Content.UId);
                response.Content.PlayTogetherList_TVTracker = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.TVTracker, request.Content.UId);
                response.Content.PlayTogetherList_Earlybird = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.Earlybird, request.Content.UId);
                response.Content.PlayTogetherList_Walk = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.Walk, request.Content.UId);
                response.Content.PlayTogetherList_Movie = PlayTogetherList(recentMomentImgs, pickUpList, PlayTypeEnum.Movie, request.Content.UId);
            }

            Task.Factory.StartNew(() =>
            {
                RefreshPlayTogetherList(request.Content.UId);
            });

            return response;
        }

        /// <summary>
        /// 获取用户最新5个一起玩动态
        /// </summary>
        private Dictionary<long,List<string>> RecentImgMomentImgs(List<PickUpDTO> pickUpList)
        {
            var uidList = new List<long>();
            foreach(var pickDto in pickUpList)
            {
                if (uidList.Contains(pickDto.UId))
                {
                    continue;
                }
                uidList.Add(pickDto.UId);
            }
            var rtnList = new Dictionary<long, List<string>>();
            foreach(long uid in uidList)
            {
                var imgList = new List<string>();
                var momentList = letterDal.GetRecentImgMomentList(uid);
                if (momentList.NotEmpty())
                {
                    foreach(var moment in momentList)
                    {
                        imgList.Add(moment.ImgContent.GetImgPath());
                    }
                }
                rtnList.Add(uid, imgList);
            }
            return rtnList;
        }


        private List<PlayTogetherType> PlayTogetherList(Dictionary<long,List<string>> imgList, List<PickUpDTO> pickUpList, PlayTypeEnum playType,long uid)
        {
            List<PickUpDTO> pickUps = null;
            if (playType != PlayTypeEnum.Other)
            {
                pickUps = pickUpList.Where(a => a.PlayType == playType).ToList();
            }
            else
            {
                pickUps = pickUpList;
            }
            if (pickUps.IsNullOrEmpty())
            {
                return null;
            }
            var rtnList = new List<PlayTogetherType>();
            var userOnline = letterDal.GetOnLineUser(uid);
            foreach (var item in pickUps)
            {
                DateTime? datetime = null;
                bool isonline = false;
                var online = userBiz.OnLineUser(item.UId);
                if (online != null)
                {
                    datetime = online.LastOnLineTime;
                    isonline = online.IsOnLine;
                }
                var dto = new PlayTogetherType()
                {
                    IsMyMoment = uid == item.MomentUId,
                    PickUpId = item.PickUpId,
                    MomentId = item.MomentId,
                    UId = item.UId,
                    OnLineDesc = datetime.GetOnlineDesc(isonline),
                    Gender = item.Gender,
                    Age = item.BirthDate.IsNullOrEmpty() ? 18 : Convert.ToDateTime(item.BirthDate).GetAgeByBirthdate(),
                    HeadImgPath = item.HeadPhotoPath.GetImgPath(),
                    IsHide = item.IsHide,
                    TextContent = item.TextContent,
                    ImgContent = item.ImgContent.GetImgPath(),
                    DistanceDesc = LocationHelper.GetDistanceDesc(userOnline.Latitude, userOnline.Longitude, online != null ? online.Latitude : 0, online != null ? online.Longitude : 0),
                    CreateTime = item.CreateTime.GetDateDesc(true),
                    PlayType=playType,
                    PlayTypeSesc=playType.ToDescription(),
                    AgeYear=Convert.ToDateTime(item.BirthDate).GetAgeYear(),
                    Constellation=Convert.ToDateTime(item.BirthDate).GetConstellation(),
                    RecentPlayMomentImgs= imgList[item.UId]
                };

                if (item.IsHide)
                {
                    dto.NickName = CommonHelper.CutNickName(item.HidingNickName, 8);
                }
                else
                {
                    dto.NickName = CommonHelper.CutNickName(item.NickName, 8);
                }
                rtnList.Add(dto);
            }
            return rtnList;
        }

        public ResponseContext<AttentionListResponse> AttentionList(RequestContext<AttentionListRequest> request)
        {
            var response = new ResponseContext<AttentionListResponse>()
            {
                Content = new AttentionListResponse()
                {
                    AttentionList = new List<PickUpType>()
                }
            };

            int pageSize = 20;
            string pickUpPageSize = JsonSettingHelper.AppSettings["MomentPageSize"];
            if (!pickUpPageSize.IsNullOrEmpty())
            {
                pageSize = Convert.ToInt32(pickUpPageSize);
            }
            var pickUpList = letterDal.AttentionListByPageIndex(request.Content.UId, request.Content.PageIndex, pageSize);
            if (pickUpList.NotEmpty())
            {
                var userOnline = letterDal.GetOnLineUser(request.Content.UId);
                if (userOnline != null)
                {
                    foreach (var item in pickUpList)
                    {
                        DateTime? datetime = null;
                        bool isonline = false;
                        var online = userBiz.OnLineUser(item.UId);
                        if (online != null)
                        {
                            datetime = online.LastOnLineTime;
                            isonline = online.IsOnLine;
                        }
                        var dto = new PickUpType()
                        {
                            MomentId = item.MomentId,
                            UId = item.UId,
                            OnLineDesc = datetime.GetOnlineDesc(isonline),
                            Gender = item.Gender,
                            Age = item.BirthDate.IsNullOrEmpty() ? 18 : Convert.ToDateTime(item.BirthDate).GetAgeByBirthdate(),
                            HeadImgPath = item.HeadPhotoPath.GetImgPath(),
                            NickName = CommonHelper.CutNickName(item.NickName,8),
                            TextContent = item.TextContent,
                            ImgContent = item.ImgContent.GetImgPath(),
                            CreateTime = item.CreateTime.GetDateDesc(true),
                            DistanceDesc = LocationHelper.GetDistanceDesc(userOnline.Latitude, userOnline.Longitude, online!=null?online.Latitude:0, online != null ? online.Longitude:0)
                        };
                        response.Content.AttentionList.Add(dto);
                    }
                }
            }

            return response;
        }

        public ResponseContext<PickUpResponse> PickUp(RequestContext<PickUpRequest> request)
        {
            var response = new ResponseContext<PickUpResponse>()
            {
                Content = new PickUpResponse()
                {
                    PickUpList = new List<PickUpType>()
                }
            };
            var user= userBiz.LetterUserByUId(request.Content.UId);
            if (user == null)
            {
                return response;
            }
            if (userBiz.UserTotalCoin(request.Content.UId) <= 0)
            {
                return new ResponseContext<PickUpResponse>(false, ErrCodeEnum.CoinEmpty, null, "金币余额不足，快去发布动态赚金币吧");
            }
            int pickUpCount;
            switch (user.Gender)
            {
                case GenderEnum.Woman:
                    pickUpCount = Convert.ToInt16(JsonSettingHelper.AppSettings["ManBottlePickUpCount"]);
                    break;
                case GenderEnum.Man:
                    pickUpCount = Convert.ToInt16(JsonSettingHelper.AppSettings["WomanBottlePickUpCount"]);
                    break;
                default:
                    pickUpCount = Convert.ToInt16(JsonSettingHelper.AppSettings["DefaultBottlePickUpCount"]);
                    break;
            }
            var moment= letterDal.GetMoment(request.Content.UId, pickUpCount, user.Gender,request.Content.MomentType, MomentSourceEnum.Default);
            if (moment == null)
            {
                return response;
            }

            var pickUp = new PickUpEntity()
            {
                PickUpId = Guid.NewGuid(),
                MomentId = moment.MomentId,
                MomentUId = moment.UId,
                PickUpUId = request.Content.UId,
                CreateTime=DateTime.Now,
                UpdateTime= DateTime.Now
            };
            bool success=letterDal.InsertPickUp(pickUp);
            if (success)
            {
                letterDal.UpdatePickCount(moment.MomentId);
                var letterUser= userBiz.LetterUserByUId(moment.UId);
                if (letterUser == null)
                {
                    return response;
                }
                if (request.Content.MomentType == MomentTypeEnum.ImgMoment)
                {
                    moment.TextContent = moment.TextContent.TextCut(18);
                }
                var partnerOnline = letterDal.GetOnLineUser(pickUp.MomentUId);
                DateTime? datetime = null;
                bool isonline = false;
                if (partnerOnline != null)
                {
                    datetime = partnerOnline.LastOnLineTime;
                    isonline = partnerOnline.IsOnLine;
                }
                var userOnline = letterDal.GetOnLineUser(request.Content.UId);
                response.Content.PickUpList.Add(new PickUpType()
                {
                    IsMyMoment=false,
                    PickUpId = pickUp.PickUpId,
                    MomentId= pickUp.MomentId,
                    UId = moment.UId,
                    OnLineDesc = datetime.GetOnlineDesc(isonline),
                    HeadImgPath = letterUser.HeadPhotoPath.GetImgPath(),
                    IsHide=moment.IsHide,
                    NickName=moment.IsHide? CommonHelper.CutNickName(moment.HidingNickName, 8): CommonHelper.CutNickName(letterUser.NickName, 8),
                    Age= letterUser.BirthDate.IsNullOrEmpty()?18: Convert.ToDateTime(letterUser.BirthDate).GetAgeByBirthdate(),
                    Gender= letterUser.Gender,
                    TextContent = moment.TextContent.Trim(),
                    ImgContent= moment.ImgContent.GetImgPath(),
                    DistanceDesc = LocationHelper.GetDistanceDesc(userOnline.Latitude, userOnline.Longitude, partnerOnline != null ? partnerOnline.Latitude : 0, partnerOnline != null ? partnerOnline.Longitude : 0),
                    CreateTime = moment.CreateTime.GetDateDesc(true)
                });
                
                userBiz.CoinChangeAsync(request.Content.UId, CoinChangeEnum.PickUpDeducted, "获取动态消耗金币");
            }
            return response;
        }

        private void RefreshPlayTogetherList(long uid)
        {
            var user = userBiz.LetterUserByUId(uid);
            if (user == null)
            {
                return;
            }
            int pickUpCount;
            switch (user.Gender)
            {
                case GenderEnum.Woman:
                    pickUpCount = Convert.ToInt16(JsonSettingHelper.AppSettings["WomanPlayTogetherPickUpCount"]);
                    break;
                case GenderEnum.Man:
                    pickUpCount = Convert.ToInt16(JsonSettingHelper.AppSettings["ManPlayTogetherPickUpCount"]);
                    break;
                default:
                    pickUpCount = Convert.ToInt16(JsonSettingHelper.AppSettings["DefaultPlayTogetherPickUpCount"]);
                    break;
            }
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.WangZhe);
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.ChiJi);
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.LianMai);
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.Game);
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.Learn);
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.TVTracker);
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.Earlybird);
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.Walk);
            PickUpPlayTogetherMoment(user, pickUpCount, PlayTypeEnum.Movie);
        }

        private void PickUpPlayTogetherMoment(LetterUserEntity userInfo,int pickUpCount, PlayTypeEnum playType)
        {
            var momentList = letterDal.GetPlayMoments(userInfo.UId, pickUpCount, userInfo.Gender, playType);
            if (momentList.IsNullOrEmpty())
            {
                return;
            }
            foreach(var item in momentList)
            {
                var pickUp = new PickUpEntity()
                {
                    PickUpId = Guid.NewGuid(),
                    MomentId = item.MomentId,
                    MomentUId = item.UId,
                    PickUpUId = userInfo.UId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                bool success = letterDal.InsertPickUp(pickUp);
                if (success)
                {
                    letterDal.UpdatePickCount(item.MomentId);
                }
            }
        }

        public ResponseContext<PublishMomentResponse> PublishMoment(RequestContext<PublishMomentRequest> request)
        {
            var response = new ResponseContext<PublishMomentResponse>()
            {
                Content = new PublishMomentResponse()
            };

            var moment = new MomentEntity()
            {
                MomentId = Guid.NewGuid(),
                UId = request.Content.UId,
                TextContent = request.Content.TextContent,
                ImgContent = request.Content.ImgContent,
                SubscribeMessageOpen=request.Content.SubscribeMessageOpen,
                IsHide=request.Content.IsHide,
                HidingNickName=request.Content.HidingNickName,
                SourceFlag=request.Content.SourceFlag,
                PlayType=request.Content.PlayType,
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now
            };

            response.Content.IsExecuteSuccess= letterDal.InsertMoment(moment);
            response.Content.MomentId = moment.MomentId;
            if (response.Content.IsExecuteSuccess)
            {
                userBiz.CoinChangeAsync(request.Content.UId, CoinChangeEnum.PublishReward, "发布动态，奖励金币");

                var pickUp = new PickUpEntity()
                {
                    PickUpId = Guid.NewGuid(),
                    MomentId = moment.MomentId,
                    MomentUId = moment.UId,
                    PickUpUId = moment.UId,
                    FromPage = PickUpFromPageEnum.Default,
                    IsPickUpDelete = false,
                    IsUserDelete = false,
                    IsPartnerDelete = false,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                letterDal.InsertPickUp(pickUp);
            }

            return response;
        }

        public ResponseContext<MyMomentListResponse> MyMomentList(RequestContext<MyMomentListRequest> request)
        {
            var response = new ResponseContext<MyMomentListResponse>()
            {
                Content = new MyMomentListResponse()
                {
                    MomentList = new List<MomentType>()
                }
            };

            int pageSize = 20;
            string pickUpPageSize = JsonSettingHelper.AppSettings["MyMomentListPageSize"];
            if (!pickUpPageSize.IsNullOrEmpty())
            {
                pageSize = Convert.ToInt32(pickUpPageSize);
            }
            var myMomentList = letterDal.GetMomentByPageIndex(request.Content.UId, request.Content.PageIndex, pageSize);
            if (myMomentList.NotEmpty())
            {
                var user = userBiz.LetterUserByUId(request.Content.UId);
                if (user == null)
                {
                    return response;
                }
                response.Content.MomentList = myMomentList.Select(a => new MomentType()
                {
                    MomentId=a.MomentId,
                    NickName=a.IsHide? CommonHelper.CutNickName(a.HidingNickName, 8) : CommonHelper.CutNickName(user.NickName, 8),
                    ShortNickName=a.IsHide?a.HidingNickName.Substring(0,1):"",
                    IsHide=a.IsHide,
                    HeadImgPath=user.HeadPhotoPath.GetImgPath(),
                    Gender=user.Gender,
                    TextContent =a.TextContent.Trim(),
                    ImgContent=a.ImgContent.GetImgPath(),
                    PublishTime=a.CreateTime.GetDateDesc()
                }).ToList();
            }
            return response;
        }

        public ResponseContext<MomentDetailResponse> MomentDetail(RequestContext<MomentDetailRequest> request)
        {
            var response = new ResponseContext<MomentDetailResponse>()
            {
                Content=new MomentDetailResponse()
            };
            var moment = letterDal.GetMoment(request.Content.MomentId);
            if (moment != null)
            {
                var user = userBiz.LetterUserByUId(moment.UId);
                if (user != null)
                {
                    response.Content = new MomentDetailResponse()
                    {
                        MomentId = moment.MomentId,
                        UId = moment.UId,
                        NickName= user.NickName.Trim(),
                        HeadImgPath=user.HeadPhotoPath.GetImgPath(),
                        TextContent = moment.TextContent.Trim(),
                        ImgContent = moment.ImgContent.GetImgPath(),
                        CreateTime = moment.CreateTime.GetDateDesc(true)
                    };
                }
            }
            return response;
        }

        public ResponseContext<BasicUserInfoResponse> UserLogin(RequestContext<UserLoginRequest> request)
        {
            var response = new ResponseContext<BasicUserInfoResponse>();

            var openIdInfo = MiniAppFactory.Factory(request.Head.Platform).GetOpenId(request.Content.LoginCode);
            if(openIdInfo==null|| openIdInfo.OpenId.IsNullOrEmpty())
            {
                LogHelper.Fatal("GetOpenIdInfo", "获取OpenId异常",null, new Dictionary<string, string>()
                {
                    { "LoginCode",request.Content.LoginCode},
                    { "Token",request.Head.Token},
                    { "Platform",request.Head.Token},
                    { "Channel",request.Head.Platform.ToDescription()},
                    { "TransactionId",request.Head.TransactionId.ToString()}
                });
                return response;
            }
            var userInfo = letterDal.LetterUser(0, openIdInfo.OpenId);
            if (userInfo == null)
            {
                userInfo = new LetterUserEntity()
                {
                    OpenId = openIdInfo.OpenId,
                    Gender = 0,
                    Country = "全部",
                    Province = "全部",
                    City = "全部",
                    Area= "全部",
                    Signature = "却道天凉好个秋~",
                    Platform = request.Head.Platform,
                    BirthDate = "2000-01-01",
                    EntranceDate = "2000-07-01",
                    LastLoginTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                bool success = letterDal.InsertLetterUser(userInfo);
                if (success)
                {
                    userInfo = letterDal.LetterUser(0, openIdInfo.OpenId);
                    userBiz.CoinChange(userInfo.UId, CoinChangeEnum.FirstLoginReward, "新注册用户赠送金币");
                }
            }

            if (userInfo == null)
            {
                LogHelper.Fatal("GetUserInfo", "获取OpenId异常", null, new Dictionary<string, string>()
                {
                    { "LoginCode",request.Content.LoginCode},
                    { "Token",request.Head.Token},
                    { "Platform",request.Head.Token},
                    { "Channel",request.Head.Platform.ToDescription()},
                    { "TransactionId",request.Head.TransactionId.ToString()}
                });
                return response;
            }
            else
            {
                UpdateLastLoginTime(userInfo);
            }
            response.Content = new BasicUserInfoResponse
            {
                UId = userInfo.UId,
                Gender = userInfo.Gender,
                NickName = userInfo.NickName,
                Constellation = Convert.ToDateTime(userInfo.BirthDate).GetConstellation(),
                AgeYear = Convert.ToDateTime(userInfo.BirthDate).GetAgeYear(),
                HeadPhotoPath = userInfo.HeadPhotoPath.GetImgPath(),
                BasicUserInfo = BasicUserInfo(userInfo).TextCut(15),
                PlaceInfo = PlaceInfo(userInfo),
                Signature = userInfo.Signature,
                IsRegister= userInfo.IsRegister,
                TotalCoin = userBiz.UserTotalCoin(userInfo.UId)
            };
            return response;
        }

        public ResponseContext<DeleteBottleResponse> DeleteBottle(RequestContext<DeleteBottleRequest> request)
        {
            var response = new ResponseContext<DeleteBottleResponse>()
            {
                Content = new DeleteBottleResponse()
            };

            var pickUp = letterDal.PickUp(request.Content.PickUpId);
            if (pickUp == null)
            {
                return response;
            }
            var updateTime = DateTime.Now;
            if (pickUp.MomentUId == request.Content.UId)
            {
                letterDal.UpdatePickDeleteTime(pickUp.PickUpId, updateTime, true);
            }
            else
            {
                if (request.Content.DeleteType == 1)
                {
                    letterDal.UpdatePickDeleteTime(pickUp.PickUpId, updateTime, false);
                }
                else
                {
                    letterDal.UpdatePickDelete(pickUp.PickUpId);
                }
            }
            letterDal.UpdateHasRead(pickUp.PickUpId, request.Content.UId, updateTime);
            response.Content.IsExecuteSuccess = true;
            response.Content.CurrentTotalUnReadCount = UnReadTotalCount(request.Content.UId);
            return response;
        }

        public ResponseContext<ReportBottleResponse> ReportBottle(RequestContext<ReportBottleRequest> request)
        {
            var response = new ResponseContext<ReportBottleResponse>()
            {
                Content = new ReportBottleResponse()
            };
            var pickUp= letterDal.PickUp(request.Content.PickUpId);
            if (pickUp == null)
            {
                return response;
            }

            var moment = letterDal.GetMoment(pickUp.MomentId);
            if (moment != null)
            {
                response.Content.IsExecuteSuccess= letterDal.UpdatePickUpReport(pickUp.MomentId);
                if (response.Content.IsExecuteSuccess)
                {
                    userBiz.CoinChangeAsync(moment.UId, CoinChangeEnum.ReportedDeducted, "动态被举报，扣除金币");
                }
            }
            return response;
        }

        public ResponseContext<ClearAllBottleResponse> ClearAllBottle(RequestContext<ClearAllBottleRequest> request)
        {
            return new ResponseContext<ClearAllBottleResponse>()
            {
                Content = new ClearAllBottleResponse()
                {
                    IsExecuteSuccess = letterDal.UpdateAllPickDeleteByUId(request.Content.UId)
                }
            };
        }

        public ResponseContext<ClearUnReadCountResponse> ClearUnReadCount(RequestContext<ClearUnReadCountRequest> request)
        {
            var response = new ResponseContext<ClearUnReadCountResponse>()
            {
                Content = new ClearUnReadCountResponse()
            };

            letterDal.UpdateHasRead(request.Content.PickUpId,request.Content.UId);
            response.Content.IsExecuteSuccess = true;
            response.Content.CurrentTotalUnReadCount = UnReadTotalCount(request.Content.UId);
            return response;
        }

        public ResponseContext<UnReadTotalCountResponse> UnReadTotalCount(RequestContext<UnReadTotalCountRequest> request)
        {
            var response = new ResponseContext<UnReadTotalCountResponse>()
            {
                Content = new UnReadTotalCountResponse()
            };
            
            response.Content.UnReadCount = UnReadTotalCount(request.Content.UId);

            return response;
        }

        public ResponseContext<DeleteAllBottleResponse> DeleteAllBottle(RequestContext<DeleteAllBottleRequest> request)
        {
            var response = new ResponseContext<DeleteAllBottleResponse>()
            {
                Content = new DeleteAllBottleResponse()
            };

            var myPickUpList= letterDal.PickUpListByPickUpUId(request.Content.UId);
            if (myPickUpList.NotEmpty())
            {
                foreach(var item in myPickUpList)
                {
                    //当瓶子发布者删除的时候，直接清空评论数据
                    if (item.IsUserDelete)
                    {
                        letterDal.DeleteDiscuss(item.PickUpId);
                    }

                    letterDal.DeleteAllPickBottle(item.PickUpId);
                }
            }

            var partnerPickUpList = letterDal.PickUpListByMomentUId(request.Content.UId);
            if (partnerPickUpList.NotEmpty())
            {
                foreach (var item in partnerPickUpList)
                {
                    if (item.IsPartnerDelete)
                    {
                        letterDal.DeleteDiscuss(item.PickUpId);
                    }
                    letterDal.DeleteAllPublishBottle(item.PickUpId);
                }
            }
            
            response.Content.IsExecuteSuccess = true;
            return response;
        }

        public ResponseContext<ClearAllUnReadCountResponse> ClearAllUnReadCount(RequestContext<ClearAllUnReadCountRequest> request)
        {
            var response = new ResponseContext<ClearAllUnReadCountResponse>()
            {
                Content = new ClearAllUnReadCountResponse()
            };
            var myPickUpList = letterDal.PickUpListByPickUpUId(request.Content.UId);
            foreach(var item in myPickUpList)
            {
                letterDal.UpdateDiscussHasRead(item.PickUpId, item.MomentUId);
            }

            var partnerPickUpList = letterDal.PickUpListByMomentUId(request.Content.UId);
            foreach (var item in partnerPickUpList)
            {
                letterDal.UpdateDiscussHasRead(item.PickUpId, item.PickUpUId);
            }
            response.Content.IsExecuteSuccess = true;
            return response;
        }

        public ResponseContext<BasicUserInfoResponse> BasicUserInfo(RequestContext<BasicUserInfoRequest> request)
        {
            var response = new ResponseContext<BasicUserInfoResponse>();
            var userInfo = userBiz.LetterUserByUId(request.Content.UId);
            if (userInfo == null)
            {
                return response;
            }
            response.Content = new BasicUserInfoResponse()
            {
                UId= userInfo.UId,
                Gender= userInfo.Gender,
                NickName = userInfo.NickName.IsNullOrEmpty()?"":userInfo.NickName.Trim(),
                IsRegister = userInfo.IsRegister,
                Constellation= Convert.ToDateTime(userInfo.BirthDate).GetConstellation(),
                AgeYear= Convert.ToDateTime(userInfo.BirthDate).GetAgeYear(),
                HeadPhotoPath = userInfo.HeadPhotoPath.GetImgPath(),
                Signature= userInfo.Signature.IsNullOrEmpty()? "却道天凉好个秋~" : userInfo.Signature.Trim(),
                BasicUserInfo= BasicUserInfo(userInfo).TextCut(15),
                PlaceInfo=PlaceInfo(userInfo)
            };
            if (request.Content.Type == 1)
            {
                response.Content.TotalCoin = userBiz.UserTotalCoin(userInfo.UId);
            }
            return response;
        }

        public ResponseContext<UpdateUserInfoResponse> UpdateUserInfo(RequestContext<UpdateUserInfoRequest> request)
        {
            var response = new ResponseContext<UpdateUserInfoResponse>();
            var userEntity = new LetterUserEntity()
            {
                UId = request.Content.UId,
                NickName = request.Content.NickName,
                Gender = request.Content.Gender,
                SchoolType = request.Content.SchoolType,
                LiveState = request.Content.LiveState,
                EntranceDate = request.Content.EntranceDate,
                SchoolName = request.Content.SchoolName,
                BirthDate = request.Content.BirthDate,
                Area = request.Content.Area,
                Province = request.Content.Province,
                City = request.Content.City,
                Signature = request.Content.Signature,
                UpdateTime = DateTime.Now
            };
            response.Content = new UpdateUserInfoResponse()
            {
                IsExecuteSuccess = letterDal.UpdateLetterUser(userEntity)
            };
            return response;
        }

        public ResponseContext<GetUserInfoResponse> GetUserInfo(RequestContext<GetUserInfoRequest> request)
        {
            var response = new ResponseContext<GetUserInfoResponse>();
            var userInfo = userBiz.LetterUserByUId(request.Content.UId);
            if (userInfo == null)
            {
                return response;
            }
            response.Content = new GetUserInfoResponse()
            {
                UId = userInfo.UId,
                Gender = userInfo.Gender,
                SchoolType = userInfo.SchoolType,
                LiveState = userInfo.LiveState,
                EntranceDate = userInfo.EntranceDate.IsNullOrEmpty()? "2000-01-01" : userInfo.EntranceDate,
                SchoolName = userInfo.SchoolName.IsNullOrEmpty()?"": userInfo.SchoolName,
                BirthDate = userInfo.BirthDate.IsNullOrEmpty() ? "2000-01-01" : userInfo.BirthDate,
                Area = userInfo.Area.IsNullOrEmpty() ? "全部" : userInfo.Area,
                Province = userInfo.Province.IsNullOrEmpty() ? "全部" : userInfo.Province,
                City = userInfo.City.IsNullOrEmpty() ? "全部" : userInfo.City,
                NickName = userInfo.NickName.Trim(),
                Signature = userInfo.Signature.IsNullOrEmpty() ? "却道天凉好个秋~" : userInfo.Signature.Trim(),
            };
            return response;
        }

        public ResponseContext<DeleteMomentResponse> DeleteMoment(RequestContext<DeleteMomentRequest> request)
        {
            var moment = letterDal.GetMoment(request.Content.MomentId);
            //物理删除
            if(moment!=null&& moment.ReplyCount == 0)
            {
                if (!string.IsNullOrEmpty(moment.ImgContent))
                {
                    string path = JsonSettingHelper.AppSettings["SetImgPath"] + moment.ImgContent;
                    System.IO.File.Delete(path);
                }
                letterDal.DeleteMoment(request.Content.MomentId);
            }
            return new ResponseContext<DeleteMomentResponse>
            {
                Content = new DeleteMomentResponse()
                {
                    IsExecuteSuccess = letterDal.UpdateMomentDelete(request.Content.MomentId)
                }
            };        
        }

        public ResponseContext<DeleteAllMomentResponse> DeleteAllMoment(RequestContext<DeleteAllMomentRequest> request)
        {
            return new ResponseContext<DeleteAllMomentResponse>
            {
                Content = new DeleteAllMomentResponse()
                {
                    IsExecuteSuccess = letterDal.UpdateMomentDelete(request.Content.UId)
                }
            };
        }

        public ResponseContext<UpdateAvatarUrlResponse> UpdateAvatarUrl(RequestContext<UpdateAvatarUrlRequest> request)
        {
            return new ResponseContext<UpdateAvatarUrlResponse>
            {
                Content = new UpdateAvatarUrlResponse()
                {
                    IsExecuteSuccess = letterDal.UpdateAvatarUrl(request.Content.AvatarUrl,request.Content.UId)
                }
            };
        }

        public ResponseContext<GetCollectListResponse> GetCollectList(RequestContext<GetCollectListRequest> request)
        {
            var response= new ResponseContext<GetCollectListResponse>
            {
                Content = new GetCollectListResponse()
                {
                    CollectList=new List<CollectType>()
                }
            };

            int pageSize = 200;
            string collectPageSize = JsonSettingHelper.AppSettings["CollectListPageSize"];
            if (!collectPageSize.IsNullOrEmpty())
            {
                pageSize = Convert.ToInt32(collectPageSize);
            }

            var collectList = letterDal.CollectListByUId(request.Content.UId, request.Content.PageIndex, pageSize);
            if (collectList.NotEmpty())
            {
                var userOnline = letterDal.GetOnLineUser(request.Content.UId);
                if (userOnline == null)
                {
                    return response;
                }
                foreach (var item in collectList)
                {
                    var dto = new CollectType()
                    {
                        CollectId = item.CollectId,
                        MomentId = item.MomentId,
                        UId = item.UId,
                        Gender=item.Gender,
                        IsHide = item.IsHide,
                        PickUpId = item.PickUpId,
                        HeadImgPath = item.HeadPhotoPath.GetImgPath(),
                        OnLineDesc = item.LastOnLineTime.GetOnlineDesc(item.IsOnLine),
                        DistanceDesc = LocationHelper.GetDistanceDesc(userOnline.Latitude, userOnline.Longitude, item != null ? item.Latitude : 0, item != null ? item.Longitude : 0),
                        TextContent = item.TextContent.Trim(),
                        ImgContent = item.ImgContent.GetImgPath(),
                        CreateTime = item.CreateTime.GetDateDesc()
                    };
                    if (item.IsHide)
                    {
                        dto.NickName= CommonHelper.CutNickName(item.HidingNickName, 8);
                        dto.ShortNickName = item.HidingNickName.Substring(0, 1);
                    }
                    else
                    {
                        dto.NickName = CommonHelper.CutNickName(item.NickName,8);
                    }
                    response.Content.CollectList.Add(dto);
                }
            }
            return response;
        }

        public ResponseContext<DeleteCollectResponse> DeleteCollect(RequestContext<DeleteCollectRequest> request)
        {
            return new ResponseContext<DeleteCollectResponse>
            {
                Content = new DeleteCollectResponse()
                {
                    IsExecuteSuccess = letterDal.DeleteCollect(request.Content.CollectId)
                }
            };
        }

        public ResponseContext<AddCollectResponse> AddCollect(RequestContext<AddCollectRequest> request)
        {
            var collect = letterDal.GetCollect(request.Content.MomentId,request.Content.UId);

            bool success;
            if (collect == null)
            {
                success=letterDal.InsertCollect(new CollectEntity()
                {
                    CollectId = Guid.NewGuid(),
                    UId = request.Content.UId,
                    MomentId=request.Content.MomentId,
                    PickUpId = request.Content.PickUpId,
                    FromPage =request.Content.FromPage,
                    CreateTime=DateTime.Now,
                    UpdateTime=DateTime.Now
                });
                if(success)
                {
                    var moment = letterDal.GetMoment(request.Content.MomentId);
                    if (moment != null)
                    {
                        userBiz.CoinChangeAsync(moment.UId, CoinChangeEnum.CollectedReward, "发布的动态被别人收藏，奖励金币");
                    }
                }
            }
            else
            {
                collect.UpdateTime = DateTime.Now;
                success = letterDal.UpdateCollectUpdateTime(collect);
            }

            return new ResponseContext<AddCollectResponse>
            {
                Content = new AddCollectResponse()
                {
                    IsExecuteSuccess = success
                }
            };
        }

        public ResponseContext<SetUserInfoResponse> SetUserInfo(RequestContext<SetUserInfoRequest> request)
        {
            var response = new ResponseContext<SetUserInfoResponse>()
            {
                Content = new SetUserInfoResponse()
            };
            var user = userBiz.LetterUserByUId(request.Content.UId);
            if (user == null)
            {
                return response;
            }
            user.HeadPhotoPath = request.Content.AvatarUrl;
            user.NickName= request.Content.NickName;
            user.Gender = request.Content.Gender;
            user.Country = request.Content.Country;
            user.Province = request.Content.Province;
            user.City = request.Content.City;
            user.UpdateTime = DateTime.Now;
            letterDal.UpdateUserBasicInfo(user);
            response.Content.TotalCoin = userBiz.UserTotalCoin(request.Content.UId);
            return response;
        }

        public ResponseContext<DeleteAllCollectResponse> DeleteAllCollect(RequestContext<DeleteAllCollectRequest> request)
        {
            return new ResponseContext<DeleteAllCollectResponse>
            {
                Content = new DeleteAllCollectResponse()
                {
                    IsExecuteSuccess = letterDal.DeleteAllCollect(request.Content.UId)
                }
            };
        }

        public ResponseContext<UserCoinInfoResponse> UserCoinInfo(RequestContext<UserCoinInfoRequest> request)
        {
            return new ResponseContext<UserCoinInfoResponse>
            {
                Content = new UserCoinInfoResponse()
                {
                    TotalCoin = userBiz.UserTotalCoin(request.Content.UId)
                }
            };
        }

        public ResponseContext<CoinDetailResponse> CoinDetail(RequestContext<CoinDetailRequest> request)
        {
            var response = new ResponseContext<CoinDetailResponse>
            {
                Content = new CoinDetailResponse()
            };
            var coinDetailList = userBiz.CoinDetailListByUId(request.Content.UId);
            if (coinDetailList.NotEmpty())
            {
                var incomeDetailList = new List<CoinDetailType>();
                var expendDetailList = new List<CoinDetailType>();
                foreach (var item in coinDetailList.OrderByDescending(a=>a.CreateTime))
                {
                    var dto = new CoinDetailType()
                    {
                        Description=item.Remark,
                        CreateTime=item.CreateTime.GetDateDesc(),
                        ChangeValue=item.ChangeValue>0?string.Format("+{0}", item.ChangeValue): string.Format("-{0}",-1*item.ChangeValue)
                    };
                    if (item.ChangeValue > 0)
                    {
                        incomeDetailList.Add(dto);
                    }
                    else
                    {
                        expendDetailList.Add(dto);
                    }
                }
                response.Content.IncomeDetailList = incomeDetailList;
                response.Content.ExpendDetailList = expendDetailList;
            }
            return response;
        }

        public PickUpEntity GetPickUpEntity(Guid pickUpId)
        {
            return letterDal.PickUp(pickUpId);
        }

        public ResponseContext<MsgSecCheckResponse> MsgSecCheck(RequestContext<MsgSecCheckRequest> request)
        {
            return new ResponseContext<MsgSecCheckResponse>()
            {
                Content = new MsgSecCheckResponse()
                {
                    IsOK = MiniAppFactory.Factory(request.Head.Platform).MsgIsOk(request.Content.TextContent)
                }
            };
        }

        public ResponseContext<ForwardMomentResponse> ForwardMoment(RequestContext<ForwardMomentRequest> request)
        {
            var response = new ResponseContext<ForwardMomentResponse>
            {
                Content = new ForwardMomentResponse()
            };
            var moment = letterDal.GetMoment(request.Content.MomentId);
            if (moment == null)
            {
                return new ResponseContext<ForwardMomentResponse>(false, ErrCodeEnum.DataIsnotExist, null, "转发失败");
            }
            var newMoment = new MomentEntity()
            {
                MomentId = Guid.NewGuid(),
                UId=request.Content.UId,
                TextContent=moment.TextContent,
                ImgContent=moment.ImgContent,
                IsDelete=false,
                IsReport=false,
                IsHide=false,
                HidingNickName=null,
                ReplyCount=0,
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now
            };
            response.Content.Success = letterDal.InsertMoment(newMoment);
            return response;
        }

        public ResponseContext<OnlineNotifyResponse> OnlineNotify(RequestContext<OnlineNotifyRequest> request)
        {
            var response = new ResponseContext<OnlineNotifyResponse>
            {
                Content = new OnlineNotifyResponse()
            };
            var onlineNotify= letterDal.OnlineNotify(request.Content.UId, request.Content.PartnerUId);
            if (onlineNotify == null)
            {
                onlineNotify = new OnlineNotifyEntity()
                {
                    OnlineNotifyId = Guid.NewGuid(),
                    UId = request.Content.UId,
                    PartnerUId = request.Content.PartnerUId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                response.Content.Success = letterDal.InsertOnlineNotify(onlineNotify);
            }
            else
            {
                response.Content.Success = true;
            } 
            
            return response;
        }

        public ResponseContext<CancelAttentionResponse> CancelAttention(RequestContext<CancelAttentionRequest> request)
        {
            return new ResponseContext<CancelAttentionResponse>()
            {
                Content = new CancelAttentionResponse()
                {
                    IsExecuteSuccess = letterDal.DeleteAttention(request.Content.UId, request.Content.PartnerUId)
                }
            };
        }

        public ResponseContext<AddAttentionResponse> AddAttention(RequestContext<AddAttentionRequest> request)
        {
            var response = new ResponseContext<AddAttentionResponse>
            {
                Content = new AddAttentionResponse() { IsExecuteSuccess=true }
            };
            AttentionEntity entity = letterDal.Attention(request.Content.UId, request.Content.PartnerUId);
            if (entity != null)
            {
                return response;
            }
            else
            {
                entity = new AttentionEntity()
                {
                    AttentionId = Guid.NewGuid(),
                    UId = request.Content.UId,
                    PartnerUId = request.Content.PartnerUId,
                    AttentionMomentId = request.Content.MomentId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                response.Content.IsExecuteSuccess = letterDal.InsertAttention(entity);
            }
            return response;
        }

        public ResponseContext<UpdateUserLocationResponse> UpdateUserLocation(RequestContext<UpdateUserLocationRequest> request)
        {
            var response = new ResponseContext<UpdateUserLocationResponse>
            {
                Content = new UpdateUserLocationResponse() { Success = true }
            };
            OnLineUserHubEntity onLineUser = userBiz.OnLineUser(request.Content.UId);
            if (onLineUser != null)
            {
                onLineUser.Latitude = request.Content.Latitude;
                onLineUser.Longitude = request.Content.Longitude;
                onLineUser.UpdateTime = DateTime.Now;

                response.Content.Success = letterDal.UpdateUserLocation(onLineUser);
            }
            return response;
        }

        public ResponseContext<AttentionMomentCountResponse> AttentionMomentCount(RequestContext<AttentionMomentCountRequest> request)
        {
            return new ResponseContext<AttentionMomentCountResponse>()
            {
               Content = new AttentionMomentCountResponse()
               {
                   UnReadCountStr = UnRead(letterDal.UnReadAttentionMomentCount(request.Content.UId))
               }
            };
        }

        public ResponseContext<UpdateLastScanMomentTimeResponse> UpdateLastScanMomentTime(RequestContext<UpdateLastScanMomentTimeRequest> request)
        {
            return new ResponseContext<UpdateLastScanMomentTimeResponse>()
            {
                Content = new UpdateLastScanMomentTimeResponse()
                {
                    Success = letterDal.UpdateLastScanMomentTime(request.Content.UId)
                }
            };
        }

        public ResponseContext<UpdateHidingResponse> UpdateHiding(RequestContext<UpdateHidingRequest> request)
        {
            
            PickUpEntity pickUp;
            if (request.Content.PickUpId != null && request.Content.PickUpId != Guid.Empty)
            {
                pickUp = letterDal.PickUp(request.Content.PickUpId);
            }
            else
            {
                pickUp = letterDal.PickUpByMomentId(request.Content.MomentId, request.Content.UId);
                if (pickUp == null)
                {
                    var moment = letterDal.GetMoment(request.Content.MomentId);
                    pickUp = new PickUpEntity()
                    {
                        PickUpId = Guid.NewGuid(),
                        MomentId = moment.MomentId,
                        MomentUId = moment.UId,
                        PickUpUId = request.Content.UId,
                        FromPage = PickUpFromPageEnum.AttentionPage,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };
                    letterDal.InsertPickUp(pickUp);
                }
            }
            return new ResponseContext<UpdateHidingResponse>()
            {
                Content = new UpdateHidingResponse()
                {
                    Success = letterDal.UpdateHiding(pickUp.PickUpId, request.Content.IsHide, request.Content.HidingNickName)
                }
            };
        }

        public ResponseContext<ChatDetailListResponse> ChatDetailList(RequestContext<ChatDetailListRequest> request)
        {
            var response = new ResponseContext<ChatDetailListResponse>() 
            { 
                Content=new ChatDetailListResponse() 
                { 
                    DiscussDetailList=new List<DiscussDetailType>()
                }
            };

            MomentEntity moment;
            PickUpEntity pickUp;
            if (request.Content.PickUpId != null && request.Content.PickUpId != Guid.Empty)
            {
                pickUp = letterDal.PickUp(request.Content.PickUpId.Value);
                moment = letterDal.GetMoment(pickUp.MomentId);
            }
            else
            {
                moment = letterDal.GetMoment(request.Content.MomentId.Value);
                pickUp = letterDal.PickUpByMomentId(request.Content.MomentId.Value, request.Content.UId);
                if (pickUp == null)
                {
                    pickUp = new PickUpEntity()
                    {
                        PickUpId = Guid.NewGuid(),
                        MomentId = moment.MomentId,
                        MomentUId = moment.UId,
                        PickUpUId = request.Content.UId,
                        FromPage = PickUpFromPageEnum.AttentionPage,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };
                    letterDal.InsertPickUp(pickUp);
                }
            }
            if (moment == null)
            {
                return response;
            }

            DateTime? deleteTime;
            if (moment.UId == request.Head.UId)
            {
                deleteTime = pickUp.UserLastDeleteTime;
            }
            else
            {
                deleteTime = pickUp.PartnerLastDeleteTime;
            }
            var discussList = letterDal.DiscussList(pickUp.PickUpId, deleteTime);
            if (discussList.NotEmpty())
            {
                var keyValues = new Dictionary<long, PickUpDTO>();
                foreach (var item in discussList.GroupBy(a => a.UId))
                {
                    keyValues.Add(item.Key, BuildPickUpDTO(item.Key, moment, pickUp));
                }
                discussList=discussList.OrderBy(a => a.CreateTime).ToList();
                for(int index=0;index< discussList.Count; index++)
                {
                    var pickDto = keyValues.FirstOrDefault(a => a.Key == discussList[index].UId);
                    if (pickDto.Value == null)
                    {
                        continue;
                    }
                    var dto = new DiscussDetailType()
                    {
                        PickUpUId = discussList[index].UId,
                        IsMyReply = discussList[index].UId == request.Content.UId,
                        HeadImgPath = pickDto.Value.HeadPhotoPath.GetImgPath(),
                        NickName = pickDto.Value.NickName.TextCut(15),
                        ShortNickName = pickDto.Value.NickName.Substring(0, 1),
                        Gender = pickDto.Value.Gender,
                        IsHide = pickDto.Value.IsHide,
                        TextContent = discussList[index].DiscussContent,
                        RecentChatTime = discussList[index].CreateTime.GetChatDetailDateTime()
                    };
                    if (discussList.Count == 1)
                    {
                        dto.IsTimeVisible = true;
                    }
                    else
                    {
                        if (index == 0)
                        {
                            dto.IsTimeVisible = true;
                        }
                        else
                        {
                            dto.IsTimeVisible = discussList[index].CreateTime.Subtract(discussList[index - 1].CreateTime).TotalMinutes >= 5;
                        }
                    }
                    response.Content.DiscussDetailList.Add(dto);
                }
            }
            return response;
        }


        #endregion

        #region private Method

        /// <summary>
        /// 获取某条动态未读数量
        /// </summary>
        private string UnReadCount(Guid pickUpId,long uId)
        {
            return UnRead(letterDal.UnReadCount(pickUpId, uId));
        }

        private string UnReadTotalCount(long uId)
        {
            return UnRead(letterDal.UnReadTotalCount(uId));
        }

        /// <summary>
        /// 未读数量文本化
        /// </summary>
        private string UnRead(int count)
        {
            if (count == 0)
            {
                return string.Empty;
            }
            else if (count > 0 && count <= 99)
            {
                return count.ToString();
            }
            else
            {
                return "99+";
            }
        }

        private string BasicUserInfo(LetterUserEntity userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            if (userInfo.Gender != GenderEnum.Default)
            {
                sb.Append(userInfo.Gender == GenderEnum.Man ? "男" : "女");
                sb.Append("•");
            }
            if (!userInfo.BirthDate.IsNullOrEmpty())
            {
                sb.AppendFormat("{0}岁", Convert.ToDateTime(userInfo.BirthDate).GetAgeByBirthdate());
                sb.Append("•");

                sb.Append(Convert.ToDateTime(userInfo.BirthDate).GetConstellation());
                sb.Append("•");
            }
            return sb.ToString().TrimEnd('•');
        }
        private string PlaceInfo(LetterUserEntity userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            if (!userInfo.Province.IsNullOrEmpty()&& userInfo.Province!="全部")
            {
                sb.Append(userInfo.Province);
                sb.Append("•");
            }

            if (!userInfo.City.IsNullOrEmpty()&& userInfo.City.Trim()!= userInfo.Province.Trim() && userInfo.City != "全部")
            {
                sb.Append(userInfo.City);
                sb.Append("•");
            }

            if (!userInfo.Area.IsNullOrEmpty()&&!userInfo.City.IsNullOrEmpty() && 
                userInfo.Area.Trim() != userInfo.City.Trim() && userInfo.Area != "全部")
            {
                sb.Append(userInfo.Area);
                sb.Append("•");
            }

            var rtn= sb.ToString().TrimEnd('•');

            return rtn.IsNullOrEmpty() ? "远方" : rtn;
        }

        /// <summary>
        /// 更新最新登录时间
        /// </summary>
        /// <param name="user"></param>
        private void UpdateLastLoginTime(LetterUserEntity user)
        {
            if (user.LastLoginTime.HasValue)
            {
                //今天首次登录
                if (user.LastLoginTime.Value < DateTime.Now.Date)
                {
                    userBiz.CoinChangeAsync(user.UId, CoinChangeEnum.SignReward, string.Format("{0}登录签到奖励金币", DateTime.Now.ToShortDateString()));
                }
                user.LastLoginTime = DateTime.Now;
            }
            else
            {
                user.LastLoginTime = DateTime.Now;
            }

            Task.Factory.StartNew(() =>
            {
                letterDal.UpdateLastLoginTime(user);
            });
        }

        #endregion
    }
}
