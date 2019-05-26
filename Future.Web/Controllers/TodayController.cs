using Future.Model.Entity.Today;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

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

        public JsonResult UploadLoadImageFlile()
        {
            try
            {
                var response = new ResponseContext<string>(string.Empty);

                var uploadfile = Request.Form.Files[0];

                var filePath = JsonSettingHelper.AppSettings["SetImgPath"];

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                if (uploadfile == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InnerError, "UploadLoadImageFlile");
                }

                //文件后缀
                var fileExtension = Path.GetExtension(uploadfile.FileName);

                //判断后缀是否是图片
                const string fileFilt = "|.gif|.jpg|.php|.jsp|.jpeg|.png|";
                if (fileExtension == null)
                {
                    response = new ResponseContext<string>(string.Empty)
                    {
                        Code = ErrCodeEnum.InnerError,
                        ResultMessage = "上传的文件没有后缀"
                    };
                    return new JsonResult(response);
                }
                if (!fileFilt.Contains(fileExtension))
                {
                    response = new ResponseContext<string>(string.Empty)
                    {
                        Code = ErrCodeEnum.Failure,
                        ResultMessage = "上传的文件不是图片"
                    };
                    return new JsonResult(response);
                }

                //判断文件大小    
                long length = uploadfile.Length;
                if (length > 1024 * 1024 * 10) //1M
                {
                    response = new ResponseContext<string>(string.Empty)
                    {
                        Code = ErrCodeEnum.Failure,
                        ResultMessage = "上传的文件不能大于10M"
                    };
                    return new JsonResult(response);
                }

                var strDateTime = DateTime.Now.ToString("yyyyMMdd"); //取得时间字符串
                var strRan = Guid.NewGuid().ToString(); //生成随机数
                var saveName = string.Format("{0}_{1}_{2}{3}", strDateTime, length, strRan, fileExtension);

                using (FileStream fs = System.IO.File.Create(filePath + saveName))
                {
                    uploadfile.CopyTo(fs);
                    fs.Flush();
                }

                response = new ResponseContext<string>(saveName);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UpLoadImg", ex);
            }
        }

        public JsonResult UpdateShortUrl(string data)
        {
            try
            {
                var request = data.JsonToObject<ImgGalleryEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = todayService.UpdateShortUrl(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UpdateShortUrl", ex);
            }
        }
        #endregion

        #region TodayHappyIndex
        public IActionResult TodayHappyIndex()
        {
            return View();
        }
        public JsonResult GetHomeInfoList(int page = 1, int rows = 10)
        {
            try
            {
                var rtn = todayService.GetHomeInfoList(page, rows);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetHomeInfoList", ex);
            }
        }

        public JsonResult GetHomeTextList()
        {
            try
            {
                long homeInfoId = Convert.ToInt16(Request.Form["HomeInfoId"]);
                if (homeInfoId <= 0)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody, "GetHomeTextList");
                }
                var rtn = todayService.GetHomeTextList(homeInfoId);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetHomeTextList", ex);
            }
        }

        public JsonResult GetHomeImgList()
        {
            try
            {
                long homeInfoId = Convert.ToInt16(Request.Form["HomeInfoId"]);
                if (homeInfoId <= 0)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody, "GetHomeImgList");
                }
                var rtn = todayService.GetHomeImgList(homeInfoId);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetHomeImgList", ex);
            }
        }

        public JsonResult AddOrUpdateHomeInfo(string data)
        {
            try
            {
                var request = data.JsonToObject<HomeInfoEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = todayService.AddOrUpdateHomeInfo(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddOrUpdateHomeInfo", ex);
            }
        }

        public JsonResult DeleteHomeInfo(string data)
        {
            try
            {
                long homeInfoId = Convert.ToInt16(data);
                if (homeInfoId <= 0)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody, "DeleteHomeInfo");
                }
                var res = todayService.DeleteHomeInfo(homeInfoId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteHomeInfo", ex);
            }
        }

        public JsonResult IndertHomeText(string data)
        {
            try
            {
                var request = data.JsonToObject<HomeTextEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code, "IndertHomeText");
                }
                var res = todayService.IndertHomeText(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "IndertHomeText", ex);
            }
        }

        /// <summary>
        /// 文本内容顺序交换
        /// </summary>
        /// <param name="homeTextId">首页文本Id</param>
        /// <param name="toUp">true：上升一个，false:下降一个</param>
        /// <returns></returns>
        public JsonResult MoveHomeText(long homeTextId,bool toUp)
        {
            try
            {
                if (homeTextId<=0)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code, "MoveHomeText");
                }
                var res = todayService.MoveHomeText(homeTextId, toUp);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "MoveHomeText", ex);
            }
        }

        /// <summary>
        /// 图片内容顺序交换
        /// </summary>
        /// <param name="homeImgId">首页图片Id</param>
        /// <param name="toUp">true：上升一个，false:下降一个</param>
        /// <returns></returns>
        public JsonResult MoveHomeImg(long homeImgId, bool toUp)
        {
            try
            {
                if (homeImgId <= 0)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code, "MoveHomeText");
                }
                var res = todayService.MoveHomeImg(homeImgId, toUp);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "MoveHomeImg", ex);
            }
        }

        public JsonResult DeleteHomeText(string data)
        {
            try
            {
                long homeTextId = Convert.ToInt16(data);
                if (homeTextId <= 0)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody, "DeleteHomeText");
                }
                var res = todayService.DeleteHomeText(homeTextId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteHomeText", ex);
            }
        }

        public JsonResult IndertHomeImg(string data)
        {
            try
            {
                var request = data.JsonToObject<HomeImgEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code, "IndertHomeImg");
                }
                var res = todayService.IndertHomeImg(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "IndertHomeImg", ex);
            }
        }

        public JsonResult DeleteHomeImg(string data)
        {
            try
            {
                long homeImgId = Convert.ToInt16(data);
                if (homeImgId <= 0)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody, "DeleteHomeImg");
                }
                var res = todayService.DeleteHomeImg(homeImgId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteHomeImg", ex);
            }
        }
        #endregion
    }
}