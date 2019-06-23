using TianCheng.Model;

namespace TianCheng.BaseService.Services.IntModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GuidInfoService<T> : InfoBusinessService<T, string>
        where T : BusinessGuidModel, new()
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public GuidInfoService(DAL.IDBOperation<T, string> dal) : base(dal)
        {

        }
    }
}
