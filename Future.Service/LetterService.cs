using Future.Model.Entity.Letter;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Repository;
using Future.Utility;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Future.Service
{
    public class LetterService
    {
        private readonly LetterRepository letterDal = SingletonProvider<LetterRepository>.Instance;

        public ResponseContext<DiscussListResponse> DiscussList(RequestContext<DiscussListRequest> request)
        {
            var response = new ResponseContext<DiscussListResponse>()
            {
                Content = new DiscussListResponse()
                {
                    DiscussList=new List<DiscussType>()
                }
            };

            var pickUpList = new List<PickUpEntity>();
            //主动捡起的瓶子
            var myPickUpList = letterDal.PickUpListPickUpUId(request.Content.UId);
            if (myPickUpList.NotEmpty())
            {
                pickUpList.AddRange(myPickUpList);
            }
            var partnerPickUpList= letterDal.PickUpListByMomentUId(request.Content.UId);
            if (partnerPickUpList.NotEmpty())
            {
                pickUpList.AddRange(partnerPickUpList);
            }

            if (pickUpList.NotEmpty())
            {
                foreach(var item in pickUpList)
                {
                    var partnerUId = item.PickUpUId == request.Content.UId ? item.MomentUId : item.PickUpUId;
                    var pickUpUser= letterDal.LetterUser(partnerUId);
                    if (pickUpUser == null)
                    {
                        continue;
                    }
                    var discussList= letterDal.DiscussList(item.PickUpId);
                    if (discussList.IsNullOrEmpty())
                    {
                        continue;
                    }
                    var lastDiscuss = discussList.OrderByDescending(a => a.CreateTime).First();
                    var dto = new DiscussType()
                    {
                        PickUpId = item.PickUpId,
                        MomentUId = item.MomentUId,
                        PartnerUId = partnerUId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = lastDiscuss.DiscussContent,
                        SortChatTime= lastDiscuss.CreateTime,
                        RecentChatTime = lastDiscuss.CreateTime.GetDateDesc()
                    };

                    response.Content.DiscussList.Add(dto);
                }
                response.Content.DiscussList = response.Content.DiscussList.OrderByDescending(a => a.SortChatTime).ToList();
            }
            return response;
        }
        
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseContext<PickUpListResponse> PickUpList(RequestContext<PickUpListRequest> request)
        {
            var response = new ResponseContext<PickUpListResponse>()
            {
                Content = new PickUpListResponse()
                {
                    PickUpList=new List<PickUpType>()
                }
            };

            int pageSize = 30;
            if (!JsonSettingHelper.AppSettings["MomentPageSize"].IsNullOrEmpty())
            {
                pageSize = Convert.ToInt32(JsonSettingHelper.AppSettings["MomentPageSize"]);
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

        public ResponseContext<PickUpResponse> PickUp(RequestContext<PickUpRequest> request)
        {
            var response = new ResponseContext<PickUpResponse>()
            {
                Content = new PickUpResponse()
                {
                    PickUpList = new List<PickUpType>()
                }
            };

            int pickUpCount = 20;
            if (!string.IsNullOrEmpty(JsonSettingHelper.AppSettings["PickUpCount"]))
            {
                pickUpCount= Convert.ToInt16(JsonSettingHelper.AppSettings["PickUpCount"]);
            }
            var moment= letterDal.GetMoment(request.Content.UId, pickUpCount);
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
                    UId= moment.UId,
                    HeadImgPath= letterUser.HeadPhotoPath.GetImgPath(),
                    NickName= letterUser.NickName,
                    TextContent= moment.TextContent.Trim(),
                    ImgContent= moment.ImgContent.GetImgPath(),
                    CreateTime= moment.CreateTime.GetDateDesc()
                });
            }
            return response;
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
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now
            };

            response.Content.IsExecuteSuccess= letterDal.InsertMoment(moment);
            return response;
        }

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
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                bool success = letterDal.InsertLetterUser(entity);
                if (success)
                {
                    response.Content.UId = letterDal.LetterUser(0, request.Content.OpenId).UId;
                }
                response.Content.IsExecuteSuccess = success;
            }
            else
            {
                if (!string.IsNullOrEmpty(request.Content.AvatarUrl))
                {
                    letterDal.UpdateAvatarUrl(request.Content.AvatarUrl, userInfoEntity.UId);
                }
                
                response.Content.UId = userInfoEntity.UId;
                response.Content.IsExecuteSuccess = true;
            }
            return response;
        }

        public ResponseContext<GetOpenIdResponse> GetOpenId(RequestContext<GetOpenIdRequest> request)
        {
            var response = new ResponseContext<GetOpenIdResponse>();

            string myAppid = JsonSettingHelper.AppSettings["LetterAppId"];
            string mySecret = JsonSettingHelper.AppSettings["LetterSecret"];

            string url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, request.Content.LoginCode);
            response.Content = HttpHelper.HttpGet<GetOpenIdResponse>(url);
            return response;
        }

        public ResponseContext<DeleteBottleResponse> DeleteBottle(RequestContext<DeleteBottleRequest> request)
        {
            var response = new ResponseContext<DeleteBottleResponse>()
            {
                Content = new DeleteBottleResponse()
            };
            letterDal.DeleteDiscuss(request.Content.PickUpId);
            response.Content.IsExecuteSuccess = letterDal.UpdatePickDelete(request.Content.PickUpId); ;
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
            }
            return response;
        }

        public ResponseContext<ClearAllBottleResponse> ClearAllBottle(RequestContext<ClearAllBottleRequest> request)
        {
            var response = new ResponseContext<ClearAllBottleResponse>()
            {
                Content = new ClearAllBottleResponse()
            };
            var pickUpList = letterDal.PickUpListPickUpUId(request.Content.UId);
            foreach(var item in pickUpList)
            {
                //清空未回复过的所有瓶子
                var discussList= letterDal.DiscussList(item.PickUpId);
                if (discussList.IsNullOrEmpty())
                {
                    letterDal.UpdatePickDelete(item.PickUpId);
                }
            }
            
            response.Content.IsExecuteSuccess = true;
            return response;
        }
    }
}
