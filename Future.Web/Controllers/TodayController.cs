using Future.Model.Entity.Letter;
using Future.Model.Entity.Sys;
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
        
        #region ImageGallery
        public IActionResult ImageGalleryList()
        {
            return View();
        }

        public JsonResult GetImageGalleryList()
        {
            try
            {
                int page = Convert.ToInt16(Request.Form["page"]);
                int rows = Convert.ToInt16(Request.Form["rows"]);
                string imgName = Request.Form["ImgName"];

                long creater = 0;
                if (!string.IsNullOrWhiteSpace(Request.Form["Creater"]))
                {
                    creater = Convert.ToInt64(Request.Form["Creater"]);
                }
                var startDateTime = new DateTime();
                if (!string.IsNullOrWhiteSpace(Request.Form["StartCreateTime"]))
                {
                    startDateTime = Convert.ToDateTime(Request.Form["StartCreateTime"]);
                }
                var endCreateTime = new DateTime();
                if (!string.IsNullOrWhiteSpace(Request.Form["EndCreateTime"]))
                {
                    endCreateTime = Convert.ToDateTime(Request.Form["EndCreateTime"]);
                }
                var rtn = todayService.GetImageGalleryList(page, rows, imgName, creater, startDateTime, endCreateTime);
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
                request.CreateUserId = CurrentUserInfo.StaffId;
                request.ModifyUserId = CurrentUserInfo.StaffId;
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
                request.ModifyUserId = CurrentUserInfo.StaffId;
                var res = todayService.UpdateShortUrl(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UpdateShortUrl", ex);
            }
        }
        #endregion

        #region SimulateUserList(模拟用户)
        public IActionResult SimulateUserList()
        {
            return View();
        }

        public JsonResult GetSimulateUserList()
        {
            try
            {
                int page = Convert.ToInt16(Request.Form["page"]);
                int rows = Convert.ToInt16(Request.Form["rows"]);
                int uId = Convert.ToInt16(Request.Form["uId"]);
                var gender = GenderEnum.All;
                if (!Request.Form["gender"].IsNullOrEmpty())
                {
                    gender = (GenderEnum)Convert.ToInt16(Request.Form["gender"]);
                }
                string nickName = Request.Form["nickName"];

                long creater = 0;
                if (!string.IsNullOrWhiteSpace(Request.Form["Creater"]))
                {
                    creater = Convert.ToInt64(Request.Form["Creater"]);
                }
                var startDateTime = new DateTime();
                if (!string.IsNullOrWhiteSpace(Request.Form["StartCreateTime"]))
                {
                    startDateTime = Convert.ToDateTime(Request.Form["StartCreateTime"]);
                }
                var endCreateTime = new DateTime();
                if (!string.IsNullOrWhiteSpace(Request.Form["EndCreateTime"]))
                {
                    endCreateTime = Convert.ToDateTime(Request.Form["EndCreateTime"]);
                }
                var rtn = todayService.GetSimulateUserList(page, rows, uId, nickName, gender, creater, startDateTime, endCreateTime);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetSimulateUserList", ex);
            }
        }

        public JsonResult AddOrUpdateSimulateUser(string data)
        {
            try
            {
                var request = data.JsonToObject<LetterUserEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = todayService.AddOrUpdateSimulateUser(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddOrUpdateSimulateUser", ex);
            }
        }

        public JsonResult DeleteLetterUser(string data)
        {
            try
            {
                long uId = Convert.ToInt16(data);
                var res = todayService.DeleteLetterUser(uId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteLetterUser", ex);
            }
        }
        #endregion
    }
}