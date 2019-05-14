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
        
    }
}