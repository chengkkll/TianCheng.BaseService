using System.Threading;
using System.Threading.Tasks;
using TianCheng.BaseService.Services;
using TianCheng.DAL;
using TianCheng.DAL.MongoDB;
using TianCheng.DAL.Redis;
using TianCheng.Model;


namespace TianCheng.BaseService
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public class MongoBusinessService<T, V, Q> : ViewBusinessService<T, V, Q, MongoDB.Bson.ObjectId>
        where T : BusinessMongoModel, new()
        where Q : QueryInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public MongoBusinessService(MongoOperation<T> dal) : base(dal)
        {

        }
    }
}
