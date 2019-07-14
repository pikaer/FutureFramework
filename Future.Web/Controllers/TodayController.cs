using Future.Model.DTO.Today;
using Future.Model.Entity.Letter;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Letter;
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
                int uId = 0;
                if (!string.IsNullOrEmpty(Request.Form["UId"]))
                {
                    uId = Convert.ToInt16(Request.Form["UId"]);
                }
                var gender = GenderEnum.All;
                if (!Request.Form["Gender"].IsNullOrEmpty())
                {
                    gender = (GenderEnum)Convert.ToInt16(Request.Form["gender"]);
                }
                string nickName = Request.Form["NickName"];

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

        public JsonResult UpdateAvatarUrl(long uId, long imgId)
        {
            try
            {
                var res = todayService.UpdateAvatarUrl(uId, imgId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UpdateAvatarUrl", ex);
            }
        }
        #endregion

        #region SimulateUserMomentList(模拟用户发布的动态)
        public IActionResult SimulateUserMomentList()
        {
            return View();
        }
        
        public JsonResult GetSimulateUserPublishList()
        {
            try
            {
                int page = Convert.ToInt16(Request.Form["page"]);
                int rows = Convert.ToInt16(Request.Form["rows"]);
                int uId = 0;
                if (!string.IsNullOrEmpty(Request.Form["UId"]))
                {
                    uId = Convert.ToInt16(Request.Form["UId"]);
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
                var state = MomentStateEnum.All;
                if (!string.IsNullOrEmpty(Request.Form["MomentState"]))
                {
                    state = (MomentStateEnum)Convert.ToInt16(Request.Form["MomentState"]);
                }

                var rtn = todayService.GetSimulateUserPublishList(page, rows, uId, state,startDateTime, endCreateTime);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetSimulateUserPublishList", ex);
            }
        }

        public JsonResult AddOrUpdateSimulateMoment(string data)
        {
            try
            {
                var request = data.JsonToObject<MomentEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = todayService.AddOrUpdateSimulateMoment(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddOrUpdateSimulateMoment", ex);
            }
        }

        public JsonResult UpdateImgContent(Guid momentId, long imgId)
        {
            try
            {
                var res = todayService.UpdateImgContent(momentId, imgId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UpdateImgContent", ex);
            }
        }

        public JsonResult DeleteSimulateMoment(string data)
        {
            try
            {
                var request = data.JsonToObject<MomentEntity>();
                var res = todayService.DeleteSimulateMoment(request.MomentId);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteSimulateMoment", ex);
            }
        }

        public JsonResult SimulateMomentPickUpList()
        {
            try
            {
                if (Request.Form["MomentId"].IsNullOrEmpty())
                {
                    return new JsonResult(new PageResult<MomentPickUpDTO>());
                }
                int page = Convert.ToInt16(Request.Form["page"]);
                int rows = Convert.ToInt16(Request.Form["rows"]);
                int uId = 0;
                if (!string.IsNullOrEmpty(Request.Form["UId"]))
                {
                    uId = Convert.ToInt16(Request.Form["UId"]);
                }
                var state = MomentPickUpEnum.All;
                if (!string.IsNullOrEmpty(Request.Form["PickUpState"]))
                {
                    state = (MomentPickUpEnum)Convert.ToInt16(Request.Form["PickUpState"]);
                }

                var momentId = new Guid(Request.Form["MomentId"]);

                var rtn = todayService.SimulateMomentPickUpList(page, rows, momentId, uId, state);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "SimulateMomentPickUpList", ex);
            }
        }

        #endregion

        #region SimulateDiscussList 模拟用户动态评论列表
        public IActionResult SimulateDiscussList()
        {
            return View();
        }

        public JsonResult GetSimulateDiscussList()
        {
            try
            {
                if (Request.Form["PickUpId"].IsNullOrEmpty())
                {
                    return new JsonResult(new PageResult<DiscussDetailDTO>());
                }
                var pickUpId = new Guid(Request.Form["PickUpId"]);

                var rtn = todayService.GetSimulateDiscussList(pickUpId);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetSimulateDiscussList", ex);
            }
        }

        public JsonResult AddDiscuss(string data)
        {
            try
            {
                var request = data.JsonToObject<DiscussEntity>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                var res = todayService.AddDiscuss(request);
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "AddDiscuss", ex);
            }
        }
        #endregion

        #region RealUserList(真实注册用户)
        public IActionResult RealUserList()
        {
            return View();
        }

        public JsonResult GetRealUserList()
        {
            try
            {
                int page = Convert.ToInt16(Request.Form["page"]);
                int rows = Convert.ToInt16(Request.Form["rows"]);
                int uId = 0;
                if (!string.IsNullOrEmpty(Request.Form["UId"]))
                {
                    uId = Convert.ToInt16(Request.Form["UId"]);
                }
                var gender = GenderEnum.All;
                if (!Request.Form["Gender"].IsNullOrEmpty())
                {
                    gender = (GenderEnum)Convert.ToInt16(Request.Form["gender"]);
                }
                string nickName = Request.Form["NickName"];
                string openId= Request.Form["OpenId"];

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

                var rtn = todayService.GetRealUserList(page, rows, uId, nickName, openId, gender, startDateTime, endCreateTime);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetRealUserList", ex);
            }
        }

        #endregion

        #region RealUserMomentList(真实注册用户动态)
        public IActionResult RealUserMomentList()
        {
            return View();
        }

        public JsonResult GetRealUserPickUpList()
        {
            try
            {
                int page = Convert.ToInt16(Request.Form["page"]);
                int rows = Convert.ToInt16(Request.Form["rows"]);
                int uId = 0;
                if (!string.IsNullOrEmpty(Request.Form["UId"]))
                {
                    uId = Convert.ToInt16(Request.Form["UId"]);
                }

                var pickType = PickUpTypeEnum.All;
                if (!string.IsNullOrEmpty(Request.Form["PickUpType"]))
                {
                    pickType = (PickUpTypeEnum)Convert.ToInt16(Request.Form["PickUpType"]);
                }

                var rtn = todayService.GetRealUserPickUpList(page, rows, uId, pickType);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetRealUserPickUpList", ex);
            }
        }

        public JsonResult GetRealUserDiscussDetail()
        {
            try
            {
                Guid pickUpId = new Guid(Request.Form["PickUpId"]);
                
                var rtn = todayService.GetRealUserDiscussDetail(pickUpId);
                return new JsonResult(rtn);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "GetRealUserDiscussDetail", ex);
            }
        }
        #endregion
    }
}