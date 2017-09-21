using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class TokenLogonInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentId { get; set; }


    }
}
