using Future.Model.Entity.Letter;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Repository;
using Future.Utility;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Future.Service
{
    public class LetterService
    {
        #region public Method

        private readonly LetterRepository letterDal = SingletonProvider<LetterRepository>.Instance;

        private readonly SysRepository sysDal = SingletonProvider<SysRepository>.Instance;

        /// <summary>
        /// 捡到的回复过的瓶子列表
        /// </summary>
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
                foreach(var item in pickUpList)
                {
                    var dto = new DiscussType()
                    {
                        PickUpId = item.PickUpId,
                        UId= item.UId,
                        HeadImgPath = item.HeadPhotoPath.GetImgPath(),
                        NickName = item.NickName,
                        TextContent = TextCut(item.DiscussContent),
                        UnReadCount=UnReadCount(item.PickUpId,request.Content.UId),
                        RecentChatTime = item.CreateTime.GetDateDesc()
                    };

                    response.Content.DiscussList.Add(dto);
                }
                response.Content.CurrentTotalUnReadCount= UnReadTotalCount(request.Content.UId);
            }
            return response;
        }
        
        /// <summary>
        /// 某个瓶子评论详情
        /// </summary>
        public ResponseContext<DiscussDetailResponse> DiscussDetail(RequestContext<DiscussDetailRequest> request)
        {
            var response = new ResponseContext<DiscussDetailResponse>();

            var pickUp= letterDal.PickUp(request.Content.PickUpId);
            if(pickUp==null)
            {
                return response;
            }
            var moment= letterDal.GetMoment(pickUp.MomentId);
            if (moment == null)
            {
                return response;
            }
            var user= letterDal.LetterUser(moment.UId);
            if (user == null)
            {
                return response;
            }

            response.Content = new DiscussDetailResponse()
            {
                MomentId = moment.MomentId,
                MomentUId= moment.UId,
                HeadImgPath= user.HeadPhotoPath.GetImgPath(),
                NickName= user.NickName.Trim(),
                TextContent= moment.TextContent.Trim(),
                ImgContent= moment.ImgContent.IsNullOrEmpty()?"":moment.ImgContent.Trim().GetImgPath(),
                CreateTime= moment.CreateTime.GetDateDesc(),
                DiscussDetailList=new List<DiscussDetailType>()
            };

            var discussList = letterDal.DiscussList(request.Content.PickUpId);
            if (discussList.NotEmpty())
            {
                foreach (var item in discussList)
                {
                    var pickUpUser = letterDal.LetterUser(item.UId);
                    if (pickUpUser == null)
                    {
                        continue;
                    }
                    var dto = new DiscussDetailType()
                    {
                        PickUpUId=item.UId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = item.DiscussContent,
                        RecentChatTime = item.UpdateTime.Value.GetDateDesc()
                    };

                    response.Content.DiscussDetailList.Add(dto);
                }
            }
            return response;
        }

        /// <summary>
        /// 评论某个瓶子
        /// </summary>
        public ResponseContext<DiscussResponse> Discuss(RequestContext<DiscussRequest> request)
        {
            var response = new ResponseContext<DiscussResponse>()
            {
                Content = new DiscussResponse()
            };

            var discuss = new DiscussEntity()
            {
                DiscussId = Guid.NewGuid(),
                PickUpId = request.Content.PickUpId,
                UId = request.Content.UId,
                DiscussContent = request.Content.TextContent,
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now
            };
            response.Content.IsExecuteSuccess= letterDal.InsertDiscuss(discuss);
            return response;
        }

        /// <summary>
        /// 捡起但没有回复的漂流瓶列表
        /// </summary>
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
            var pickUpList = letterDal.PickUpListByPageIndex(request.Content.UId,request.Content.PageIndex, pageSize);
            if (pickUpList.NotEmpty())
            {
                foreach (var item in pickUpList)
                {
                    var pickUpUser = letterDal.LetterUser(item.MomentUId);
                    if (pickUpUser == null)
                    {
                        continue;
                    }
                    var moment= letterDal.GetMoment(item.MomentId);
                    if (moment == null)
                    {
                        continue;
                    }
                    var dto = new PickUpType()
                    {
                        PickUpId = item.PickUpId,
                        MomentId= item.MomentId,
                        UId = item.MomentUId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = moment.TextContent,
                        ImgContent = moment.ImgContent.GetImgPath(),
                        CreateTime = moment.CreateTime.GetDateDesc()
                    };

                    response.Content.PickUpList.Add(dto);
                }
            }
            
            return response;
        }

        /// <summary>
        /// 捡一个瓶子
        /// </summary>
        public ResponseContext<PickUpResponse> PickUp(RequestContext<PickUpRequest> request)
        {
            var response = new ResponseContext<PickUpResponse>()
            {
                Content = new PickUpResponse()
                {
                    PickUpList = new List<PickUpType>()
                }
            };
            var user= letterDal.LetterUser(request.Content.UId);
            if (user == null)
            {
                return response;
            }
            int pickUpCount = 20;
            if (!string.IsNullOrEmpty(JsonSettingHelper.AppSettings["PickUpCount"]))
            {
                pickUpCount= Convert.ToInt16(JsonSettingHelper.AppSettings["PickUpCount"]);
            }
            var moment= letterDal.GetMoment(request.Content.UId, pickUpCount, user.Gender);
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
                var letterUser= letterDal.LetterUser(moment.UId);
                if (letterUser == null)
                {
                    return response;
                }
                response.Content.PickUpList.Add(new PickUpType()
                {
                    PickUpId = pickUp.PickUpId,
                    MomentId= pickUp.MomentId,
                    UId = moment.UId,
                    HeadImgPath= letterUser.HeadPhotoPath.GetImgPath(),
                    NickName= letterUser.NickName,
                    TextContent= moment.TextContent.Trim(),
                    ImgContent= moment.ImgContent.GetImgPath(),
                    CreateTime= moment.CreateTime.GetDateDesc()
                });
            }
            return response;
        }

        /// <summary>
        /// 扔一个瓶子
        /// </summary>
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
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now
            };

            response.Content.IsExecuteSuccess= letterDal.InsertMoment(moment);
            return response;
        }

        /// <summary>
        /// 我扔出去的所有动态
        /// </summary>
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
                response.Content.MomentList = myMomentList.Select(a => new MomentType()
                {
                    MomentId=a.MomentId,
                    TextContent=a.TextContent.Trim(),
                    ImgContent=a.ImgContent.GetImgPath(),
                    PublishTime=a.CreateTime.GetDateDesc()
                }).ToList();
            }
            return response;
        }

        /// <summary>
        /// 动态详情
        /// </summary>
        public ResponseContext<MomentDetailResponse> MomentDetail(RequestContext<MomentDetailRequest> request)
        {
            var response = new ResponseContext<MomentDetailResponse>()
            {
                Content=new MomentDetailResponse()
            };
            var moment = letterDal.GetMoment(request.Content.MomentId);
            if (moment != null)
            {
                var user = letterDal.LetterUser(moment.UId);
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
                        CreateTime = moment.CreateTime.GetDateDesc()
                    };
                }
                
            }
            return response;
        }

        /// <summary>
        /// 更新用户信息并返回UId
        /// </summary>
        public ResponseContext<SetUserInfoResponse> SetUserInfo(RequestContext<SetUserInfoRequest> request)
        {
            var response = new ResponseContext<SetUserInfoResponse>()
            {
                Content = new SetUserInfoResponse()
            };
            
            var userInfoEntity = letterDal.LetterUser(0, request.Content.OpenId);
            if (userInfoEntity == null)
            {
                var entity = new LetterUserEntity()
                {
                    OpenId = request.Content.OpenId,
                    Gender = (GenderEnum)request.Content.Gender,
                    NickName = request.Content.NickName,
                    Country= request.Content.Country,
                    Province = request.Content.Province,
                    City = request.Content.City,
                    HeadPhotoPath = request.Content.AvatarUrl,
                    Signature= "与恶龙缠斗过久,自身亦成为恶龙；凝视深渊过久,深渊将回以凝视。",
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                //使用模拟信息
                string configStr = JsonSettingHelper.AppSettings["UseSimulateUserInfo"];
                if (!string.IsNullOrEmpty(configStr))
                {
                    if (Convert.ToBoolean(configStr))
                    {
                        var imgList = sysDal.ImgGalleryList();
                        if (imgList.NotEmpty())
                        {
                            var img = imgList.OrderBy(a => a.UseCount).First();
                            entity.NickName = img.ImgName.Trim();
                            entity.HeadPhotoPath= img.ShortUrl.Trim();

                            sysDal.UpdateImgUseCount(img.ImgId);
                        }
                    }
                }
                bool success = letterDal.InsertLetterUser(entity);
                if (success)
                {
                    response.Content.UId = letterDal.LetterUser(0, request.Content.OpenId).UId;
                }
                response.Content.IsExecuteSuccess = success;
            }
            else
            {
                response.Content.UId = userInfoEntity.UId;
                response.Content.IsExecuteSuccess = true;
            }
            return response;
        }

        /// <summary>
        /// 获取用户小程序端唯一标示
        /// </summary>
        public ResponseContext<GetOpenIdResponse> GetOpenId(RequestContext<GetOpenIdRequest> request)
        {
            var response = new ResponseContext<GetOpenIdResponse>();

            string myAppid = JsonSettingHelper.AppSettings["LetterAppId"];
            string mySecret = JsonSettingHelper.AppSettings["LetterSecret"];

            string url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, request.Content.LoginCode);
            response.Content = HttpHelper.HttpGet<GetOpenIdResponse>(url);
            return response;
        }

        /// <summary>
        /// 删除单个瓶子
        /// </summary>
        public ResponseContext<DeleteBottleResponse> DeleteBottle(RequestContext<DeleteBottleRequest> request)
        {
            var response = new ResponseContext<DeleteBottleResponse>()
            {
                Content = new DeleteBottleResponse()
            };

            var pickUp= letterDal.PickUp(request.Content.PickUpId);
            if (pickUp == null)
            {
                return response;
            }
            if(pickUp.MomentUId== request.Content.UId)
            {
                letterDal.UpdatePickDelete(pickUp.PickUpId,1,0);
            }
            else
            {
                letterDal.UpdatePickDelete(pickUp.PickUpId,0,1);
            }
            response.Content.IsExecuteSuccess = true;
            response.Content.CurrentTotalUnReadCount = UnReadTotalCount(request.Content.UId);
            return response;
        }

        /// <summary>
        /// 举报某个瓶子
        /// </summary>
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
            }
            return response;
        }

        /// <summary>
        /// 清空未回复的所有瓶子
        /// </summary>
        public ResponseContext<ClearAllBottleResponse> ClearAllBottle(RequestContext<ClearAllBottleRequest> request)
        {
            var response = new ResponseContext<ClearAllBottleResponse>()
            {
                Content = new ClearAllBottleResponse()
            };
            var pickUpList = letterDal.PickUpListByPickUpUIdWithoutReply(request.Content.UId);
            foreach(var item in pickUpList)
            {
                letterDal.UpdatePickDelete(item.PickUpId, 0, 1);
            }
            
            response.Content.IsExecuteSuccess = true;
            return response;
        }

        /// <summary>
        /// 清空单个瓶子未读数量
        /// </summary>
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

        /// <summary>
        /// 未读总数量
        /// </summary>
        public ResponseContext<UnReadTotalCountResponse> UnReadTotalCount(RequestContext<UnReadTotalCountRequest> request)
        {
            var response = new ResponseContext<UnReadTotalCountResponse>()
            {
                Content = new UnReadTotalCountResponse()
            };
            
            response.Content.UnReadCount = UnReadTotalCount(request.Content.UId);

            return response;
        }

        /// <summary>
        /// 删除回复过的所有瓶子
        /// </summary>
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

        /// <summary>
        /// 所有未读消息标为已读
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 用户基础信息
        /// </summary>
        public ResponseContext<BasicUserInfoResponse> BasicUserInfo(RequestContext<BasicUserInfoRequest> request)
        {
            var response = new ResponseContext<BasicUserInfoResponse>();
            var userInfo = letterDal.LetterUser(request.Content.UId);
            if (userInfo == null)
            {
                return response;
            }
            response.Content = new BasicUserInfoResponse()
            {
                UId= userInfo.UId,
                NickName= userInfo.NickName.Trim(),
                HeadPhotoPath= userInfo.HeadPhotoPath.GetImgPath(),
                Signature= userInfo.Signature.IsNullOrEmpty()? "与恶龙缠斗过久,自身亦成为恶龙；凝视深渊过久,深渊将回以凝视。" : userInfo.Signature.Trim(),
                BasicUserInfo= BasicUserInfo(userInfo),
                PlaceInfo=PlaceInfo(userInfo)
            };
            return response;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
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

        /// <summary>
        /// 获取用户完整信息
        /// </summary>
        public ResponseContext<GetUserInfoResponse> GetUserInfo(RequestContext<GetUserInfoRequest> request)
        {
            var response = new ResponseContext<GetUserInfoResponse>();
            var userInfo = letterDal.LetterUser(request.Content.UId);
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
                EntranceDate = userInfo.EntranceDate.IsNullOrEmpty()?"": userInfo.EntranceDate,
                SchoolName = userInfo.SchoolName.IsNullOrEmpty()?"": userInfo.SchoolName,
                BirthDate = userInfo.BirthDate.IsNullOrEmpty() ? "" : userInfo.BirthDate,
                Area = userInfo.Area.IsNullOrEmpty() ? "" : userInfo.Area,
                Province = userInfo.Province.IsNullOrEmpty() ? "" : userInfo.Province,
                City = userInfo.City.IsNullOrEmpty() ? "" : userInfo.City,
                NickName = userInfo.NickName.Trim(),
                Signature = userInfo.Signature.IsNullOrEmpty() ? "" : userInfo.Signature.Trim(),
            };
            return response;
        }

        public ResponseContext<DeleteMomentResponse> DeleteMoment(RequestContext<DeleteMomentRequest> request)
        {
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

        /// <summary>
        /// 用户收藏列表
        /// </summary>
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
                foreach(var item in collectList)
                {
                    response.Content.CollectList.Add(new CollectType()
                    {
                        CollectId=item.CollectId,
                        MomentId=item.MomentId,
                        PickUpId=item.PickUpId,
                        TextContent =item.TextContent.Trim(),
                        ImgContent=item.ImgContent.GetImgPath(),
                        CreateTime=item.CreateTime.GetDateDesc()
                    });
                }
            }
            return response;
        }

        /// <summary>
        /// 删除收藏
        /// </summary>
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

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseContext<AddCollectResponse> AddCollect(RequestContext<AddCollectRequest> request)
        {
            var collect = letterDal.GetCollect(request.Content.MomentId,request.Content.UId);

            bool success = false;
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

        /// <summary>
        /// 清空所有收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 文本截取处理
        /// </summary>
        private string TextCut(string text)
        {
            if (string.IsNullOrEmpty(text)|| string.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            else if (text.Length < 15)
            {
                return text;
            }
            else
            {
                string result = text.Substring(0, 15);
                return result + "...";
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

            if (!userInfo.Area.IsNullOrEmpty() && userInfo.Area.Trim() != userInfo.City.Trim() && userInfo.Area != "全部")
            {
                sb.Append(userInfo.Area);
                sb.Append("•");
            }

            return sb.ToString().TrimEnd('•');
        }
        
        #endregion
    }
}
