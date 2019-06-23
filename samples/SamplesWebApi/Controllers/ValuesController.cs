using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Annotations;
using TianCheng.BaseService;

namespace SamplesWebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        ///  获取IP地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("User/Ip")]
        public string Get([FromServices]IHttpContextAccessor accessor)
        {
            return accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        /// <summary>
        ///  获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return id.ToString();
        }

        /// <summary>
        ///  新增对象
        /// </summary>
        /// <param name="value"></param>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.Values.Create")]
        [SwaggerOperation(Tags = new[] { "系统管理-测试接口" })]
        [Route("{id}")]
        [HttpPost]
        public void Create([FromBody]string value)
        {
        }

        /// <summary>
        ///  修改对象
        /// </summary>
        /// <param name="value"></param>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.Values.Update")]
        [SwaggerOperation(Tags = new[] { "系统管理-测试接口" })]
        [Route("{id}")]
        [HttpPut]
        public void Update([FromBody]string value)
        {
        }


        /// <summary>
        ///  上传文件测试
        /// </summary>
        /// <param name="file"></param>
        [SwaggerOperation(Tags = new[] { "系统管理-测试接口" })]
        [HttpPost("testUpfile")]
        public UploadFileInfo Upfile(IFormFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            string webFile = $"~/wwwroot/UploadFiles/{Guid.NewGuid().ToString()}";
            return UploadFileHandle.UploadImage(webFile, file);
        }

        /// <summary>
        ///  逻辑删除
        /// </summary>
        /// <param name="id"></param>
        [SwaggerOperation(Tags = new[] { "系统管理-测试接口" })]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
        }
        /// <summary>
        ///  物理删除
        /// </summary>
        /// <param name="id"></param>
        [SwaggerOperation(Tags = new[] { "系统管理-测试接口" })]
        [HttpDelete("Remove/{id}")]
        public void Remove(int id)
        {
            TianCheng.Model.ApiException.ThrowBadRequest("不允许删除操作");
        }
    }
}
