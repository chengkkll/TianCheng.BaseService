using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.DAL.MongoDB;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public class MongoBusinessService<T, Q> : InfoBusinessQueryService<T, Q, MongoDB.Bson.ObjectId>
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
