using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.BaseService.PlugIn.LoadApi
{
    /// <summary>
    /// 请求方式
    /// </summary>
    public enum ApiMethodType
    {
        /// <summary>
        /// GET
        /// </summary>
        GET = 1,
        /// <summary>
        /// POST
        /// </summary>
        POST = 2,
        /// <summary>
        /// PATCH
        /// </summary>
        PATCH = 4,
        /// <summary>
        /// PUT
        /// </summary>
        PUT = 8,
        /// <summary>
        /// DELETE
        /// </summary>
        DELETE = 16,
        /// <summary>
        /// OPTIONS
        /// </summary>
        OPTIONS = 32
    }
}
