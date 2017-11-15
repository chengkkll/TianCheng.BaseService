using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace TianCheng.BaseService.PlugIn
{
    /// <summary>
    /// 请求api接口时的权限控制
    /// </summary>
    public interface IAuthService 
    {
        /// <summary>
        /// 每次请求接口时，加载用户的权限信息
        /// </summary>
        /// <param name="identity"></param>
        void FillFunctionPolicy(ClaimsIdentity identity);

        /// <summary>
        /// 登录接口，根据账号和密码获取Token
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string Login(string account, string password);
        /// <summary>
        /// 退出处理
        /// </summary>
        /// <param name="logonInfo"></param>
        void Logout(TokenLogonInfo logonInfo);

    }
}
