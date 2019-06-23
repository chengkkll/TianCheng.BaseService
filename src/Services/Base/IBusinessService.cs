using System.Linq;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 数据增删改查的通用接口
    /// </summary>
    /// <typeparam name="T">操作的对象类型</typeparam>
    /// <typeparam name="IdType">对象的ID类型</typeparam>
    public interface IBusinessService<T, IdType> : IServiceRegister
        where T : IBusinessModel<IdType>
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

        /// <summary>
        /// 无条件查询所有
        /// </summary>
        /// <returns></returns>
        IQueryable<T> SearchQueryable();
    }
}
