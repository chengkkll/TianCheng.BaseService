using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianCheng.DAL.MongoDB;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 通用基本服务
    /// </summary>
    public class BusinessService<T, V, Q> : IInfoService<T>
        where T : BusinessMongoModel, new()
        where Q : QueryInfo
    {
        #region 构造方法
        /// <summary>
        /// 数据操作对象
        /// </summary>
        protected MongoOperation<T> _Dal;
        /// <summary>
        /// 具体业务日志工具
        /// </summary>
        protected readonly ILogger<BusinessService<T, V, Q>> _logger;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="logger"></param>
        public BusinessService(MongoOperation<T> dal, ILogger<BusinessService<T, V, Q>> logger)
        {
            _Dal = dal;
            _logger = logger;
        }
        #endregion

        #region 获取一个操作实例
        /// <summary>
        /// 
        /// </summary>
        static private BusinessService<T, V, Q> _Service = null;
        /// <summary>
        /// 
        /// </summary>
        static public BusinessService<T, V, Q> Service
        {
            get { return _Service; }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public BusinessService()
        {
            if (_Service == null)
            {
                _Service = this;
            }
        }
        #endregion

        #region 所有查询功能

        #region SearchById
        #region SearchById事件处理
        /// <summary>
        /// 查询到Info对象后的事件处理
        /// </summary>
        static public Action<T> OnSearchInfoById;
        /// <summary>
        /// 转换View后的事件处理
        /// </summary>
        static public Action<V> OnSearchViewById;
        #endregion
        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T _SearchById(string id)
        {
            // 必须输入ID
            if (String.IsNullOrWhiteSpace(id))
            {
                throw ApiException.BadRequest("请求的id不能为空。");
            }
            // 根据ID获取信息
            var info = _Dal.SearchById(id);
            // 查询到Info对象后的事件处理
            if (OnSearchInfoById != null)
            {
                OnSearchInfoById(info);
            }
            return info;
        }

        /// <summary>
        /// 根据id查询对象信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual V SearchById(string id)
        {
            //根据ID获取员工信息
            var info = _SearchById(id);
            //返回
            V view = AutoMapper.Mapper.Map<V>(info);

            // 转换View后的事件处理
            if (OnSearchViewById != null)
            {
                OnSearchViewById(view);
            }

            return view;
        }
        #endregion

        #region 无条件查询所有
        /// <summary>
        /// 无条件查询所有
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> SearchQueryable()
        {
            return _Dal.Queryable();
        }
        #endregion

        #region 基础数据过滤  Filter
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual IQueryable<T> _Filter(Q input)
        {
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

        #region 无分页数据查询
        /// <summary>
        /// 无分页查询 获取默认的View对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<V> Filter(Q input)
        {
            return Filter<V>(input);
        }
        /// <summary>
        /// 无分页查询 获取指定的View对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<OV> Filter<OV>(Q input)
        {
            var list = _Filter(input).ToList();
            var viewList = AutoMapper.Mapper.Map<List<OV>>(list);
            return viewList ?? new List<OV>();
        }
        #endregion

        #region 根据分页/分步信息过滤数据
        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="filter">查询条件（包括分页及排序的信息）</param>
        /// <param name="queryResult">所有满足过滤条件的数据</param>
        /// <param name="resultPagination">返回的分页信息</param>
        protected virtual IQueryable<T> SetFilterPagination(Q filter, IQueryable<T> queryResult, PagedResultPagination resultPagination)
        {
            #region 设置分页信息及返回值
            //初始化查询条件
            if (filter.Pagination == null) filter.Pagination = QueryPagination.DefaultObject;
            if (filter.Pagination.PageMaxRecords == 0) { filter.Pagination.PageMaxRecords = QueryPagination.DefaultPageMaxRecords; }

            //设置返回的分页信息
            resultPagination.PageMaxRecords = filter.Pagination.PageMaxRecords;
            resultPagination.TotalRecords = queryResult.Count();
            resultPagination.TotalPage = (int)Math.Ceiling((double)resultPagination.TotalRecords / (double)resultPagination.PageMaxRecords);
            //设置页号
            resultPagination.Index = filter.Pagination.Index;
            if (resultPagination.Index > resultPagination.TotalPage) { resultPagination.Index = resultPagination.TotalPage; }
            if (resultPagination.Index == 0) { resultPagination.Index = 1; }
            //返回查询的数据结果
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
            var viewList = AutoMapper.Mapper.Map<List<OV>>(infoList);
            return new PagedResult<OV>(viewList, pagination);
        }
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
        /// 获取所有的记录条数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _Dal.Queryable().Count();
        }
        /// <summary>
        /// 根据条件获取记录数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Count(Q input)
        {
            var queryResult = _Filter(input);
            return queryResult.Count();
        }
        #endregion

        #endregion

        #region 删除方法

        #region 删除事件处理
        /// <summary>
        /// 逻辑删除的前置事件处理
        /// </summary>
        static public Action<T, TokenLogonInfo> OnDeleting;
        /// <summary>
        /// 逻辑删除后的事件处理
        /// </summary>
        static public Action<T, TokenLogonInfo> OnDeleted;

        /// <summary>
        /// 取消逻辑删除的前置事件处理
        /// </summary>
        static public Action<T, TokenLogonInfo> OnUnDeleting;
        /// <summary>
        /// 取消逻辑删除后的事件处理
        /// </summary>
        static public Action<T, TokenLogonInfo> OnUnDeleted;

        /// <summary>
        /// 物理删除的前置事件处理
        /// </summary>
        static public Action<T, TokenLogonInfo> OnRemoving;
        /// <summary>
        /// 物理删除后的事件处理
        /// </summary>
        static public Action<T, TokenLogonInfo> OnRemoved;
        #endregion

        #region 派生类中可重写的方法
        /// <summary>
        /// 删除的通用验证处理
        /// </summary>
        /// <param name="info"></param>
        protected virtual void DeleteRemoveCheck(T info)
        {
            //如果验证有问题抛出异常
        }
        /// <summary>
        /// 逻辑删除的验证处理
        /// </summary>
        /// <param name="info"></param>
        protected virtual void DeleteCheck(T info)
        {
            //如果验证有问题抛出异常
        }
        /// <summary>
        /// 物理删除的验证处理
        /// </summary>
        /// <param name="info"></param>
        protected virtual void RemoveCheck(T info)
        {
            //如果验证有问题抛出异常
        }

        /// <summary>
        /// 逻辑删除的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo">登录的用户信息</param>
        protected virtual void Deleted(T info, TokenLogonInfo logonInfo) { }
        /// <summary>
        /// 取消逻辑删除的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo">登录的用户信息</param>
        protected virtual void UnDeleted(T info, TokenLogonInfo logonInfo) { }
        /// <summary>
        /// 物理删除的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo">登录的用户信息</param>
        protected virtual void Removed(T info, TokenLogonInfo logonInfo) { }
        #endregion

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">要删除的数据id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        public ResultView Delete(string id, TokenLogonInfo logonInfo)
        {
            // 判断id是否有对应数据
            T info = _SearchById(id);
            // 判断对应数据是否可以删除.
            DeleteRemoveCheck(info);

            DeleteCheck(info);
            // 逻辑删除前置事件处理
            if (OnDeleting != null)
            {
                OnDeleting(info, logonInfo);
            }
            // 设置删除状态
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.IsDelete = true;

            // 持久化数据
            _Dal.Update(info);
            Deleted(info, logonInfo);
            // 逻辑删除后置事件处理
            if (OnDeleted != null)
            {
                OnDeleted(info, logonInfo);
            }
            // 返回保存结果
            return ResultView.Success(id);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">要删除的数据id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        public ResultView Remove(string id, TokenLogonInfo logonInfo)
        {
            //判断id是否有对应数据
            T info = _SearchById(id);
            return Remove(info, logonInfo);
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public ResultView Remove(T info, TokenLogonInfo logonInfo)
        {
            if (info == null)
            {
                throw ApiException.EmptyData("删除的数据无法找到，");
            }
            //判断对应数据是否可以删除.
            DeleteRemoveCheck(info);
            RemoveCheck(info);
            // 物理删除时的前置事件处理
            if (OnRemoving != null)
            {
                OnRemoving(info, logonInfo);
            }
            //持久化数据
            _Dal.Remove(info);
            Removed(info, logonInfo);
            // 物理删除时的后置事件处理
            if (OnRemoved != null)
            {
                OnRemoved(info, logonInfo);
            }
            //返回保存结果
            return ResultView.Success(info.Id.ToString());
        }

        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public virtual ResultView UnDelete(string id, TokenLogonInfo logonInfo)
        {
            // 判断id是否有对应数据
            T info = _SearchById(id);

            // 取消逻辑删除的前置事件处理
            if (OnUnDeleting != null)
            {
                OnUnDeleting(info, logonInfo);
            }
            // 取消删除状态
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.IsDelete = false;

            // 持久化数据
            _Dal.Update(info);
            UnDeleted(info, logonInfo);

            // 取消逻辑删除的后置事件处理
            if (OnUnDeleted != null)
            {
                OnUnDeleted(info, logonInfo);
            }

            // 返回保存结果
            return ResultView.Success(id);
        }

        #endregion

        #region 新增 / 修改方法

        #region 派生类中可重写的方法
        /// <summary>
        /// 更新的前置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Creating(T info, TokenLogonInfo logonInfo) { }
        /// <summary>
        /// 更新的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Created(T info, TokenLogonInfo logonInfo) { }

        /// <summary>
        /// 更新的前置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Updating(T info, T old, TokenLogonInfo logonInfo) { }

        /// <summary>
        /// 更新的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Updated(T info, T old, TokenLogonInfo logonInfo) { }

        /// <summary>
        /// 保存的前置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Saving(T info, TokenLogonInfo logonInfo) { }

        /// <summary>
        /// 保存的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Saved(T info, TokenLogonInfo logonInfo) { }

        /// <summary>
        /// 保存前的数据校验
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void SavingCheck(T info, TokenLogonInfo logonInfo) { }

        /// <summary>
        /// 是否可以执行新增操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        protected virtual bool IsExecuteCreate(T info, TokenLogonInfo logonInfo) { return true; }

        /// <summary>
        /// 是否可以执行保存操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        protected virtual bool IsExecuteUpdate(T info, T old, TokenLogonInfo logonInfo) { return true; }
        #endregion

        #region 新增和修改事件处理
        /// <summary>
        /// 新增前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
        /// </summary>
        static public Action<T, TokenLogonInfo> OnCreatingCheck;
        /// <summary>
        /// 新增前置事件
        /// </summary>
        static public Action<T, TokenLogonInfo> OnCreating;
        /// <summary>
        /// 新增后置事件
        /// </summary>
        static public Action<T, TokenLogonInfo> OnCreated;

        /// <summary>
        /// 更新前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
        /// </summary>
        static public Action<T, T, TokenLogonInfo> OnUpdateCheck;
        /// <summary>
        /// 更新前置事件
        /// </summary>
        static public Action<T, T, TokenLogonInfo> OnUpdating;
        /// <summary>
        /// 更新后置事件
        /// </summary>
        static public Action<T, T, TokenLogonInfo> OnUpdated;

        /// <summary>
        /// 保存前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
        /// </summary>
        static public Action<T, TokenLogonInfo> OnSaveCheck;
        /// <summary>
        /// 保存前置事件
        /// </summary>
        static public Action<T, TokenLogonInfo> OnSaving;
        /// <summary>
        /// 保存后置事件
        /// </summary>
        static public Action<T, TokenLogonInfo> OnSaved;
        #endregion

        /// <summary>
        /// 创建一个对象信息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public virtual ResultView Create(V view, TokenLogonInfo logonInfo)
        {
            //将提交的数据转成实体对象
            var info = AutoMapper.Mapper.Map<T>(view);
            return Create(info, logonInfo);
        }
        /// <summary>
        /// 创建一个对象信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public virtual ResultView Create(T info, TokenLogonInfo logonInfo)
        {
            // 新增数据前的预制数据
            info.CreateDate = DateTime.Now;
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.CreaterId = logonInfo.Id;
            info.CreaterName = logonInfo.Name;
            info.IsDelete = false;
            info.ProcessState = ProcessState.Edit;

            #region 保存验证
            // 保存的数据校验
            SavingCheck(info, logonInfo);
            // 判断是否可以执行新增操作
            if (!IsExecuteCreate(info, logonInfo)) return ResultView.Success();
            // 保存数据验证的事件处理
            if (OnCreatingCheck != null)
            {
                OnCreatingCheck(info, logonInfo);
            }
            if (OnSaveCheck != null)
            {
                OnSaveCheck(info, logonInfo);
            }
            #endregion

            #region 保存的前置处理
            // 新增的前置操作，可以被重写
            Creating(info, logonInfo);
            // 新增/保存的通用前置操作，可以被重写
            Saving(info, logonInfo);
            // 新增时的前置事件处理
            if (OnCreating != null)
            {
                OnCreating(info, logonInfo);
            }
            if (OnSaving != null)
            {
                OnSaving(info, logonInfo);
            }
            #endregion

            // 持久化数据
            _Dal.Insert(info);

            #region 保存后置处理
            // 新增的通用后置操作，可以被重写
            Created(info, logonInfo);
            // 新增/保存的通用后置操作，可以被重写
            Saved(info, logonInfo);
            // 新增后的后置事件处理
            if (OnCreated != null)
            {
                OnCreated(info, logonInfo);
            }
            if (OnSaved != null)
            {
                OnSaved(info, logonInfo);
            }
            #endregion

            // 返回保存结果
            return ResultView.Success(info.Id);
        }


        /// <summary>
        /// 更新一个对象信息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public virtual ResultView Update(V view, TokenLogonInfo logonInfo)
        {
            //将提交的数据转成实体对象
            var info = AutoMapper.Mapper.Map<T>(view);
            return Update(info, logonInfo);
        }
        /// <summary>
        /// 更新一个对象信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public virtual ResultView Update(T info, TokenLogonInfo logonInfo)
        {
            // 根据ID获取原数据信息
            var old = _SearchById(info.Id.ToString());
            // 更新数据前的预制数据
            info.CreateDate = old.CreateDate;
            info.CreaterId = old.CreaterId;
            info.CreaterName = old.CreaterName;
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.IsDelete = false;
            if (info.ProcessState == ProcessState.None)
            {
                info.ProcessState = ProcessState.Edit;
            }

            #region 保存验证
            // 保存的数据校验，可以被重写
            SavingCheck(info, logonInfo);
            if (OnUpdateCheck != null)
            {
                OnUpdateCheck(info, old, logonInfo);
            }
            if (OnSaveCheck != null)
            {
                OnSaveCheck(info, logonInfo);
            }
            // 判断是否可以执行新增操作，可以被重写
            if (!IsExecuteUpdate(info, old, logonInfo)) return ResultView.Success();
            #endregion

            #region 保存的前置处理        
            // 更新的前置操作，可以被重写
            Updating(info, old, logonInfo);
            // 新增/保存的通用前置操作，可以被重写
            Saving(info, logonInfo);
            // 事件处理
            if (OnUpdating != null)
            {
                OnUpdating(info, old, logonInfo);
            }
            if (OnSaving != null)
            {
                OnSaving(info, logonInfo);
            }
            #endregion

            // 持久化数据
            _Dal.Update(info);

            #region 保存后置处理
            // 更新的后置操作，可以被重写
            Updated(info, old, logonInfo);
            // 新增/保存的通用后置操作，可以被重写
            Saved(info, logonInfo);
            // 新增后的后置事件处理
            if (OnUpdated != null)
            {
                OnUpdated(info, old, logonInfo);
            }
            if (OnSaved != null)
            {
                OnSaved(info, logonInfo);
            }
            #endregion

            // 返回保存结果
            return ResultView.Success(info.Id);
        }
        #endregion

        #region 审核操作
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        public ResultView Review(string id, TokenLogonInfo logonInfo)
        {
            var info = _SearchById(id);
            info.ReleaseDate = DateTime.Now;
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.ProcessState = ProcessState.Review;
            _Dal.Update(info);
            return ResultView.Success(info.Id);
        }

        /// <summary>
        /// 审核拒绝
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        public ResultView ReviewDisagreed(string id, TokenLogonInfo logonInfo)
        {
            var info = _SearchById(id);
            info.ReleaseDate = DateTime.Now;
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.ProcessState = ProcessState.Apply;
            _Dal.Update(info);
            return ResultView.Success(info.Id);
        }


        #endregion
    }
}
