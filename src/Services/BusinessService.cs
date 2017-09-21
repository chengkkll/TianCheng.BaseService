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
        protected DALCommon<T> _Dal;
        /// <summary>
        /// 具体业务日志工具
        /// </summary>
        protected readonly ILogger<BusinessService<T, V, Q>> _logger;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="logger"></param>
        public BusinessService(DALCommon<T> dal, ILogger<BusinessService<T, V, Q>> logger)
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
        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T _SearchById(string id)
        {
            //必须输入ID
            if (String.IsNullOrWhiteSpace(id))
            {
                throw ApiException.BadRequest("请求的id不能为空。");
            }
            //根据ID获取信息
            var info = _Dal.SearchById(id);
           
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
            return AutoMapper.Mapper.Map<V>(info);
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
            var query = _Dal.SearchQueryable();

            #region 查询条件
            //不显示删除的数据
            query = query.Where(e => e.IsDelete == false);
            #endregion

            #region 设置排序规则
            //设置排序方式
            switch (input.OrderBy)
            {
                case "dateAsc": { query = query.OrderBy(e => e.UpdateDate); break; }
                case "dateDesc": { query = query.OrderByDescending(e => e.UpdateDate); break; }
                default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
            }
            #endregion

            //返回查询结果
            return query;
        }
        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="filter">查询条件（包括分页及排序的信息）</param>
        /// <param name="queryResult">所有满足过滤条件的数据</param>
        /// <param name="resultPagination">返回的分页信息</param>
        protected IQueryable<T> SetFilterPagination(Q filter, IQueryable<T> queryResult, PagedResultPagination resultPagination)
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

        /// <summary>
        /// 获取指定类型的返回值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual List<T> _FilterStep(Q input)
        {
            var queryResult = _Filter(input);
            if (input.Pagination == null) input.Pagination = new QueryPagination() { Index = 1, PageMaxRecords = 10 };
            if (input.Pagination.Index <= 0) input.Pagination.Index = 1;
            if (input.Pagination.PageMaxRecords <= 0) input.Pagination.PageMaxRecords = 10;

            PagedResultPagination resultPage = new PagedResultPagination();
            queryResult = SetFilterPagination(input, queryResult, resultPage);
            if (resultPage.TotalPage < input.Pagination.Index)
            {
                return new List<T>();
            }

            return queryResult.ToList();
        }

        /// <summary>
        /// 获取指定类型的返回值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual List<TO> FilterStep<TO>(Q input)
        {
            var infoList = _FilterStep(input);
            return AutoMapper.Mapper.Map<List<TO>>(infoList);
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 无分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual List<V> Filter(Q input)
        {
            var list = _Filter(input).ToList();
            var viewList = AutoMapper.Mapper.Map<List<V>>(list);
            return viewList ?? new List<V>();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual PagedResult<V> FilterPage(Q input)
        {
            var queryResult = _Filter(input);
            PagedResultPagination pagination = new PagedResultPagination();
            queryResult = SetFilterPagination(input, queryResult, pagination);
            var infoList = queryResult.ToList();
            var viewList = AutoMapper.Mapper.Map<List<V>>(infoList);
            return new PagedResult<V>(viewList, pagination);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual PagedResult<T> _FilterPage(Q input)
        {
            var queryResult = _Filter(input);
            PagedResultPagination pagination = new PagedResultPagination();
            queryResult = SetFilterPagination(input, queryResult, pagination);
            var infoList = queryResult.ToList();

            return new PagedResult<T>(infoList, pagination);
        }



        /// <summary>
        /// 获取一个查询列表对象形式的集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual List<SelectView> Select(Q input)
        {
            var list = _Filter(input);
            return AutoMapper.Mapper.Map<List<SelectView>>(list);
        }

        /// <summary>
        /// 获取满足条件的前几条数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual List<SelectView> SelectTop(Q input)
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

        #region 无条件查询所有
        /// <summary>
        /// 无条件查询所有
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> SearchQueryable()
        {
            return _Dal.SearchQueryable();
        }
        #endregion
        #endregion

        #region 删除方法
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
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">要删除的数据id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        public virtual ResultView Delete(string id, TokenLogonInfo logonInfo)
        {
            //判断id是否有对应数据
            T info = _SearchById(id);
            //判断对应数据是否可以删除.
            DeleteRemoveCheck(info);
            DeleteCheck(info);
            //设置删除状态
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.IsDelete = true;

            //持久化数据
            _Dal.Save(info);
            Deleted(info, logonInfo);
            //返回保存结果
            return ResultView.Success(id);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">要删除的数据id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        public virtual ResultView Remove(string id, TokenLogonInfo logonInfo)
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
        public virtual ResultView Remove(T info, TokenLogonInfo logonInfo)
        {
            if (info == null)
            {
                throw ApiException.EmptyData("删除的数据无法找到，");
            }
            //判断对应数据是否可以删除.
            DeleteRemoveCheck(info);
            RemoveCheck(info);
            //持久化数据
            _Dal.Remove(info);
            Removed(info, logonInfo);
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
            //判断id是否有对应数据
            T info = _SearchById(id);

            //取消删除状态
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.IsDelete = false;

            //持久化数据
            _Dal.Update(info);
            UnDeleted(info, logonInfo);
            //返回保存结果
            return ResultView.Success(id);
        }

        #endregion

        #region 新增 / 修改方法
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
            //新增数据前的预制数据
            info.CreateDate = DateTime.Now;
            info.UpdateDate = DateTime.Now;
            info.UpdaterId = logonInfo.Id;
            info.UpdaterName = logonInfo.Name;
            info.CreaterId = logonInfo.Id;
            info.CreaterName = logonInfo.Name;
            info.IsDelete = false;
            info.ProcessState = ProcessState.Edit;
            //保存的数据校验
            SavingCheck(info, logonInfo);
            //判断是否可以执行新增操作
            if (!IsExecuteCreate(info, logonInfo)) return ResultView.Success();
            //新增的前置操作，可以被重写
            Creating(info, logonInfo);
            //新增/保存的通用前置操作，可以被重写
            Saving(info, logonInfo);
            //持久化数据
            _Dal.Insert(info);
            //新增的通用后置操作，可以被重写
            Created(info, logonInfo);
            //新增/保存的通用后置操作，可以被重写
            Saved(info, logonInfo);
            //返回保存结果
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
            //根据ID获取原数据信息
            var old = _SearchById(info.Id.ToString());
            //更新数据前的预制数据
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
            //保存的数据校验，可以被重写
            SavingCheck(info, logonInfo);
            //判断是否可以执行新增操作，可以被重写
            if (!IsExecuteUpdate(info, old, logonInfo)) return ResultView.Success();
            //更新的前置操作，可以被重写
            Updating(info, old, logonInfo);
            //新增/保存的通用前置操作，可以被重写
            Saving(info, logonInfo);
            //持久化数据
            _Dal.Save(info);
            //更新的后置操作，可以被重写
            Updated(info, old, logonInfo);
            //新增/保存的通用后置操作，可以被重写
            Saved(info, logonInfo);
            //返回保存结果
            return ResultView.Success(info.Id);
        }
        #endregion


        #region 审核操作
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        public ResultView Review(string id , TokenLogonInfo logonInfo)
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
