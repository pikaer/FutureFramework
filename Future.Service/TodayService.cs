using Future.Model.DTO.Today;
using Future.Model.Entity.Letter;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Letter;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Repository;
using Future.Utility;
using Infrastructure;
using System;
using System.IO;
using System.Linq;

namespace Future.Service
{
    /// <summary>
    /// 今日份小程序Web端后台业务层
    /// </summary>
    public class TodayService
    {
        private readonly SysRepository sysDal = SingletonProvider<SysRepository>.Instance;

        private readonly LetterRepository letterDal = SingletonProvider<LetterRepository>.Instance;

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
            if (entityList!=null&&entityList.Item1.NotEmpty())
            {
                var imageList = entityList.Item1.Select(a => new ImgGalleryDTO()
                {
                    ImgId=a.ImgId,
                    ImgName=a.ImgName,
                    Url=a.ShortUrl.GetImgPath(),
                    Remark=a.Remark,
                    UseCount=a.UseCount,
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

        public PageResult<SimulateUserDTO> GetSimulateUserList(int pageIndex, int pageSize, long uId, string nickName, GenderEnum gender,long creater, DateTime? startDateTime, DateTime? endCreateTime)
        {
            var rtn = new PageResult<SimulateUserDTO>();
            var entityList = letterDal.GetSimulateUserList(pageIndex, pageSize, uId, nickName, gender,creater, startDateTime, endCreateTime);
            if (entityList!=null&&entityList.Item1.NotEmpty())
            {
                var imageList = entityList.Item1.Select(a => new SimulateUserDTO()
                {
                    UId = a.UId,
                    GenderDesc = a.Gender.ToDescription(),
                    SchoolTypeDesc = a.SchoolType.ToDescription(),
                    LiveStateDesc= a.LiveState.ToDescription(),
                    Gender = a.Gender,
                    SchoolType = a.SchoolType,
                    LiveState= a.LiveState,
                    EntranceDate = a.EntranceDate,
                    SchoolName = a.SchoolName,
                    NickName = a.NickName,
                    BirthDate = a.BirthDate,
                    Province = a.Province,
                    City = a.City,
                    Area = a.Area,
                    Signature = a.Signature,
                    HeadPhotoPath =a.HeadPhotoPath.GetImgPath(),
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
            if (entity != null && !entity.ShortUrl.IsNullOrEmpty() &&!string.IsNullOrWhiteSpace(entity.ShortUrl))
            {
                string path = JsonSettingHelper.AppSettings["SetImgPath"] + entity.ShortUrl;
                File.Delete(path);
            }
        }

        public ResponseContext<bool> AddOrUpdateSimulateUser(LetterUserEntity request)
        {
            bool success = true;
            if (request.UId <= 0)
            {
                request.CreateTime = DateTime.Now;
                request.UserType =UserTypeEnum.SimulationUser;
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
    }
}
