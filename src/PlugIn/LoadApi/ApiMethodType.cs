namespace TianCheng.BaseService
{
    /// <summary>
    /// 请求方式
    /// </summary>
    internal enum ApiMethodType
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
