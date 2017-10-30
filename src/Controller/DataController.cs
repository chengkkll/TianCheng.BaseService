using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 基础数据控制器基类
    /// </summary>
    public class DataController : Controller
    {
        private TokenLogonInfo _LogonInfo = null;
        /// <summary>
        /// 获取登录信息
        /// </summary>
        protected TokenLogonInfo LogonInfo
        {
            get
            {
                if(_LogonInfo != null)
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
                    switch(item.Type)
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
    }

}
