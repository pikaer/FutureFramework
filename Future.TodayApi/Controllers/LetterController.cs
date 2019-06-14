﻿using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Future.Service;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Future.TodayApi.Controllers
{
    /// <summary>
    /// 漂流瓶控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LetterController : BaseController
    {
        private readonly string MODULE = "LetterController";
        private readonly LetterService api = SingletonProvider<LetterService>.Instance;

        /// <summary>
        /// 瓶子互动列表
        /// </summary>
        [HttpPost]
        public JsonResult DiscussList()
        {
            RequestContext<DiscussListRequest> request = null;
            ResponseContext<DiscussListResponse> response = null;
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                request = json.JsonToObject<RequestContext<DiscussListRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Content == null|| request.Content.UId<=0|| request.Content.PageIndex<=0)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                response = api.DiscussList(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DiscussList", ex);
            }
            finally
            {
                WriteServiceLog(MODULE, "DiscussList", request?.Head, response == null ? ErrCodeEnum.Failure : response.Code, response?.ResultMessage, request, response);
            }
        }

        /// <summary>
        /// 瓶子互动详情列表
        /// </summary>
        [HttpPost]
        public JsonResult DiscussDetailList()
        {
            RequestContext<DiscussDetailListRequest> request = null;
            ResponseContext<DiscussDetailListResponse> response = null;
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                request = json.JsonToObject<RequestContext<DiscussDetailListRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Content == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                response = api.DiscussDetailList(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DiscussDetailList", ex);
            }
            finally
            {
                WriteServiceLog(MODULE, "DiscussDetailList", request?.Head, response == null ? ErrCodeEnum.Failure : response.Code, response?.ResultMessage, request, response);
            }
        }

        /// <summary>
        /// 首页瓶子捡起列表
        /// </summary>
        [HttpPost]
        public JsonResult PickUpList()
        {
            RequestContext<PickUpListRequest> request = null;
            ResponseContext<PickUpListResponse> response = null;
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                request = json.JsonToObject<RequestContext<PickUpListRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Content == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                response = api.PickUpList(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "PickUpList", ex);
            }
            finally
            {
                WriteServiceLog(MODULE, "PickUpList", request?.Head, response == null ? ErrCodeEnum.Failure : response.Code, response?.ResultMessage, request, response);
            }
        }

        /// <summary>
        /// 发布动态
        /// </summary>
        [HttpPost]
        public JsonResult PublishMoment()
        {
            RequestContext<PublishMomentRequest> request = null;
            ResponseContext<PublishMomentResponse> response = null;
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                request = json.JsonToObject<RequestContext<PublishMomentRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Content == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                response = api.PublishMoment(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "PublishMoment", ex);
            }
            finally
            {
                WriteServiceLog(MODULE, "PublishMoment", request?.Head, response == null ? ErrCodeEnum.Failure : response.Code, response?.ResultMessage, request, response);
            }
        }

        /// <summary>
        /// 上传动态图片
        /// </summary>
        [HttpPost]
        public JsonResult UpLoadImg()
        {
            var response = new ResponseContext<UpLoadImgResponse>();
            try
            {
                var uploadfile = Request.Form.Files[0];

                var filePath = JsonSettingHelper.AppSettings["SetImgPath"];

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                if (uploadfile == null)
                {
                    response = new ResponseContext<UpLoadImgResponse>(false, ErrCodeEnum.Failure, null, "上传文件失败");
                    return new JsonResult(response);
                }

                //文件后缀
                var fileExtension = Path.GetExtension(uploadfile.FileName);

                //判断后缀是否是图片
                const string fileFilt = "|.gif|.jpg|.php|.jsp|.jpeg|.png|";
                if (fileExtension == null)
                {
                    response = new ResponseContext<UpLoadImgResponse>(false, ErrCodeEnum.Failure, null, "上传的文件没有后缀");
                    return new JsonResult(response);
                }
                if (!fileFilt.Contains(fileExtension))
                {
                    response = new ResponseContext<UpLoadImgResponse>(false, ErrCodeEnum.Failure, null, "上传的文件不是图片");
                    return new JsonResult(response);
                }

                //判断文件大小    
                long length = uploadfile.Length;
                if (length > 1024 * 1024 * 10) //1M
                {
                    response = new ResponseContext<UpLoadImgResponse>(false, ErrCodeEnum.Failure, null, "上传的文件不能大于10M");
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

                response.Content = new UpLoadImgResponse()
                {
                    ImgPath = saveName,
                    ImgLength = length,
                    ImgMime = fileExtension

                };
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UpLoadImg", ex);
            }
        }

        /// <summary>
        /// 删除已上传的图片
        /// </summary>
        [HttpPost]
        public JsonResult DeleteImg()
        {
            RequestContext<DeleteImgRequest> request = null;
            ResponseContext<DeleteImgResponse> response = null;
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                request = json.JsonToObject<RequestContext<DeleteImgRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Content == null || request.Content.ImgPath.IsNullOrEmpty())
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }

                string path = JsonSettingHelper.AppSettings["SetImgPath"] + request.Content.ImgPath;
                System.IO.File.Delete(path);
                response.Content.IsExecuteSuccess = true;
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "DeleteImg", ex);
            }
            finally
            {
                WriteServiceLog(MODULE, "DeleteImg", request?.Head, response == null ? ErrCodeEnum.Failure : response.Code, response?.ResultMessage, request, response);
            }
        }

        /// <summary>
        /// 存入用户信息
        /// </summary>
        [HttpPost]
        public JsonResult SetUserInfo()
        {
            RequestContext<SetUserInfoRequest> request = null;
            ResponseContext<SetUserInfoResponse> response = null;
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                request = json.JsonToObject<RequestContext<SetUserInfoRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Content == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                response = api.SetUserInfo(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "SetUserInfo", ex);
            }
            finally
            {
                WriteServiceLog(MODULE, "SetUserInfo", request?.Head, response == null ? ErrCodeEnum.Failure : response.Code, response?.ResultMessage, request, response);
            }
        }
    }
}