using TianCheng.Model;

namespace TianCheng.BaseService.Services.IntModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public class IntBusinessService<T, V, Q> : ViewBusinessService<T, V, Q, int>
        where T : BusinessIntModel, new()
        where Q : QueryInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public IntBusinessService(DAL.IDBOperation<T, int> dal) : base(dal)
        {

        }
    }
}
