using Future.Model.DTO.Today;
using Future.Model.Entity.Letter;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Letter;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Repository;
using Future.Service.Interface;
using Future.Utility;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Future.Service.Implement
{
    public class TodayBiz: ITodayBiz
    {
        private readonly SysRepository sysDal = SingletonProvider<SysRepository>.Instance;

        private readonly LetterRepository letterDal = SingletonProvider<LetterRepository>.Instance;

        private readonly IUserBiz userBiz = SingletonProvider<UserBiz>.Instance;

        public string GetStaffName(long userId)
        {
            if (userId <= 0)
            {
                return string.Empty;
            }
            var entity = sysDal.StaffByUId(userId);
            if (entity == null)
            {
                return string.Empty;
            }
            return entity.StaffName;
        }

        public PageResult<ImgGalleryDTO> GetImageGalleryList(int pageIndex, int pageSize, string imgName, long creater, DateTime? startDateTime, DateTime? endCreateTime)
        {
            var rtn = new PageResult<ImgGalleryDTO>();
            var entityList = sysDal.ImgGalleryList(pageIndex, pageSize, imgName, creater, startDateTime, endCreateTime);
            if (entityList != null && entityList.Item1.NotEmpty())
            {
                var imageList = entityList.Item1.Select(a => new ImgGalleryDTO()
                {
                    ImgId = a.ImgId,
                    ImgName = a.ImgName,
                    Url = a.ShortUrl.GetImgPath(),
                    Remark = a.Remark,
                    UseCount = a.UseCount,
                    CreateUser = GetStaffName(a.CreateUserId),
                    ModifyUser = GetStaffName(a.ModifyUserId),
                    CreateTimeDesc = a.CreateTime.ToString(),
                    ModifyTimeDesc = a.ModifyTime.ToString(),
                }).ToList();
                rtn.Rows = imageList;
                rtn.Total = entityList.Item2;
            }
            return rtn;
        }

        public ResponseContext<bool> DeleteImage(long imageId)
        {
            DeleteLocalImage(imageId);
            bool success = sysDal.DeleteImage(imageId);
            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public ResponseContext<bool> AddOrUpdateImage(ImgGalleryEntity request)
        {
            bool success = true;
            if (request.ImgId <= 0)
            {
                request.CreateTime = DateTime.Now;
                request.ModifyTime = DateTime.Now;
                success = sysDal.IndertImageGallery(request);
            }
            else
            {
                request.ModifyTime = DateTime.Now;
                success = sysDal.UpdateImageGallery(request);
            }

            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public ResponseContext<bool> UpdateShortUrl(ImgGalleryEntity request)
        {
            DeleteLocalImage(request.ImgId);
            request.ModifyTime = DateTime.Now;
            bool success = sysDal.UpdateShortUrl(request);

            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public PageResult<SimulateUserDTO> GetSimulateUserList(int pageIndex, int pageSize, long uId, string nickName, GenderEnum gender, long creater, DateTime? startDateTime, DateTime? endCreateTime)
        {
            var rtn = new PageResult<SimulateUserDTO>();
            var entityList = letterDal.GetSimulateUserList(pageIndex, pageSize, uId, nickName, gender, creater, startDateTime, endCreateTime);
            if (entityList != null && entityList.Item1.NotEmpty())
            {
                var imageList = entityList.Item1.Select(a => new SimulateUserDTO()
                {
                    UId = a.UId,
                    GenderDesc = a.Gender.ToDescription(),
                    SchoolTypeDesc = a.SchoolType.ToDescription(),
                    LiveStateDesc = a.LiveState.ToDescription(),
                    Gender = a.Gender,
                    SchoolType = a.SchoolType,
                    LiveState = a.LiveState,
                    EntranceDate = a.EntranceDate,
                    SchoolName = a.SchoolName,
                    NickName = a.NickName,
                    BirthDate = a.BirthDate,
                    Province = a.Province,
                    City = a.City,
                    Area = a.Area,
                    Signature = a.Signature,
                    HeadPhotoPath = a.HeadPhotoPath.GetImgPath(),
                    CreateTimeDesc = a.CreateTime.ToString()
                }).ToList();
                rtn.Rows = imageList;
                rtn.Total = entityList.Item2;
            }
            return rtn;
        }

        public PageResult<RealUserDTO> GetRealUserList(int pageIndex, int pageSize, long uId, string nickName, string openId, GenderEnum gender, DateTime? startDateTime, DateTime? endCreateTime)
        {
            var rtn = new PageResult<RealUserDTO>();
            var entityList = letterDal.GetRealUserList(pageIndex, pageSize, uId, nickName, openId, gender, startDateTime, endCreateTime);
            if (entityList != null && entityList.Item1.NotEmpty())
            {
                var imageList = entityList.Item1.Select(a => new RealUserDTO()
                {
                    UId = a.UId,
                    OpenId = a.OpenId,
                    GenderDesc = a.Gender.ToDescription(),
                    SchoolTypeDesc = a.SchoolType.ToDescription(),
                    LiveStateDesc = a.LiveState.ToDescription(),
                    Gender = a.Gender,
                    SchoolType = a.SchoolType,
                    LiveState = a.LiveState,
                    EntranceDate = a.EntranceDate,
                    SchoolName = a.SchoolName,
                    NickName = a.NickName,
                    BirthDate = a.BirthDate,
                    Province = a.Province,
                    City = a.City,
                    Area = a.Area,
                    Signature = a.Signature,
                    HeadPhotoPath = a.HeadPhotoPath.GetImgPath(),
                    CreateTimeDesc = a.CreateTime.ToString()
                }).ToList();
                rtn.Rows = imageList;
                rtn.Total = entityList.Item2;
            }
            return rtn;
        }

        private void DeleteLocalImage(long imgId)
        {
            if (imgId <= 0)
            {
                return;
            }
            var entity = sysDal.ImgGallery(imgId);
            if (entity != null && !entity.ShortUrl.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(entity.ShortUrl))
            {
                string path = JsonSettingHelper.AppSettings["SetImgPath"] + entity.ShortUrl;
                File.Delete(path);
            }
        }

        public ResponseContext<bool> AddOrUpdateSimulateUser(LetterUserEntity request)
        {
            bool success;
            if (request.UId <= 0)
            {
                request.CreateTime = DateTime.Now;
                request.UserType = UserTypeEnum.SimulationUser;
                request.UpdateTime = DateTime.Now;
                success = letterDal.InsertLetterUser(request);
            }
            else
            {
                request.UpdateTime = DateTime.Now;
                success = letterDal.UpdateLetterUser(request);
            }

            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public ResponseContext<bool> DeleteLetterUser(long uId)
        {
            bool success = letterDal.UpdateLetterUserDelete(uId);
            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public ResponseContext<bool> UpdateAvatarUrl(long uId, long imgId)
        {
            var img = sysDal.ImgGallery(imgId);
            if (img == null)
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, false, "图片不存在");
            }

            bool success = letterDal.UpdateAvatarUrl(img.ShortUrl, uId);
            if (success)
            {
                sysDal.UpdateImgUseCount(imgId);
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public PageResult<PickUpListDTO> GetRealUserPickUpList(int page, int rows, int uId, PickUpTypeEnum pickType)
        {
            var rtn = new PageResult<PickUpListDTO>();
            var pickUpList = letterDal.PickUpListByParam(uId, page, rows, pickType);
            if (pickUpList != null && pickUpList.Item1.NotEmpty())
            {
                var pickUps = new List<PickUpListDTO>();
                foreach (var item in pickUpList.Item1)
                {
                    var pickUpUser = userBiz.LetterUserByUId(item.MomentUId);
                    if (pickUpUser == null)
                    {
                        continue;
                    }
                    var moment = letterDal.GetMoment(item.MomentId);
                    if (moment == null)
                    {
                        continue;
                    }
                    var dto = new PickUpListDTO()
                    {
                        PickUpId = item.PickUpId,
                        UId = item.MomentUId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = moment.TextContent,
                        ImgContent = moment.ImgContent.GetImgPath(),
                        CreateTime = moment.CreateTime.GetDateDesc()
                    };

                    pickUps.Add(dto);
                }
                rtn.Rows = pickUps;
                rtn.Total = pickUpList.Item2;
            }
            return rtn;
        }

        public PageResult<PublishMomentListDTO> GetSimulateUserPublishList(int page, int rows, long uId, MomentStateEnum state, DateTime startDateTime, DateTime endCreateTime)
        {
            var rtn = new PageResult<PublishMomentListDTO>();
            var momentList = letterDal.GetMomentList(page, rows, uId, state, startDateTime, endCreateTime);
            if (momentList != null && momentList.Item1.NotEmpty())
            {
                rtn.Rows = momentList.Item1.Select(a => new PublishMomentListDTO()
                {
                    MomentId = a.MomentId,
                    TextContent = a.TextContent.Trim(),
                    ImgContent = a.ImgContent.GetImgPath(),
                    IsDelete = a.IsDelete,
                    ReplyCount = a.ReplyCount,
                    CreateTime = a.CreateTime.GetDateDesc(),
                    CanEdit = a.ReplyCount <= 0
                }).ToList();
                rtn.Total = momentList.Item2;
            }
            return rtn;
        }

        public ResponseContext<bool> AddOrUpdateSimulateMoment(MomentEntity request)
        {
            bool success = true;
            if (request.MomentId == new Guid())
            {
                request.UpdateTime = DateTime.Now;
                request.MomentId = Guid.NewGuid();
                success = letterDal.InsertMoment(request);
            }
            else
            {
                var moment = letterDal.GetMoment(request.MomentId);
                if (moment == null)
                {
                    return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, false, "动态不存在，请联系管理员");
                }
                if (moment.ReplyCount > 0)
                {
                    return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, false, "该动态已被订阅，不能修改!");
                }
                request.UpdateTime = DateTime.Now;
                success = letterDal.UpdateMoment(request);
            }
            return new ResponseContext<bool>(success);
        }

        public ResponseContext<bool> DeleteSimulateMoment(Guid momentId)
        {
            bool success = letterDal.DeleteMoment(momentId);
            return new ResponseContext<bool>(success);
        }

        public ResponseContext<bool> UpdateImgContent(Guid momentId, long imgId)
        {
            if (momentId == new Guid())
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, false, "动态不存在，请联系管理员");
            }
            var img = sysDal.ImgGallery(imgId);
            if (img == null)
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, false, "图片不存在");
            }

            bool success = letterDal.UpdateMomentImgContent(img.ShortUrl, momentId);
            if (success)
            {
                sysDal.UpdateImgUseCount(imgId);
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public List<DiscussDetailDTO> GetRealUserDiscussDetail(Guid pickUpId)
        {
            var rtn = new List<DiscussDetailDTO>();
            var discussList = letterDal.DiscussList(pickUpId);
            if (discussList.NotEmpty())
            {
                foreach (var item in discussList)
                {
                    var pickUpUser = userBiz.LetterUserByUId(item.UId);
                    if (pickUpUser == null)
                    {
                        continue;
                    }
                    var dto = new DiscussDetailDTO()
                    {
                        PickUpUId = item.UId,
                        HeadImgPath = pickUpUser.HeadPhotoPath.GetImgPath(),
                        NickName = pickUpUser.NickName,
                        TextContent = item.DiscussContent,
                        RecentChatTime = item.UpdateTime.Value.GetDateDesc()
                    };
                    rtn.Add(dto);
                }
            }

            return rtn;
        }

        public PageResult<DiscussDetailDTO> GetSimulateDiscussList(Guid pickUpId)
        {
            var rtn = new PageResult<DiscussDetailDTO>();
            var list = letterDal.DiscussList(pickUpId);
            if (list.NotEmpty())
            {
                var rows = new List<DiscussDetailDTO>();
                foreach (var item in list)
                {
                    var user = userBiz.LetterUserByUId(item.UId);
                    if (user == null)
                    {
                        continue;
                    }
                    rows.Add(new DiscussDetailDTO()
                    {
                        PickUpUId = item.UId,
                        HeadImgPath = user.HeadPhotoPath.GetImgPath(),
                        NickName = user.NickName,
                        TextContent = item.DiscussContent.Trim(),
                        RecentChatTime = item.CreateTime.GetDateDesc(),
                        HasRead = item.HasRead
                    });
                }
                rtn.Rows = rows;
                rtn.Total = rows.Count;
            }
            return rtn;
        }

        public ResponseContext<bool> AddDiscuss(DiscussEntity request)
        {
            request.CreateTime = DateTime.Now;
            request.UpdateTime = DateTime.Now;
            request.DiscussId = Guid.NewGuid();
            return new ResponseContext<bool>(letterDal.InsertDiscuss(request));
        }

        public PageResult<MomentPickUpDTO> SimulateMomentPickUpList(int page, int rows, Guid momentId, int uId, MomentPickUpEnum state)
        {
            var rtn = new PageResult<MomentPickUpDTO>();
            var pickUpList = letterDal.GetPickUpList(page, rows, momentId, uId, state);
            if (pickUpList != null && pickUpList.Item1.NotEmpty())
            {
                var list = new List<MomentPickUpDTO>();
                foreach (var item in pickUpList.Item1)
                {
                    var user = userBiz.LetterUserByUId(item.PickUpUId);
                    if (user == null)
                    {
                        continue;
                    }
                    var discussList = letterDal.DiscussList(item.PickUpId);
                    list.Add(new MomentPickUpDTO()
                    {
                        MomentId = item.MomentId,
                        UId = item.PickUpUId,
                        PickUpId = item.PickUpId,
                        NickName = user.NickName,
                        HeadPhotoPath = user.HeadPhotoPath.GetImgPath(),
                        Gender = user.Gender,
                        GenderDesc = user.Gender.ToDescription(),
                        DiscussCount = discussList.IsNullOrEmpty() ? 0 : discussList.Count,
                        CreateTime = item.CreateTime.GetDateDesc()
                    });
                    rtn.Total = pickUpList.Item2;
                    rtn.Rows = list;
                }
            }
            return rtn;
        }
    }
}

