using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianCheng.BaseService;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 数据增删改查的通用接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInfoService<T> : IServiceRegister
    {
        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T _SearchById(string id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        ResultView Create(T info, TokenLogonInfo logonInfo);
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        ResultView Update(T info, TokenLogonInfo logonInfo);
    }
}
