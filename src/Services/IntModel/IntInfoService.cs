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
    public class IntBusinessService<T> : InfoBusinessService<T, int>
        where T : BusinessIntModel, new()
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
