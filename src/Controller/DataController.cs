using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 基础数据控制器基类
    /// </summary>
    public class DataController : Controller
    {
        #region 登录信息
        private TokenLogonInfo _LogonInfo = null;
        /// <summary>
        /// 获取登录信息
        /// </summary>
        protected TokenLogonInfo LogonInfo
        {
            get
            {
                if (_LogonInfo != null)
                {
                    return _LogonInfo;
                }
                if (User == null || User.Claims == null)
                {
                    return new TokenLogonInfo();
                }
                _LogonInfo = new TokenLogonInfo();
                foreach (var item in User.Claims)
                {
                    switch (item.Type)
                    {
                        case "name":
                            {
                                _LogonInfo.Name = item.Value;
                                break;
                            }
                        case "id":
                            {
                                _LogonInfo.Id = item.Value;
                                break;
                            }
                        case "roleId":
                            {
                                _LogonInfo.RoleId = item.Value;
                                break;
                            }
                        case "depId":
                            {
                                _LogonInfo.DepartmentId = item.Value;
                                break;
                            }
                        case "depName":
                            {
                                _LogonInfo.DepartmentName = item.Value;
                                break;
                            }
                        case "function_policy":
                            {
                                _LogonInfo.FunctionPolicyList.Add(item.Value);
                                break;
                            }
                    }
                }
                return _LogonInfo;
            }
        }
        #endregion

        #region 请求地址
        private IHttpContextAccessor _ContextAccessor = null;
        /// <summary>
        /// 获取请求链接的服务
        /// </summary>
        protected IHttpContextAccessor ContextAccessor
        {
            get
            {
                if (_ContextAccessor == null)
                {
                    _ContextAccessor = ServiceLoader.GetService<IHttpContextAccessor>();
                }
                return _ContextAccessor;
            }
        }
        /// <summary>
        /// 获取请求的IP地址
        /// </summary>
        protected string IpAddress
        {
            get
            {
                if (ContextAccessor != null)
                {
                    return ContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                return string.Empty;
            }
        }
        #endregion
    }
}
