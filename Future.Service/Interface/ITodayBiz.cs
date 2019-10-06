using Future.Model.DTO.Today;
using Future.Model.Entity.Letter;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Letter;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using System;
using System.Collections.Generic;

namespace Future.Service.Interface
{
    public interface ITodayBiz
    {
        string GetStaffName(long userId);

        PageResult<ImgGalleryDTO> GetImageGalleryList(int pageIndex, int pageSize, string imgName, long creater, DateTime? startDateTime, DateTime? endCreateTime);

        ResponseContext<bool> DeleteImage(long imageId);

        ResponseContext<bool> AddOrUpdateImage(ImgGalleryEntity request);

        ResponseContext<bool> UpdateShortUrl(ImgGalleryEntity request);

        PageResult<SimulateUserDTO> GetSimulateUserList(int pageIndex, int pageSize, long uId, string nickName, GenderEnum gender, long creater, DateTime? startDateTime, DateTime? endCreateTime);

        PageResult<RealUserDTO> GetRealUserList(int pageIndex, int pageSize, long uId, string nickName, string openId, GenderEnum gender, DateTime? startDateTime, DateTime? endCreateTime);

        ResponseContext<bool> AddOrUpdateSimulateUser(LetterUserEntity request);

        ResponseContext<bool> DeleteLetterUser(long uId);

        ResponseContext<bool> UpdateAvatarUrl(long uId, long imgId);

        PageResult<PickUpListDTO> GetRealUserPickUpList(int page, int rows, int uId, PickUpTypeEnum pickType);

        PageResult<PublishMomentListDTO> GetSimulateUserPublishList(int page, int rows, long uId, MomentStateEnum state, DateTime startDateTime, DateTime endCreateTime);

        ResponseContext<bool> AddOrUpdateSimulateMoment(MomentEntity request);

        ResponseContext<bool> DeleteSimulateMoment(Guid momentId);

        ResponseContext<bool> UpdateImgContent(Guid momentId, long imgId);

        List<DiscussDetailDTO> GetRealUserDiscussDetail(Guid pickUpId);

        PageResult<DiscussDetailDTO> GetSimulateDiscussList(Guid pickUpId);

        ResponseContext<bool> AddDiscuss(DiscussEntity request);

        PageResult<MomentPickUpDTO> SimulateMomentPickUpList(int page, int rows, Guid momentId, int uId, MomentPickUpEnum state);
    }
}
