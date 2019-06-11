using Future.Model.Utils;
using Future.Repository;
using Future.Utility;
using Infrastructure;
using System.Linq;

namespace Future.Service
{
    public class LetterService
    {
        private readonly LetterRepository letterDal = SingletonProvider<LetterRepository>.Instance;

        public ResponseContext<PickUpListResponse> PickUpList(RequestContext<PickUpListRequest> request)
        {
            var response = new ResponseContext<PickUpListResponse>()
            {
                Content = new PickUpListResponse()
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
                    var dto = new PickUpType()
                    {
                        PickUpId = item.PickUpId,
                        MomentUId = item.MomentUId,
                        PickUpUId = item.PickUpUId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = lastDiscuss.DiscussContent,
                        RecentChatTime = item.UpdateTime.Value.GetDateDesc()
                    };

                    response.Content.PickUpList.Add(dto);
                }
            }
            return response;
        }
        
        public ResponseContext<DiscussListResponse> DiscussList(RequestContext<DiscussListRequest> request)
        {
            var response = new ResponseContext<DiscussListResponse>()
            {
                Content = new DiscussListResponse()
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
                    var dto = new DiscussType()
                    {
                        PickUpId = item.PickUpId,
                        PickUpUId=item.UId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = item.DiscussContent,
                        RecentChatTime = item.UpdateTime.Value.GetDateDesc()
                    };

                    response.Content.DiscussList.Add(dto);
                }
            }
            return response;
        }
    }
}
