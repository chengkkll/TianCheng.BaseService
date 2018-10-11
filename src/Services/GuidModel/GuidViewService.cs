using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Model;

namespace TianCheng.BaseService.Services.IntModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public class GuidViewService<T, V, Q> : ViewBusinessService<T, V, Q, string>
        where T : BusinessGuidModel, new()
        where Q : QueryInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public GuidViewService(DAL.IDBOperation<T, string> dal) : base(dal)
        {

        }
    }
}
