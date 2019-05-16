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
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
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
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
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
                return ErrorJsonResult(ErrCodeEnum.InnerError, ex);
            }
        }
    }
}