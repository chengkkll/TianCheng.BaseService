using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TianCheng.BaseService.Services;
using TianCheng.DAL;
using TianCheng.DAL.MongoDB;
using TianCheng.DAL.Redis;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    ///// <summary>
    ///// 通用基本服务
    ///// </summary>
    //public class BusinessService<T, V, Q, IdType> : BusinessServiceAction<T>, IBusinessService<T, IdType>
    //    where T : IBusinessModel<IdType>, new()
    //    where Q : QueryInfo

    //{
    //    #region 构造方法        
    //    /// <summary>
    //    /// 数据操作对象
    //    /// </summary>
    //    protected IDBOperation<T, IdType> _Dal;
    //    /// <summary>
    //    /// 具体业务日志工具
    //    /// </summary>
    //    protected readonly ILogger<BusinessService<T, V, Q, IdType>> _logger;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    protected readonly IServiceProvider _ServicesProvider;

    //    private HttpContext _context = null;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    protected HttpContext _Context
    //    {
    //        get
    //        {
    //            if (_context == null)
    //            {
    //                object factory = _ServicesProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
    //                _context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;
    //            }
    //            return _context;
    //        }
    //    }

    //    /// <summary>
    //    /// 构造方法
    //    /// </summary>
    //    /// <param name="dal"></param>
    //    public BusinessService(IDBOperation<T, IdType> dal)
    //    {
    //        _Dal = dal;
    //        _logger = ServiceLoader.GetService<ILogger<BusinessService<T, V, Q, IdType>>>();
    //        _ServicesProvider = ServiceLoader.Instance;
    //    }

    //    /// <summary>
    //    /// 构造方法
    //    /// </summary>
    //    /// <param name="dal"></param>
    //    /// <param name="logger"></param>
    //    /// <param name="servicesProvider"></param>
    //    public BusinessService(IDBOperation<T, IdType> dal, ILogger<BusinessService<T, V, Q, IdType>> logger, IServiceProvider servicesProvider)
    //    {
    //        _Dal = dal;
    //        _logger = logger;
    //        _ServicesProvider = servicesProvider;
    //    }
    //    #endregion

    //    //#region 缓存操作
    //    ///// <summary>
    //    ///// 是否启用缓存
    //    ///// </summary>
    //    //protected virtual bool EnableCache { get; } = false;

    //    ///// <summary>
    //    ///// 是否可用Redis缓存
    //    ///// </summary>
    //    //protected bool HasRedisCache
    //    //{
    //    //    get { return EnableCache && RedisConnection.HasRedis; }
    //    //}
    //    ///// <summary>
    //    ///// 操作Redis的服务
    //    ///// </summary>
    //    //RedisHashOperation<T> RedisCache = new RedisHashOperation<T>();

    //    ///// <summary>
    //    ///// 重置本对象的Redis缓存
    //    ///// </summary>
    //    //public void ResetRedisCache()
    //    //{
    //    //    // 异步完善缓存信息
    //    //    ThreadPool.QueueUserWorkItem(h =>
    //    //    {
    //    //        RedisCache.Init(_Dal.Queryable().ToList(), e => e.Id.ToString());
    //    //    });
    //    //}
    //    //#endregion

    //    #region 获取一个操作实例
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    static private BusinessService<T, V, Q, IdType> _Service = null;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    static public BusinessService<T, V, Q, IdType> Service
    //    {
    //        get { return _Service; }
    //    }
    //    /// <summary>
    //    /// 构造方法
    //    /// </summary>
    //    public BusinessService()
    //    {
    //        if (_Service == null)
    //        {
    //            _Service = this;
    //        }
    //    }
    //    #endregion

    //    #region 所有查询功能

    //    #region SearchById
    //    #region SearchById事件处理
    //    /// <summary>
    //    /// 转换View后的事件处理
    //    /// </summary>
    //    static public Action<V> OnSearchViewById;
    //    #endregion

    //    /// <summary>
    //    /// 根据ID列表获取对象集合
    //    /// </summary>
    //    /// <param name="ids"></param>
    //    /// <returns></returns>
    //    public List<T> _SearchByIds(IEnumerable<string> ids)
    //    {
    //        return _Dal.SearchByIds(ids);
    //    }

    //    /// <summary>
    //    /// 根据id查询对象信息  如果无查询结果会抛出异常
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    public virtual T _SearchById(string id)
    //    {
    //        T info = new T();
    //        // 判断是否通过缓存读取数据
    //        //if (HasRedisCache)
    //        //{
    //        //    info = RedisCache.GetValue(id);
    //        //    if (info != null)
    //        //    {
    //        //        return info;
    //        //    }
    //        //}
    //        // 将传入ID转成对象处理
    //        T t = new T();
    //        t.SetId(id);
    //        // 从数据库中获取对象
    //        info = _SearchById(t.Id);
    //        // 将获取到的对象放入Redis中
    //        //if (info != null && EnableCache && RedisConnection.HasRedis)
    //        //{
    //        //    RedisCache.SetValue(id, info);
    //        //}
    //        // 返回查询结果对象
    //        return info;
    //    }

    //    /// <summary>
    //    /// 根据id查询对象信息  如果无查询结果会抛出异常
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    public T _SearchById(IdType id)
    //    {
    //        _logger.LogTrace($"根据ID查询对象（_SearchById），对象类型为：[{typeof(T).FullName}]");
    //        // 检查ID是否有效
    //        if (!new T().CheckId(id))
    //        {
    //            _logger.LogWarning($"按ID查询对象时，ID无效。类型为：[{typeof(T).FullName}][id={id}]");
    //            throw ApiException.BadRequest("请求的id无效。");
    //        }
    //        // 根据ID获取信息
    //        var info = _Dal.SearchByTypeId(id);
    //        _logger.LogTrace($"根据ID查询对象，已获取对象。类型为：[{typeof(T).FullName}]\r\n对象值：[{info.ToJson()}]");
    //        // 查询到Info对象后的事件处理
    //        OnSearchInfoById?.Invoke(info);
    //        return info;
    //    }

    //    /// <summary>
    //    /// 根据id查询对象信息
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    public virtual V SearchById(string id)
    //    {
    //        _logger.LogTrace($"根据ID查询对象（SearchById），对象类型为：[{typeof(V).FullName}]");
    //        //根据ID获取对象信息
    //        var info = _SearchById(id);
    //        // 如果查询不到数据，抛出404异常
    //        if (info == null)
    //        {
    //            _logger.LogWarning($"根据ID查询对象，无法找到数据。类型为：[{typeof(V).FullName}]\r\nid值：[{id}]");
    //            ApiException.ThrowEmptyData("无法找到您要的数据");
    //        }

    //        //返回
    //        V view = AutoMapper.Mapper.Map<V>(info);

    //        // 转换View后的事件处理
    //        OnSearchViewById?.Invoke(view);
    //        _logger.LogTrace($"根据ID查询对象，已获取对象。类型为：[{typeof(V).FullName}]\r\n对象值：[{view.ToJson()}]");
    //        return view;
    //    }
    //    #endregion

    //    #region 无条件查询所有
    //    ///// <summary>
    //    ///// 获取所有缓存中的数据
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //public IQueryable<T> RedisCacheQuery()
    //    //{
    //    //    // 如果缓存的数据与数据库的数量不符
    //    //    if (_Dal.Queryable().Count() != RedisCache.Count())
    //    //    {
    //    //        // 异步完善缓存信息
    //    //        ResetRedisCache();
    //    //        // 先返回数据库链式查询条件
    //    //        return _Dal.Queryable();
    //    //    }

    //    //    return RedisCache.GetAllData().AsQueryable();
    //    //}
    //    /// <summary>
    //    /// 无条件查询所有
    //    /// </summary>
    //    /// <returns></returns>
    //    public IQueryable<T> SearchQueryable()
    //    {
    //        return _Dal.Queryable();
    //    }
    //    #endregion

    //    #region 基础数据过滤  Filter
    //    /// <summary>
    //    /// 按条件查询数据
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public virtual IQueryable<T> _Filter(Q input)
    //    {
    //        _logger.LogTrace($"按条件查询（IQueryable<T> _Filter(Q input)）。查询结果对象类型为：[{typeof(T).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{input.ToJson()}]");

    //        var query = _Dal.Queryable();

    //        #region 查询条件
    //        //不显示删除的数据
    //        query = query.Where(e => e.IsDelete == false);
    //        #endregion

    //        #region 设置排序规则
    //        //设置排序方式
    //        switch (input.Sort.Property)
    //        {
    //            case "date": { query = input.Sort.IsAsc ? query.OrderBy(e => e.UpdateDate) : query.OrderByDescending(e => e.UpdateDate); break; }
    //            default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
    //        }
    //        #endregion

    //        //返回查询结果
    //        return query;
    //    }
    //    #endregion

    //    #region 无分页数据查询
    //    /// <summary>
    //    /// 无分页查询 获取默认的View对象
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public List<V> Filter(Q input)
    //    {
    //        _logger.LogTrace($"按条件无分页查询（List<V> Filter(Q input)）。查询结果对象类型为：[{typeof(V).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{input.ToJson()}]");
    //        return Filter<V>(input);
    //    }
    //    /// <summary>
    //    /// 无分页查询 获取指定的View对象
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public List<OV> Filter<OV>(Q input)
    //    {
    //        _logger.LogTrace($"按条件无分页查询-获取指定的View对象 （List<OV> Filter<OV>(Q input)）。查询结果对象类型为：[{typeof(OV).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{input.ToJson()}]");
    //        var list = _Filter(input).ToList();
    //        var viewList = AutoMapper.Mapper.Map<List<OV>>(list);
    //        return viewList ?? new List<OV>();
    //    }
    //    #endregion

    //    #region 根据分页/分步信息过滤数据
    //    /// <summary>
    //    /// 设置分页信息
    //    /// </summary>
    //    /// <param name="filter">查询条件（包括分页及排序的信息）</param>
    //    /// <param name="queryResult">所有满足过滤条件的数据</param>
    //    /// <param name="resultPagination">返回的分页信息</param>
    //    protected virtual IQueryable<T> SetFilterPagination(Q filter, IQueryable<T> queryResult, PagedResultPagination resultPagination)
    //    {
    //        _logger.LogTrace($"设置数据分页效果 （IQueryable<T> SetFilterPagination(Q filter, IQueryable<T> queryResult, PagedResultPagination resultPagination)）。查询结果对象类型为：[{typeof(T).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{filter.ToJson()}]\r\n分页数据为：[{resultPagination.ToJson()}]");
    //        #region 设置分页信息及返回值
    //        //初始化查询条件
    //        if (filter.Pagination == null) filter.Pagination = QueryPagination.DefaultObject;
    //        if (filter.Pagination.PageMaxRecords == 0) { filter.Pagination.PageMaxRecords = QueryPagination.DefaultPageMaxRecords; }

    //        //设置返回的分页信息
    //        resultPagination.PageMaxRecords = filter.Pagination.PageMaxRecords;
    //        resultPagination.TotalRecords = queryResult.Count();
    //        resultPagination.TotalPage = (int)Math.Ceiling((double)resultPagination.TotalRecords / (double)resultPagination.PageMaxRecords);
    //        //设置页号
    //        resultPagination.Index = filter.Pagination.Index;
    //        if (resultPagination.Index > resultPagination.TotalPage) { resultPagination.Index = resultPagination.TotalPage; }
    //        if (resultPagination.Index == 0) { resultPagination.Index = 1; }
    //        //返回查询的数据结果
    //        return queryResult.Skip((resultPagination.Index - 1) * resultPagination.PageMaxRecords).Take(resultPagination.PageMaxRecords);

    //        #endregion
    //    }
    //    #endregion

    //    #region 分步查询
    //    /// <summary>
    //    /// 分步查询数据，返回实体对象信息
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public List<T> _FilterStep(Q input)
    //    {
    //        _logger.LogTrace($"设置分步查询数据 （List<T> _FilterStep(Q input)）。查询结果对象类型为：[{typeof(T).FullName}]\r\n查询条件对象类型为：[{typeof(Q).FullName}]\r\n查询条件为：[{input.ToJson()}]");
    //        var queryResult = _Filter(input);
    //        if (input.Pagination == null) input.Pagination = new QueryPagination() { Index = 1, PageMaxRecords = 10 };
    //        if (input.Pagination.Index <= 0) input.Pagination.Index = 1;
    //        if (input.Pagination.PageMaxRecords <= 0) input.Pagination.PageMaxRecords = 10;

    //        PagedResultPagination resultPage = new PagedResultPagination();
    //        queryResult = SetFilterPagination(input, queryResult, resultPage);
    //        if (resultPage.TotalPage < input.Pagination.Index)  // 如果查询到最后一页，返回空列表
    //        {

    //            return new List<T>();
    //        }

    //        return queryResult.ToList();
    //    }
    //    /// <summary>
    //    /// 分步查询数据，返回默认的View对象
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public List<V> FilterStep(Q input)
    //    {
    //        return FilterStep<V>(input);
    //    }
    //    /// <summary>
    //    /// 分步查询数据，返回指定的View对象
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public List<VO> FilterStep<VO>(Q input)
    //    {
    //        var infoList = _FilterStep(input);
    //        return AutoMapper.Mapper.Map<List<VO>>(infoList);
    //    }
    //    #endregion

    //    #region 分页查询
    //    /// <summary>
    //    /// 分页查询 返回默认的View对象
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public PagedResult<V> FilterPage(Q input)
    //    {
    //        return FilterPage<V>(input);
    //    }
    //    /// <summary>
    //    /// 分页查询 转换成指定的View对象
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public PagedResult<OV> FilterPage<OV>(Q input)
    //    {
    //        var queryResult = _Filter(input);
    //        PagedResultPagination pagination = new PagedResultPagination();
    //        queryResult = SetFilterPagination(input, queryResult, pagination);
    //        var infoList = queryResult.ToList();
    //        //if (infoList.Count == 0)
    //        //{
    //        //    _Context.Response.StatusCode = 204;
    //        //}
    //        var viewList = AutoMapper.Mapper.Map<List<OV>>(infoList);
    //        return new PagedResult<OV>(viewList, pagination);
    //    }
    //    /// <summary>
    //    /// 分页查询 获取实体对象
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public PagedResult<T> _FilterPage(Q input)
    //    {
    //        var queryResult = _Filter(input);
    //        PagedResultPagination pagination = new PagedResultPagination();
    //        queryResult = SetFilterPagination(input, queryResult, pagination);
    //        var infoList = queryResult.ToList();
    //        //if (infoList.Count == 0)
    //        //{
    //        //    _Context.Response.StatusCode = 204;
    //        //}
    //        return new PagedResult<T>(infoList, pagination);
    //    }
    //    #endregion

    //    #region 下拉列表数据查询
    //    /// <summary>
    //    /// 获取一个查询列表对象形式的集合
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public List<SelectView> Select(Q input)
    //    {
    //        var list = _Filter(input);
    //        return AutoMapper.Mapper.Map<List<SelectView>>(list);
    //    }

    //    /// <summary>
    //    /// 获取满足条件的前几条数据    input.Pagination.PageMaxRecords为指定的返回记录的最大条数
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public List<SelectView> SelectTop(Q input)
    //    {
    //        var queryResult = _Filter(input);
    //        PagedResultPagination pagination = new PagedResultPagination();
    //        input.Pagination.Index = 1;
    //        if (input.Pagination.PageMaxRecords <= 0) input.Pagination.PageMaxRecords = 10;
    //        queryResult = SetFilterPagination(input, queryResult, pagination);

    //        var infoList = queryResult.ToList();

    //        return AutoMapper.Mapper.Map<List<SelectView>>(infoList);
    //    }
    //    #endregion

    //    #region Count
    //    /// <summary>
    //    /// 获取所有的记录条数
    //    /// </summary>
    //    /// <returns></returns>
    //    public int Count()
    //    {
    //        _logger.LogTrace($"查询记录总条数 （int Count()）。查询对象类型为：[{typeof(T).FullName}]");
    //        return _Dal.Queryable().Count();
    //    }
    //    /// <summary>
    //    /// 根据条件获取记录数量
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns></returns>
    //    public int Count(Q input)
    //    {
    //        _logger.LogTrace($"查询记录总条数 （int Count()）。查询对象类型为：[{typeof(T).FullName}]\r\n查询条件对象[{typeof(Q).FullName}]\r\n查询条件：[{input.ToJson()}]");
    //        var queryResult = _Filter(input);
    //        return queryResult.Count();
    //    }
    //    #endregion

    //    #endregion

    //    #region 删除方法

    //    #region 派生类中可重写的方法
    //    /// <summary>
    //    /// 删除的通用验证处理
    //    /// </summary>
    //    /// <param name="info"></param>
    //    protected virtual void DeleteRemoveCheck(T info)
    //    {
    //        //如果验证有问题抛出异常
    //    }
    //    /// <summary>
    //    /// 逻辑删除的验证处理
    //    /// </summary>
    //    /// <param name="info"></param>
    //    protected virtual void DeleteCheck(T info)
    //    {
    //        //如果验证有问题抛出异常
    //    }
    //    /// <summary>
    //    /// 物理删除的验证处理
    //    /// </summary>
    //    /// <param name="info"></param>
    //    protected virtual void RemoveCheck(T info)
    //    {
    //        //如果验证有问题抛出异常
    //    }

    //    /// <summary>
    //    /// 删除的通用前置处理
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void DeleteRemoving(T info, TokenLogonInfo logonInfo)
    //    {
    //    }
    //    /// <summary>
    //    /// 逻辑删除的前置处理
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void Deleting(T info, TokenLogonInfo logonInfo)
    //    {
    //    }
    //    /// <summary>
    //    /// 取消逻辑删除的前置处理
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void UnDeleting(T info, TokenLogonInfo logonInfo)
    //    {
    //    }
    //    /// <summary>
    //    /// 物理删除的前置处理
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void Removing(T info, TokenLogonInfo logonInfo)
    //    {
    //    }

    //    /// <summary>
    //    /// 逻辑删除的后置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo">登录的用户信息</param>
    //    protected virtual void Deleted(T info, TokenLogonInfo logonInfo) { }
    //    /// <summary>
    //    /// 取消逻辑删除的后置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo">登录的用户信息</param>
    //    protected virtual void UnDeleted(T info, TokenLogonInfo logonInfo) { }
    //    /// <summary>
    //    /// 物理删除的后置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo">登录的用户信息</param>
    //    protected virtual void Removed(T info, TokenLogonInfo logonInfo) { }
    //    /// <summary>
    //    /// 删除的通用后置处理
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void DeleteRemoved(T info, TokenLogonInfo logonInfo)
    //    {
    //    }
    //    #endregion

    //    #region 逻辑删除
    //    /// <summary>
    //    /// 逻辑删除
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="logonInfo"></param>
    //    /// <returns></returns>
    //    public ResultView Delete(string id, TokenLogonInfo logonInfo)
    //    {
    //        return Delete(_SearchById(id), logonInfo);
    //    }
    //    /// <summary>
    //    /// 逻辑删除
    //    /// </summary>
    //    /// <param name="id">要删除的数据id</param>
    //    /// <param name="logonInfo">登录的用户信息</param>
    //    /// <returns></returns>
    //    public ResultView DeleteById(IdType id, TokenLogonInfo logonInfo)
    //    {
    //        // 为了做删除的前后置操作，所以先获取要删除的数据，再做删除处理
    //        return Delete(_SearchById(id), logonInfo);
    //    }
    //    /// <summary>
    //    /// 逻辑删除
    //    /// </summary>
    //    /// <param name="info">要删除的数据id</param>
    //    /// <param name="logonInfo">登录的用户信息</param>
    //    /// <returns></returns>
    //    public ResultView Delete(T info, TokenLogonInfo logonInfo)
    //    {
    //        if (info == null)
    //        {
    //            throw ApiException.EmptyData("无法找到要删除的数据");
    //        }
    //        string id = info.Id.ToString();
    //        _logger.LogTrace($"逻辑删除 ==> 类型为：[{typeof(T).FullName}]\t删除ID为：[{id}]");
    //        try
    //        {
    //            // 判断对应数据是否可以删除.
    //            DeleteRemoveCheck(info);
    //            OnDeleteRemoveCheck?.Invoke(info, logonInfo);
    //            DeleteCheck(info);
    //            OnDeleteCheck?.Invoke(info, logonInfo);
    //            // 逻辑删除的前置操作
    //            DeleteRemoving(info, logonInfo);
    //            Deleting(info, logonInfo);
    //            // 逻辑删除前置事件处理
    //            OnDeleting?.Invoke(info, logonInfo);
    //            // 设置逻辑删除状态
    //            info.UpdateDate = DateTime.Now;
    //            info.UpdaterId = logonInfo.Id;
    //            info.UpdaterName = logonInfo.Name;
    //            info.IsDelete = true;

    //            // 持久化数据
    //            _Dal.UpdateObject(info);
    //            // 逻辑删除的后置处理
    //            Deleted(info, logonInfo);
    //            DeleteRemoved(info, logonInfo);
    //            // 逻辑删除后置事件处理
    //            OnDeleted?.Invoke(info, logonInfo);
    //            // 返回保存结果
    //            return ResultView.Success(id);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, $"逻辑删除异常 ==> 类型为：[{typeof(T).FullName}]\t删除ID为：[{id}]\r\n删除对象：[{info.ToJson()}]");
    //            throw;
    //        }
    //    }
    //    #endregion

    //    #region 物理删除
    //    /// <summary>
    //    /// 物理删除
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="logonInfo"></param>
    //    /// <returns></returns>
    //    public ResultView Remove(string id, TokenLogonInfo logonInfo)
    //    {
    //        return Remove(_SearchById(id), logonInfo);
    //    }
    //    /// <summary>
    //    /// 物理删除
    //    /// </summary>
    //    /// <param name="id">要删除的数据id</param>
    //    /// <param name="logonInfo">登录的用户信息</param>
    //    /// <returns></returns>
    //    public ResultView RemoveById(IdType id, TokenLogonInfo logonInfo)
    //    {
    //        // 为了做删除的前后置操作，所以先获取要删除的数据，再做删除处理
    //        return Remove(_SearchById(id), logonInfo);
    //    }
    //    /// <summary>
    //    /// 物理删除
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    /// <returns></returns>
    //    public ResultView Remove(T info, TokenLogonInfo logonInfo)
    //    {
    //        if (info == null)
    //        {
    //            throw ApiException.EmptyData("无法找到要删除的数据");
    //        }
    //        string id = info.Id.ToString();
    //        _logger.LogTrace($"物理删除 ==> 类型为：[{typeof(T).FullName}]\t删除ID为：[{id}]");
    //        try
    //        {
    //            // 判断对应数据是否可以删除.
    //            DeleteRemoveCheck(info);
    //            OnDeleteRemoveCheck?.Invoke(info, logonInfo);
    //            RemoveCheck(info);
    //            OnRemoveCheck?.Invoke(info, logonInfo);
    //            // 物理删除的前置操作
    //            DeleteRemoving(info, logonInfo);
    //            Removing(info, logonInfo);
    //            // 物理删除时的前置事件处理
    //            OnRemoving?.Invoke(info, logonInfo);
    //            // 持久化数据
    //            _Dal.RemoveObject(info);
    //            // 物理删除的后置处理
    //            Removed(info, logonInfo);
    //            DeleteRemoved(info, logonInfo);
    //            // 物理删除时的后置事件处理
    //            OnRemoved?.Invoke(info, logonInfo);
    //            // 返回保存结果
    //            return ResultView.Success(info.Id.ToString());
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, $"物理删除异常 ==> 类型为：[{typeof(T).FullName}]\t删除ID为：[{id}]]\r\n删除对象为：[{info.ToJson()}]");
    //            throw;
    //        }
    //    }
    //    #endregion

    //    #region 还原删除
    //    /// <summary>
    //    /// 取消逻辑删除
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="logonInfo"></param>
    //    /// <returns></returns>
    //    public ResultView UnDelete(string id, TokenLogonInfo logonInfo)
    //    {
    //        return UnDelete(_SearchById(id), logonInfo);
    //    }
    //    /// <summary>
    //    /// 取消逻辑删除
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="logonInfo"></param>
    //    /// <returns></returns>
    //    public virtual ResultView UnDeleteById(IdType id, TokenLogonInfo logonInfo)
    //    {
    //        // 为了做还原删除的前后置操作，所以先获取要还原的数据，再做还原处理
    //        return UnDelete(_SearchById(id), logonInfo);
    //    }
    //    /// <summary>
    //    /// 取消逻辑删除
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    /// <returns></returns>
    //    public virtual ResultView UnDelete(T info, TokenLogonInfo logonInfo)
    //    {
    //        // 判断id是否有对应数据
    //        if (info == null)
    //        {
    //            throw ApiException.EmptyData("无法找到要还原的数据");
    //        }
    //        string id = info.Id.ToString();
    //        _logger.LogTrace($"还原删除数据 ==> 类型为：[{typeof(T).FullName}]\t还原ID为：[{id}]");
    //        try
    //        {
    //            // 取消逻辑删除的前置操作
    //            UnDeleting(info, logonInfo);
    //            // 取消逻辑删除的前置事件处理
    //            OnUnDeleting?.Invoke(info, logonInfo);
    //            // 取消删除状态
    //            info.UpdateDate = DateTime.Now;
    //            info.UpdaterId = logonInfo.Id;
    //            info.UpdaterName = logonInfo.Name;
    //            info.IsDelete = false;

    //            // 持久化数据
    //            _Dal.UpdateObject(info);
    //            // 还原的后置操作
    //            UnDeleted(info, logonInfo);
    //            // 取消还原删除的后置事件处理
    //            OnUnDeleted?.Invoke(info, logonInfo);

    //            // 返回保存结果
    //            return ResultView.Success(id);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, $"还原删除数据异常 ==> 类型为：[{typeof(T).FullName}]\t还原ID为：[{id}]\r\n还原对象：[{info.ToJson()}]");
    //            throw;
    //        }
    //    }
    //    #endregion

    //    #endregion

    //    #region 新增 / 修改方法

    //    #region 派生类中可重写的方法
    //    /// <summary>
    //    /// 更新的前置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void Creating(T info, TokenLogonInfo logonInfo) { }
    //    /// <summary>
    //    /// 更新的后置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void Created(T info, TokenLogonInfo logonInfo) { }

    //    /// <summary>
    //    /// 更新的前置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="old"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void Updating(T info, T old, TokenLogonInfo logonInfo) { }

    //    /// <summary>
    //    /// 更新的后置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="old"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void Updated(T info, T old, TokenLogonInfo logonInfo) { }

    //    /// <summary>
    //    /// 保存的前置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void Saving(T info, TokenLogonInfo logonInfo) { }

    //    /// <summary>
    //    /// 保存的后置操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void Saved(T info, TokenLogonInfo logonInfo) { }

    //    /// <summary>
    //    /// 保存前的数据校验
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected virtual void SavingCheck(T info, TokenLogonInfo logonInfo) { }

    //    /// <summary>
    //    /// 是否可以执行新增操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    /// <returns></returns>
    //    protected virtual bool IsExecuteCreate(T info, TokenLogonInfo logonInfo) { return true; }

    //    /// <summary>
    //    /// 是否可以执行保存操作
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="old"></param>
    //    /// <param name="logonInfo"></param>
    //    /// <returns></returns>
    //    protected virtual bool IsExecuteUpdate(T info, T old, TokenLogonInfo logonInfo) { return true; }
    //    #endregion

    //    /// <summary>
    //    /// 创建一个对象信息
    //    /// </summary>
    //    /// <param name="view">更新对象</param>
    //    /// <param name="logonInfo">登录人信息</param>
    //    /// <returns></returns>
    //    public virtual ResultView Create(V view, TokenLogonInfo logonInfo)
    //    {
    //        //将提交的数据转成实体对象
    //        var info = AutoMapper.Mapper.Map<T>(view);
    //        return Create(info, logonInfo);
    //    }
    //    /// <summary>
    //    /// 创建一个对象信息
    //    /// </summary>
    //    /// <param name="info">新增对象</param>
    //    /// <param name="logonInfo">登录人信息</param>
    //    /// <returns></returns>
    //    public virtual ResultView Create(T info, TokenLogonInfo logonInfo)
    //    {
    //        if (info == null)
    //        {
    //            ApiException.ThrowBadRequest("新增对象不能为空");
    //        }
    //        _logger.LogTrace($"新增一个对象信息 ==> 类型为：[{typeof(T).FullName}]\r\n新增对象：[{info.ToJson()}]");

    //        try
    //        {
    //            // 新增数据前的预制数据
    //            info.CreateDate = DateTime.Now;
    //            info.UpdateDate = DateTime.Now;
    //            info.UpdaterId = logonInfo.Id;
    //            info.UpdaterName = logonInfo.Name;
    //            info.CreaterId = logonInfo.Id;
    //            info.CreaterName = logonInfo.Name;
    //            info.IsDelete = false;
    //            info.ProcessState = ProcessState.Edit;

    //            #region 保存验证
    //            // 保存的数据校验
    //            SavingCheck(info, logonInfo);
    //            // 判断是否可以执行新增操作
    //            if (!IsExecuteCreate(info, logonInfo)) return ResultView.Success();
    //            // 保存数据验证的事件处理
    //            OnCreatingCheck?.Invoke(info, logonInfo);
    //            OnSaveCheck?.Invoke(info, logonInfo);
    //            #endregion

    //            #region 保存的前置处理
    //            // 新增的前置操作，可以被重写
    //            Creating(info, logonInfo);
    //            // 新增/保存的通用前置操作，可以被重写
    //            Saving(info, logonInfo);
    //            // 新增时的前置事件处理
    //            OnCreating?.Invoke(info, logonInfo);
    //            OnSaving?.Invoke(info, logonInfo);
    //            #endregion

    //            // 持久化数据
    //            _Dal.InsertObject(info);

    //            #region 保存后置处理
    //            // 新增的通用后置操作，可以被重写
    //            Created(info, logonInfo);
    //            // 新增/保存的通用后置操作，可以被重写
    //            Saved(info, logonInfo);
    //            // 新增后的后置事件处理
    //            OnCreated?.Invoke(info, logonInfo);
    //            OnSaved?.Invoke(info, logonInfo);
    //            #endregion

    //            // 返回保存结果
    //            return ResultView.Success(info.IdString);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, $"新增一个对象信息异常 ==> 类型为：[{typeof(T).FullName}]\r\n操作人信息：[{logonInfo.ToJson()}]\r\n新增对象：[{info.ToJson()}]");
    //            throw;
    //        }
    //    }


    //    /// <summary>
    //    /// 更新一个对象信息
    //    /// </summary>
    //    /// <param name="view">更新对象</param>
    //    /// <param name="logonInfo">登录人信息</param>
    //    /// <returns></returns>
    //    public virtual ResultView Update(V view, TokenLogonInfo logonInfo)
    //    {
    //        //将提交的数据转成实体对象
    //        var info = AutoMapper.Mapper.Map<T>(view);
    //        return Update(info, logonInfo);
    //    }
    //    /// <summary>
    //    /// 更新一个对象信息
    //    /// </summary>
    //    /// <param name="info">更新对象</param>
    //    /// <param name="logonInfo">登录人信息</param>
    //    /// <returns></returns>
    //    public virtual ResultView Update(T info, TokenLogonInfo logonInfo)
    //    {
    //        if (info == null)
    //        {
    //            ApiException.ThrowBadRequest("更新对象不能为空");
    //        }
    //        _logger.LogTrace($"更新一个对象信息 ==> 类型为：[{typeof(T).FullName}]\r\n操作人信息：[{logonInfo.ToJson()}]\r\n更新对象：[{info.ToJson()}]");

    //        try
    //        {
    //            // 根据ID获取原数据信息
    //            var old = _SearchById(info.Id);
    //            // 更新数据前的预制数据
    //            info.CreateDate = old.CreateDate;
    //            info.CreaterId = old.CreaterId;
    //            info.CreaterName = old.CreaterName;
    //            info.UpdateDate = DateTime.Now;
    //            info.UpdaterId = logonInfo.Id;
    //            info.UpdaterName = logonInfo.Name;
    //            info.IsDelete = false;
    //            if (info.ProcessState == ProcessState.None)
    //            {
    //                info.ProcessState = ProcessState.Edit;
    //            }

    //            #region 保存验证
    //            // 保存的数据校验，可以被重写
    //            SavingCheck(info, logonInfo);
    //            OnUpdateCheck?.Invoke(info, old, logonInfo);
    //            OnSaveCheck?.Invoke(info, logonInfo);
    //            // 判断是否可以执行新增操作，可以被重写
    //            if (!IsExecuteUpdate(info, old, logonInfo)) return ResultView.Success();
    //            #endregion

    //            #region 保存的前置处理        
    //            // 更新的前置操作，可以被重写
    //            Updating(info, old, logonInfo);
    //            // 新增/保存的通用前置操作，可以被重写
    //            Saving(info, logonInfo);
    //            // 事件处理
    //            OnUpdating?.Invoke(info, old, logonInfo);
    //            OnSaving?.Invoke(info, logonInfo);
    //            #endregion

    //            // 持久化数据
    //            _Dal.UpdateObject(info);

    //            #region 保存后置处理
    //            // 更新的后置操作，可以被重写
    //            Updated(info, old, logonInfo);
    //            // 新增/保存的通用后置操作，可以被重写
    //            Saved(info, logonInfo);
    //            // 新增后的后置事件处理
    //            OnUpdated?.Invoke(info, old, logonInfo);
    //            OnSaved?.Invoke(info, logonInfo);
    //            #endregion

    //            // 返回保存结果
    //            return ResultView.Success(info.IdString);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, $"更新一个对象信息异常 ==> 类型为：[{typeof(T).FullName}]\r\n操作人信息：[{logonInfo.ToJson()}]\r\n更新对象：[{info.ToJson()}]");
    //            throw;
    //        }
    //    }
    //    #endregion

    //    #region 审核操作 应单建一个流程子对象处理流程信息
    //    /// <summary>
    //    /// 审核通过
    //    /// </summary>
    //    /// <param name="id">审核对象ID</param>
    //    /// <param name="logonInfo">登录人信息</param>
    //    /// <param name="state">审核状态</param>
    //    public ResultView Review(IdType id, TokenLogonInfo logonInfo, ProcessState state = ProcessState.Review)
    //    {
    //        var info = _SearchById(id);
    //        info.ReleaseDate = DateTime.Now;
    //        info.UpdateDate = DateTime.Now;
    //        info.UpdaterId = logonInfo.Id;
    //        info.UpdaterName = logonInfo.Name;
    //        info.ProcessState = state;
    //        _Dal.UpdateObject(info);
    //        return ResultView.Success(info.IdString);
    //    }

    //    /// <summary>
    //    /// 审核拒绝
    //    /// </summary>
    //    /// <param name="id">审核对象ID</param>
    //    /// <param name="logonInfo">登录人信息</param>
    //    /// <param name="state">审核拒绝状态</param>
    //    public ResultView ReviewDisagreed(IdType id, TokenLogonInfo logonInfo, ProcessState state = ProcessState.Apply)
    //    {
    //        var info = _SearchById(id);
    //        info.ReleaseDate = DateTime.Now;
    //        info.UpdateDate = DateTime.Now;
    //        info.UpdaterId = logonInfo.Id;
    //        info.UpdaterName = logonInfo.Name;
    //        info.ProcessState = state;
    //        _Dal.UpdateObject(info);
    //        return ResultView.Success(info.IdString);
    //    }


    //    #endregion
    //}
}
