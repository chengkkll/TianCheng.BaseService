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
    public class MongoBusinessService<T> : InfoBusinessService<T, MongoDB.Bson.ObjectId>
        where T : BusinessMongoModel, new()
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
