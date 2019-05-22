using Future.Model.Entity.Today;
using Future.Model.Enum.Sys;
using Future.Service;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Future.Web.Controllers
{
    public class TodayController : BaseController
    {
        private readonly TodayService todayService = SingletonProvider<TodayService>.Instance;

        #region TextGallery
        public IActionResult TextGalleryList()
        {
            return View();
        }
        
        public JsonResult GetTextGalleryList(int page=1, int rows=10)
        {
            try
            {
                var rtn = todayService.GetTextList(page, rows);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetTextGalleryList", ex);
            }
        }

        public JsonResult AddOrUpdateText(string data)
        {
            try
            {
                var request = data.JsonToObject<TextGalleryEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = todayService.AddOrUpdateText(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddOrUpdateText", ex);
            }
        }

        public JsonResult DeleteText(string data)
        {
            try
            {
                long textId = Convert.ToInt16(data);
                var res = todayService.DeleteText(textId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteText", ex);
            }
        }
        #endregion

        #region ImageGallery
        public IActionResult ImageGalleryList()
        {
            return View();
        }

        public JsonResult GetImageGalleryList(int page = 1, int rows = 10)
        {
            try
            {
                var rtn = todayService.GetImageGalleryList(page, rows);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetImageGalleryList", ex);
            }
        }

        public JsonResult AddOrUpdateImage(string data)
        {
            try
            {
                var request = data.JsonToObject<ImgGalleryEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = todayService.AddOrUpdateImage(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddOrUpdateImage", ex);
            }
        }

        public JsonResult DeleteImage(string data)
        {
            try
            {
                long imageId = Convert.ToInt16(data);
                var res = todayService.DeleteImage(imageId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteImage", ex);
            }
        }
        #endregion
    }
}