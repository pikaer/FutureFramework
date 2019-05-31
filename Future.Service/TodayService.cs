using Future.Model.DTO.Today;
using Future.Model.Entity.Today;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Repository;
using Future.Utility;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Future.Service
{
    public class TodayService
    {
        private readonly TodayRepository todayDal = SingletonProvider<TodayRepository>.Instance;
        private readonly SysRepository sysDal = SingletonProvider<SysRepository>.Instance;

        public PageResult<TextGalleryDTO> GetTextList(int pageIndex, int pageSize, string textContent, string textSource, long creater,DateTime? startDateTime, DateTime? endCreateTime)
        {
            var rtn = new PageResult<TextGalleryDTO>();
            var entityList = todayDal.TextGalleryList(pageIndex, pageSize, textContent, textSource, creater, startDateTime, endCreateTime);
            if (entityList.NotEmpty())
            {
                var txtList = entityList.Select(a => new TextGalleryDTO()
                {
                    TextId=a.TextId,
                    TextContent = a.TextContent,
                    TextSource = a.TextSource,
                    Author = a.Author,
                    Remark = a.Remark,
                    CreateUser = GetStaffName(a.CreateUserId),
                    ModifyUser = GetStaffName(a.ModifyUserId),
                    CreateTimeDesc = a.CreateTime.ToString(),
                    ModifyTimeDesc = a.ModifyTime.ToString()
                }).ToList();
                rtn.Rows = txtList;
                rtn.Total = todayDal.TextGalleryListCount();
            }
            return rtn;
        }

        public string GetStaffName(long userId)
        {
            if (userId <= 0)
            {
                return string.Empty;
            }
            var entity = sysDal.Staff(userId);
            if (entity == null)
            {
                return string.Empty;
            }
            return entity.StaffName;
        }

        public ResponseContext<bool> DeleteText(long textId)
        {
            bool success= todayDal.DeleteText(textId);
            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }
        
        public ResponseContext<bool> AddOrUpdateText(TextGalleryEntity req)
        {
            bool success = true;
            if (req.TextId <= 0)
            {
                req.CreateTime = DateTime.Now;
                req.CreateUserId = 1;
                req.ModifyTime = DateTime.Now;
                req.ModifyUserId = 1;
                success = todayDal.IndertTextGallery(req);
            }
            else
            {
                req.ModifyTime = DateTime.Now;
                req.ModifyUserId = 1;
                success = todayDal.UpdateTextGallery(req);
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

        public PageResult<ImgGalleryDTO> GetImageGalleryList(int pageIndex, int pageSize, string imgName, string imgSource, long creater, DateTime? startDateTime, DateTime? endCreateTime)
        {
            var rtn = new PageResult<ImgGalleryDTO>();
            var entityList = todayDal.ImgGalleryList(pageIndex, pageSize, imgName, imgSource, creater, startDateTime, endCreateTime);
            if (entityList.NotEmpty())
            {
                var imageList = entityList.Select(a => new ImgGalleryDTO()
                {
                    ImgId=a.ImgId,
                    ImgName=a.ImgName,
                    ImgSource=a.ImgSource,
                    Url=a.ShortUrl.GetImgPath(),
                    Author =a.Author,
                    Remark=a.Remark,
                    CreateUser = GetStaffName(a.CreateUserId),
                    ModifyUser = GetStaffName(a.ModifyUserId),
                    CreateTimeDesc = a.CreateTime.ToString(),
                    ModifyTimeDesc = a.ModifyTime.ToString(),
                }).ToList();
                rtn.Rows = imageList;
                rtn.Total = todayDal.ImgGalleryListCount();
            }
            return rtn;
        }

        public ResponseContext<bool> DeleteImage(long imageId)
        {
            DeleteLocalImage(imageId);
            bool success = todayDal.DeleteImage(imageId);
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
                request.CreateUserId = 1;
                request.ModifyTime = DateTime.Now;
                request.ModifyUserId = 1;
                success = todayDal.IndertImageGallery(request);
            }
            else
            {
                request.ModifyTime = DateTime.Now;
                request.ModifyUserId = 1;
                success = todayDal.UpdateImageGallery(request);
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
            request.ModifyUserId = 1;
            bool success = todayDal.UpdateShortUrl(request);

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
            var entity = todayDal.ImgGallery(imgId);
            if (entity != null && !entity.ShortUrl.IsNullOrEmpty() &&!string.IsNullOrWhiteSpace(entity.ShortUrl))
            {
                string path = JsonSettingHelper.AppSettings["SetImgPath"] + entity.ShortUrl;
                File.Delete(path);
            }
        }

        public PageResult<HomeInfoDTO> GetHomeInfoList(int pageIndex, int pageSize, DateTime? startDateTime, DateTime? endCreateTime)
        {
            var rtn = new PageResult<HomeInfoDTO>();
            var entityList = todayDal.HomeInfoList(pageIndex, pageSize, startDateTime, endCreateTime);
            if (entityList.NotEmpty())
            {
                var list = entityList.OrderByDescending(x=>x.DisplayStartDateTime).Select(a => new HomeInfoDTO()
                {
                    HomeInfoId = a.HomeInfoId,
                    DisplayStartDateTime = a.DisplayStartDateTime.ToString(),
                    DisplayEndDateTime = a.DisplayEndDateTime.ToString(),
                    Remark = a.Remark,
                    CreateUser = GetStaffName(a.CreateUserId),
                    ModifyUser = GetStaffName(a.ModifyUserId),
                    CreateTimeDesc = a.CreateTime.ToString(),
                    ModifyTimeDesc = a.ModifyTime.ToString()
                }).ToList();
                rtn.Rows = list;
                rtn.Total = todayDal.HomeInfoCount();
            }
            return rtn;
        }

        public PageResult<HomeTextDTO> GetHomeTextList(long homeInfoId)
        {
            var rtn = new PageResult<HomeTextDTO>();
            var entityList = todayDal.HomeTextList(homeInfoId);
            if (entityList.NotEmpty())
            {
                var list = new List<HomeTextDTO>();
                foreach (var item in entityList.OrderBy(a=>a.SortNum))
                {
                    var textEntity = todayDal.TextGallery(item.TextId);
                    if (textEntity == null)
                    {
                        continue;
                    }

                    list.Add(new HomeTextDTO()
                    {
                        HomeTextId = item.HomeTextId,
                        HomeInfoId = item.HomeInfoId,
                        TextId = item.TextId,
                        TextContent = textEntity.TextContent,
                        TextSource = textEntity.TextSource,
                        Author = textEntity.Author,
                        Remark = textEntity.Remark,
                        CreateUser = GetStaffName(item.CreateUserId),
                        CreateTimeDesc = item.CreateTime.ToString()
                    });
                }

                rtn.Rows = list;
                rtn.Total = todayDal.HomeTextCount();
            }
            return rtn;
        }

        public PageResult<HomeImgDTO> GetHomeImgList(long homeInfoId)
        {
            var rtn = new PageResult<HomeImgDTO>();
            var entityList = todayDal.HomeImgList(homeInfoId);
            if (entityList.NotEmpty())
            {
                var list = new List<HomeImgDTO>();
                foreach (var item in entityList.OrderBy(a => a.SortNum))
                {
                    var imgEntity = todayDal.ImgGallery(item.ImgId);
                    if (imgEntity == null)
                    {
                        continue;
                    }

                    list.Add(new HomeImgDTO()
                    {
                        HomeImgId = item.HomeImgId,
                        HomeInfoId = item.HomeInfoId,
                        ImgId = item.ImgId,
                        ImgUrl = imgEntity.ShortUrl.GetImgPath(),
                        ImgSource = imgEntity.ImgSource,
                        Author = imgEntity.Author,
                        Remark = imgEntity.Remark,
                        CreateUser = GetStaffName(item.CreateUserId),
                        CreateTimeDesc = item.CreateTime.ToString()
                    });
                }

                rtn.Rows = list;
                rtn.Total = todayDal.HomeTextCount();
            }
            return rtn;
        }

        public ResponseContext<bool> AddOrUpdateHomeInfo(HomeInfoEntity req)
        {
            var existStartList = todayDal.HomeInfoListByDisplayTime(req.DisplayStartDateTime);
            if (existStartList.NotEmpty())
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.Failure, false,"已经存在时间冲突的数据，请修改展示起始时间!");
            }

            var existEndList = todayDal.HomeInfoListByDisplayTime(req.DisplayEndDateTime);
            if (existEndList.NotEmpty())
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.Failure, false, "已经存在时间冲突的数据，请修改展示起始时间!");
            }

            bool success = true;
            if (req.HomeInfoId <= 0)
            {
                req.CreateTime = DateTime.Now;
                req.CreateUserId = 1;
                req.ModifyTime = DateTime.Now;
                req.ModifyUserId = 1;
                success = todayDal.IndertHomeInfo(req);
            }
            else
            {
                req.ModifyTime = DateTime.Now;
                req.ModifyUserId = 1;
                success = todayDal.UpdateHomeInfo(req);
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

        public ResponseContext<bool> MoveHomeText(long homeTextId, bool toUp)
        {
            var homeTextEntity = todayDal.HomeText(homeTextId);
            if (homeTextEntity == null)
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.DataIsnotExist, false);
            }

            var textList= todayDal.HomeTextList(homeTextEntity.HomeInfoId);
            if (textList.IsNullOrEmpty())
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.DataIsnotExist, false);
            }

            HomeTextEntity oldTextEntity = null;
            if (toUp)
            {
                oldTextEntity = textList.Where(a => a.SortNum < homeTextEntity.SortNum).OrderByDescending(a => a.SortNum).FirstOrDefault();
                
            }
            else
            {
                oldTextEntity = textList.Where(a => a.SortNum > homeTextEntity.SortNum).OrderBy(a => a.SortNum).FirstOrDefault();
            }

            if (oldTextEntity != null)
            {
                int oldSortNum = oldTextEntity.SortNum;
                oldTextEntity.SortNum = homeTextEntity.SortNum;
                homeTextEntity.SortNum = oldSortNum;

                todayDal.UpdateHomeTextSortNum(oldTextEntity);
                todayDal.UpdateHomeTextSortNum(homeTextEntity);
            }
            return new ResponseContext<bool>(true);
        }

        public ResponseContext<bool> MoveHomeImg(long homeImgId, bool toUp)
        {
            var homeImgEntity = todayDal.HomeImg(homeImgId);
            if (homeImgEntity == null)
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.DataIsnotExist, false);
            }

            var imgList = todayDal.HomeImgList(homeImgEntity.HomeInfoId);
            if (imgList.IsNullOrEmpty())
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.DataIsnotExist, false);
            }

            HomeImgEntity oldImgEntity = null;
            if (toUp)
            {
                oldImgEntity = imgList.Where(a => a.SortNum < homeImgEntity.SortNum).OrderByDescending(a => a.SortNum).FirstOrDefault();

            }
            else
            {
                oldImgEntity = imgList.Where(a => a.SortNum > homeImgEntity.SortNum).OrderBy(a => a.SortNum).FirstOrDefault();
            }

            if (oldImgEntity != null)
            {
                int oldSortNum = oldImgEntity.SortNum;
                oldImgEntity.SortNum = homeImgEntity.SortNum;
                homeImgEntity.SortNum = oldSortNum;

                todayDal.UpdateHomeImgSortNum(oldImgEntity);
                todayDal.UpdateHomeImgSortNum(homeImgEntity);
            }
            return new ResponseContext<bool>(true);
        }

        public ResponseContext<bool> IndertHomeText(HomeTextEntity request)
        {
            int currentSortNum = 1;
            var list = todayDal.HomeTextList(request.HomeInfoId);
            if (list.NotEmpty())
            {
                currentSortNum = list.OrderByDescending(a => a.SortNum).First().SortNum+1;
            }

            request.SortNum = currentSortNum;
            request.CreateTime = DateTime.Now;
            request.CreateUserId = 1;
            bool success = todayDal.IndertHomeText(request);
            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public ResponseContext<bool> IndertHomeImg(HomeImgEntity request)
        {
            int currentSortNum = 1;
            var list = todayDal.HomeImgList(request.HomeInfoId);
            if (list.NotEmpty())
            {
                currentSortNum = list.OrderByDescending(a => a.SortNum).First().SortNum + 1;
            }

            request.SortNum = currentSortNum;
            request.CreateTime = DateTime.Now;
            request.CreateUserId = 1;
            bool success = todayDal.IndertHomeImg(request);
            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public ResponseContext<bool> DeleteHomeInfo(long homeInfoId)
        {
            todayDal.DeleteHomeTextByHomeInfoId(homeInfoId);
            todayDal.DeleteHomeImgByHomeInfoId(homeInfoId);
            bool success = todayDal.DeleteHomeInfo(homeInfoId);
            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }
        
        public ResponseContext<bool> DeleteHomeText(long homeTextId)
        {
            bool success = todayDal.DeleteHomeTextByHomeTextId(homeTextId);
            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public ResponseContext<bool> DeleteHomeImg(long homeImgId)
        {
            bool success = todayDal.DeleteHomeImgByHomeImgId(homeImgId);
            if (success)
            {
                return new ResponseContext<bool>(success);
            }
            else
            {
                return new ResponseContext<bool>(false, ErrCodeEnum.InnerError, success);
            }
        }

        public ResponseContext<GetHomeInfoResponse> GetHomeInfo(RequestContext<GetHomeInfoRequest> request)
        {
            var response = new ResponseContext<GetHomeInfoResponse>
            {
                Content = new GetHomeInfoResponse()
                {
                    TodayImgList=new List<TodayContentType>(),
                    TodayTextList=new List<TodayContentType>()
                }
            };
            var homeInfo= todayDal.HomeInfo(DateTime.Now);
            if (homeInfo == null)
            {
                return response;
            }

            var textList= todayDal.HomeTextList(homeInfo.HomeInfoId);
            if (textList.NotEmpty())
            {
               foreach(var item in textList)
               {
                    var text= todayDal.TextGallery(item.TextId);
                    if (text == null)
                    {
                        continue;
                    }
                    response.Content.TodayTextList.Add(new TodayContentType()
                    {
                        Author = text.Author.Trim(),
                        Content = text.TextContent.Trim()
                    });
               }
            }

            var imgList= todayDal.HomeImgList(homeInfo.HomeInfoId);
            if (imgList.NotEmpty())
            {
                foreach (var item in imgList)
                {
                    var img = todayDal.ImgGallery(item.ImgId);
                    if (img == null)
                    {
                        continue;
                    }
                    response.Content.TodayImgList.Add(new TodayContentType()
                    {
                        Author = img.Author.Trim(),
                        Content = img.ShortUrl.GetImgPath()
                    });
                }
            }
            return response;
        }
    }
}
