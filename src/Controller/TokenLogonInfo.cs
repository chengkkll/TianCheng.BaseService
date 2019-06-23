using System.Collections.Generic;

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
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 当前用户拥有的功能点列表
        /// </summary>
        public List<string> FunctionPolicyList { get; set; } = new List<string>();

        /// <summary>
        /// 判断当前登录用户是否拥有指定的权限
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public bool HasPolicy(string policy)
        {
            return FunctionPolicyList.Contains(policy);
        }
    }
}
