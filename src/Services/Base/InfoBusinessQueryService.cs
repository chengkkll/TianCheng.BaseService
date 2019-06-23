using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TianCheng.DAL;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public class InfoBusinessQueryService<T, Q, IdType> : InfoBusinessService<T, IdType>, IBusinessService<T, IdType>
         where T : IBusinessModel<IdType>, new()
         where Q : QueryInfo
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public InfoBusinessQueryService(IDBOperation<T, IdType> dal) : base(dal)
        {

        }
        #endregion

        #region 所有查询功能        

        #region 基础数据过滤  Filter
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual IQueryable<T> _Filter(Q input)
        {
            _logger.LogTrace($"按条件查询（IQueryable<T> _Filter(Q input)）。查询结果对象类型为：[{typeof(T).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{input.ToJson()}]");

            var query = _Dal.Queryable();

            #region 查询条件
            //不显示删除的数据
            query = query.Where(e => e.IsDelete == false);
            #endregion

            #region 设置排序规则
            //设置排序方式
            switch (input.Sort.Property)
            {
                case "date": { query = input.Sort.IsAsc ? query.OrderBy(e => e.UpdateDate) : query.OrderByDescending(e => e.UpdateDate); break; }
                default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
            }
            #endregion

            //返回查询结果
            return query;
        }
        #endregion

        #region 根据分页/分步信息过滤数据
        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="filter">查询条件（包括分页及排序的信息）</param>
        /// <param name="queryResult">所有满足过滤条件的数据</param>
        /// <param name="resultPagination">返回的分页信息</param>
        protected virtual IQueryable<T> SetFilterPagination(Q filter, IQueryable<T> queryResult,
            PagedResultPagination resultPagination)
        {
            _logger.LogTrace($"设置数据分页效果 （IQueryable<T> SetFilterPagination(Q filter, IQueryable<T> queryResult, PagedResultPagination resultPagination)）。查询结果对象类型为：[{typeof(T).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{filter.ToJson()}]\r\n分页数据为：[{resultPagination.ToJson()}]");

            #region 设置分页信息及返回值
            // 初始化查询条件
            if (filter.Pagination == null) filter.Pagination = QueryPagination.DefaultObject;
            if (filter.Pagination.PageMaxRecords == 0) { filter.Pagination.PageMaxRecords = QueryPagination.DefaultPageMaxRecords; }

            // 设置返回的分页信息
            resultPagination.PageMaxRecords = filter.Pagination.PageMaxRecords;
            resultPagination.TotalRecords = queryResult.Count();
            resultPagination.TotalPage = (int)Math.Ceiling((double)resultPagination.TotalRecords / (double)resultPagination.PageMaxRecords);

            // 设置页号
            resultPagination.Index = filter.Pagination.Index;
            if (resultPagination.Index > resultPagination.TotalPage) { resultPagination.Index = resultPagination.TotalPage; }
            if (resultPagination.Index == 0) { resultPagination.Index = 1; }

            // 返回查询的数据结果
            return queryResult.Skip((resultPagination.Index - 1) * resultPagination.PageMaxRecords).Take(resultPagination.PageMaxRecords);
            #endregion
        }
        #endregion

        #region 分步查询
        /// <summary>
        /// 分步查询数据，返回实体对象信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<T> _FilterStep(Q input)
        {
            _logger.LogTrace($"设置分步查询数据 （List<T> _FilterStep(Q input)）。查询结果对象类型为：[{typeof(T).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{input.ToJson()}]");
            var queryResult = _Filter(input);
            if (input.Pagination == null) input.Pagination = new QueryPagination() { Index = 1, PageMaxRecords = 10 };
            if (input.Pagination.Index <= 0) input.Pagination.Index = 1;
            if (input.Pagination.PageMaxRecords <= 0) input.Pagination.PageMaxRecords = 10;

            PagedResultPagination resultPage = new PagedResultPagination();
            queryResult = SetFilterPagination(input, queryResult, resultPage);
            if (resultPage.TotalPage < input.Pagination.Index)  // 如果查询到最后一页，返回空列表
            {

                return new List<T>();
            }

            return queryResult.ToList();
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询 获取实体对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagedResult<T> _FilterPage(Q input)
        {
            var queryResult = _Filter(input);
            PagedResultPagination pagination = new PagedResultPagination();
            queryResult = SetFilterPagination(input, queryResult, pagination);
            var infoList = queryResult.ToList();
            //if (infoList.Count == 0)
            //{
            //    _Context.Response.StatusCode = 204;
            //}
            return new PagedResult<T>(infoList, pagination);
        }
        #endregion

        #region 下拉列表数据查询
        /// <summary>
        /// 获取一个查询列表对象形式的集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<SelectView> Select(Q input)
        {
            var list = _Filter(input);
            return AutoMapper.Mapper.Map<List<SelectView>>(list);
        }

        /// <summary>
        /// 获取满足条件的前几条数据    input.Pagination.PageMaxRecords为指定的返回记录的最大条数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<SelectView> SelectTop(Q input)
        {
            var queryResult = _Filter(input);
            PagedResultPagination pagination = new PagedResultPagination();
            input.Pagination.Index = 1;
            if (input.Pagination.PageMaxRecords <= 0) input.Pagination.PageMaxRecords = 10;
            queryResult = SetFilterPagination(input, queryResult, pagination);

            var infoList = queryResult.ToList();

            return AutoMapper.Mapper.Map<List<SelectView>>(infoList);
        }
        #endregion

        #region Count

        /// <summary>
        /// 根据条件获取记录数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Count(Q input)
        {
            _logger.LogTrace($"查询记录总条数 （int Count()）。查询对象类型为：[{typeof(T).FullName}]\r\n查询条件对象[{typeof(Q).FullName}]\r\n查询条件：[{input.ToJson()}]");
            var queryResult = _Filter(input);
            return queryResult.Count();
        }
        #endregion

        #endregion
    }

}
