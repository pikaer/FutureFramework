using Future.Model.Entity.Letter;
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

            var pickUpList = letterDal.PickUpList(request.Content.UId, request.Content.PageIndex);
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

            var pickUpList = letterDal.PickUpList(request.Content.UId,request.Content.PageIndex);
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
                        MomentId = item.MomentId,
                        UId = item.MomentUId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = moment.TextContent,
                        ImgContent = moment.ImgContent,
                        CreateTime = item.CreateTime.GetDateDesc()
                    };

                    response.Content.PickUpList.Add(dto);
                }
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
    }
}
