using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public class GuidQueryService<T, Q> : InfoBusinessQueryService<T, Q, string>
        where T : BusinessGuidModel, new()
        where Q : QueryInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public GuidQueryService(DAL.IDBOperation<T, string> dal) : base(dal)
        {

        }
    }
}
