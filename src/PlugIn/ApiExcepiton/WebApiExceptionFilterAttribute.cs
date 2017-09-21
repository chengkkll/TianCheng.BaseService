using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TianCheng.Model;

namespace TiangCheng.BaseService.PlugIn.ApiExcepiton
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="modelMetadataProvider"></param>
        public WebApiExceptionFilterAttribute(IHostingEnvironment hostingEnvironment, IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        /// <summary>
        /// 重写基类的异常处理方法
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ApiException ae)
            {
                context.HttpContext.Response.StatusCode = (int)ae.HttpStatus;
                context.Result = new JsonResult(new { code = ae.Code, message = ae.Message });
                base.OnException(context);
            }
            else if (context.Exception is System.TimeoutException te)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                context.Result = new JsonResult(new { code = 502, message = "无法链接数据库" });
                base.OnException(context);
            }
            else if (context.Exception is MongoDB.Driver.MongoConnectionException me)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                context.Result = new JsonResult(new { code = 502, message = "无法链接服务器" });
                base.OnException(context);
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(new { code = 500, message = context.Exception.Message });
                base.OnException(context);
            }
            //else if (actionExecutedContext.Exception is BadRequestParameterException)
            //{
            //    var mResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            //    string msg = actionExecutedContext.Exception.Message;
            //    mResponse.Content = new StringContent("{\"code\": 400 ,\"message\": \"" + msg + "\" }");
            //    mResponse.ReasonPhrase = "bad request";

            //    actionExecutedContext.Response = mResponse;
            //}
            //else if (actionExecutedContext.Exception is TianChengException)
            //{
            //    TianChengException te = actionExecutedContext.Exception as TianChengException;
            //    int code = te.Code;
            //    string msg = te.Message;
            //    var mResponse = new HttpResponseMessage(HttpStatusCode.Forbidden);
            //    mResponse.Content = new StringContent("{\"code\": " + code + " ,\"message\": \"" + msg + "\" }");
            //    mResponse.ReasonPhrase = "tc exception";

            //    actionExecutedContext.Response = mResponse;
            //}
            //else if (actionExecutedContext.Exception is RecordOperateException)
            //{
            //    var mResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            //    string msg = actionExecutedContext.Exception.Message;
            //    mResponse.Content = new StringContent("{ \"code\": 400 ,\"message\": \"" + msg + "\" }");
            //    mResponse.Content.Headers.ContentType.MediaType = "application/json;utf-8";
            //    mResponse.ReasonPhrase = "bad request";

            //    actionExecutedContext.Response = mResponse;
            //}
            //.....这里可以根据项目需要返回到客户端特定的状态码。如果找不到相应的异常，统一返回服务端错误500
            //else
            //{
            //    context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //}
            // TODO: Pass additional detailed data via ViewData
            //context.Result = result;


        }
    }

}
