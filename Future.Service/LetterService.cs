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

            var pickUpList = letterDal.PickUpList(request.Content.UId);
            if (pickUpList.NotEmpty())
            {
                foreach(var item in pickUpList)
                {
                    var pickUpUser= letterDal.LetterUser(item.PickUpUId);
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
                        PickUpUId = item.PickUpUId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = lastDiscuss.DiscussContent,
                        RecentChatTime = item.UpdateTime.Value.GetDateDesc()
                    };

                    response.Content.DiscussList.Add(dto);
                }
            }
            return response;
        }
        
        public ResponseContext<DiscussDetailListResponse> DiscussDetailList(RequestContext<DiscussDetailListRequest> request)
        {
            var response = new ResponseContext<DiscussDetailListResponse>()
            {
                Content = new DiscussDetailListResponse()
                {
                    DiscussDetailList=new List<DiscussDetailType>()
                }
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
                        PickUpId = item.PickUpId,
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
                DiscussContent = request.Content.TextContent
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
                        CreateTime = item.CreateTime.GetDateDesc()
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
                    HeadPhotoPath = "pikaer.jpg",
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
            response.Content.IsExecuteSuccess = letterDal.DeletePickUp(request.Content.PickUpId); ;
            return response;
        }

        public ResponseContext<ReportBottleResponse> ReportBottle(RequestContext<ReportBottleRequest> request)
        {
            var response = new ResponseContext<ReportBottleResponse>()
            {
                Content = new ReportBottleResponse()
            };
            var pickUp= letterDal.GetMoment(request.Content.PickUpId);
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
    }
}
