using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TianCheng.Model;
using Microsoft.AspNetCore.Http;
using TianCheng.DAL;
using Microsoft.Extensions.Logging;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public class ViewBusinessService<T, V, Q, IdType> : InfoBusinessQueryService<T, Q, IdType>, IBusinessService<T, IdType>
        where T : IBusinessModel<IdType>, new()
        where Q : QueryInfo
    {
        #region 构造方法        
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public ViewBusinessService(IDBOperation<T, IdType> dal) : base(dal)
        {

        }
        #endregion

        #region 新增 / 修改方法
        /// <summary>
        /// 创建一个对象信息
        /// </summary>
        /// <param name="view">更新对象</param>
        /// <param name="logonInfo">登录人信息</param>
        /// <returns></returns>
        public virtual ResultView Create(V view, TokenLogonInfo logonInfo)
        {
            //将提交的数据转成实体对象
            var info = AutoMapper.Mapper.Map<T>(view);
            return Create(info, logonInfo);
        }

        /// <summary>
        /// 更新一个对象信息
        /// </summary>
        /// <param name="view">更新对象</param>
        /// <param name="logonInfo">登录人信息</param>
        /// <returns></returns>
        public virtual ResultView Update(V view, TokenLogonInfo logonInfo)
        {
            //将提交的数据转成实体对象
            var info = AutoMapper.Mapper.Map<T>(view);
            return Update(info, logonInfo);
        }
        #endregion

        #region SearchById
        #region SearchById事件处理
        /// <summary>
        /// 转换View后的事件处理
        /// </summary>
        static public Action<V> OnSearchViewById;
        #endregion

        /// <summary>
        /// 根据id查询对象信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual V SearchById(string id)
        {
            _logger.LogTrace($"根据ID查询对象（SearchById），对象类型为：[{typeof(V).FullName}]");
            //根据ID获取对象信息
            var info = _SearchById(id);
            // 如果查询不到数据，抛出404异常
            if (info == null)
            {
                _logger.LogWarning($"根据ID查询对象，无法找到数据。类型为：[{typeof(V).FullName}]\r\nid值：[{id}]");
                ApiException.ThrowEmptyData("无法找到您要的数据");
            }

            //返回
            V view = AutoMapper.Mapper.Map<V>(info);

            // 转换View后的事件处理
            OnSearchViewById?.Invoke(view);
            _logger.LogTrace($"根据ID查询对象，已获取对象。类型为：[{typeof(V).FullName}]\r\n对象值：[{view.ToJson()}]");
            return view;
        }
        #endregion

        #region 无分页View数据查询
        /// <summary>
        /// 无分页查询 获取默认的View对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<V> Filter(Q input)
        {
            _logger.LogTrace($"按条件无分页查询（List<V> Filter(Q input)）。查询结果对象类型为：[{typeof(V).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{input.ToJson()}]");
            return Filter<V>(input);
        }
        /// <summary>
        /// 无分页查询 获取指定的View对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<OV> Filter<OV>(Q input)
        {
            _logger.LogTrace($"按条件无分页查询-获取指定的View对象 （List<OV> Filter<OV>(Q input)）。查询结果对象类型为：[{typeof(OV).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{input.ToJson()}]");
            var list = _Filter(input).ToList();
            var viewList = AutoMapper.Mapper.Map<List<OV>>(list);
            return viewList ?? new List<OV>();
        }
        #endregion

        #region 分步查询
        /// <summary>
        /// 分步查询数据，返回默认的View对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<V> FilterStep(Q input)
        {
            return FilterStep<V>(input);
        }
        /// <summary>
        /// 分步查询数据，返回指定的View对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<VO> FilterStep<VO>(Q input)
        {
            var infoList = _FilterStep(input);
            return AutoMapper.Mapper.Map<List<VO>>(infoList);
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询 返回默认的View对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagedResult<V> FilterPage(Q input)
        {
            return FilterPage<V>(input);
        }
        /// <summary>
        /// 分页查询 转换成指定的View对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagedResult<OV> FilterPage<OV>(Q input)
        {
            var queryResult = _Filter(input);
            PagedResultPagination pagination = new PagedResultPagination();
            queryResult = SetFilterPagination(input, queryResult, pagination);
            var infoList = queryResult.ToList();
            //if (infoList.Count == 0)
            //{
            //    _Context.Response.StatusCode = 204;
            //}
            var viewList = AutoMapper.Mapper.Map<List<OV>>(infoList);
            return new PagedResult<OV>(viewList, pagination);
        }
        #endregion
    }
}
