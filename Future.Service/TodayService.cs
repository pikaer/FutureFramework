using Future.Model.DTO.Today;
using Future.Model.Entity.Sys;
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
    public class TodayService
    {
        private readonly SysRepository sysDal = SingletonProvider<SysRepository>.Instance;
        
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
            if (entityList.NotEmpty())
            {
                var imageList = entityList.Select(a => new ImgGalleryDTO()
                {
                    ImgId=a.ImgId,
                    ImgName=a.ImgName,
                    Url=a.ShortUrl.GetImgPath(),
                    Remark=a.Remark,
                    CreateUser = GetStaffName(a.CreateUserId),
                    ModifyUser = GetStaffName(a.ModifyUserId),
                    CreateTimeDesc = a.CreateTime.ToString(),
                    ModifyTimeDesc = a.ModifyTime.ToString(),
                }).ToList();
                rtn.Rows = imageList;
                rtn.Total = sysDal.ImgGalleryListCount();
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
        
    }
}
