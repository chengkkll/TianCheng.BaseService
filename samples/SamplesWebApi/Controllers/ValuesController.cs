using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SamplesWebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        protected HttpContext _Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ValuesController(ILogger<ValuesController> logger, IServiceProvider servicesProvider)
        {
            _logger = logger;


            var provider = servicesProvider;

            object factory = servicesProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
            _Context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;
        }
        /// <summary>
        ///  GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _Context.Response.StatusCode = 204;
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        ///  GET api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        ///  POST api/values
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        ///  PUT api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        ///  DELETE api/values/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
