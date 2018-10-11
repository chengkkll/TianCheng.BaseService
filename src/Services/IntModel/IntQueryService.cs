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
    public class IntBusinessService<T, Q> : InfoBusinessQueryService<T, Q, int>
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
