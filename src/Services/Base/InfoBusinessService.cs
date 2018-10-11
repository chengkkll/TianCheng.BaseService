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
    /// 基本信息业务处理
    /// </summary>
    public class InfoBusinessService<T, IdType> : BusinessServiceAction<T>, IBusinessService<T, IdType>
         where T : IBusinessModel<IdType>, new()
    {
        #region 构造方法        
        /// <summary>
        /// 数据操作对象
        /// </summary>
        protected IDBOperation<T, IdType> _Dal;
        /// <summary>
        /// 具体业务日志工具
        /// </summary>
        protected readonly ILogger<InfoBusinessService<T, IdType>> _logger;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public InfoBusinessService(IDBOperation<T, IdType> dal)
        {
            _Dal = dal;
            _logger = ServiceLoader.GetService<ILogger<InfoBusinessService<T, IdType>>>();
        }
        #endregion

        #region 所有查询功能

        #region SearchById
        /// <summary>
        /// 根据ID列表获取对象集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> _SearchByIds(IEnumerable<string> ids)
        {
            return _Dal.SearchByIds(ids);
        }

        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T _SearchById(string id)
        {
            T info = new T();

            // 将传入ID转成对象处理
            T t = new T();
            t.SetId(id);
            // 从数据库中获取对象
            info = _SearchById(t.Id);

            // 返回查询结果对象
            return info;
        }

        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T _SearchById(IdType id)
        {
            _logger.LogTrace($"根据ID查询对象（_SearchById），对象类型为：[{typeof(T).FullName}]");
            // 检查ID是否有效
            if (!new T().CheckId(id))
            {
                _logger.LogWarning($"按ID查询对象时，ID无效。类型为：[{typeof(T).FullName}][id={id}]");
                throw ApiException.BadRequest("请求的id无效。");
            }
            // 根据ID获取信息
            var info = _Dal.SearchByTypeId(id);
            _logger.LogTrace($"根据ID查询对象，已获取对象。类型为：[{typeof(T).FullName}]\r\n对象值：[{info.ToJson()}]");
            // 查询到Info对象后的事件处理
            OnSearchInfoById?.Invoke(info);
            return info;
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

        #region Count
        /// <summary>
        /// 获取所有的记录条数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            _logger.LogTrace($"查询记录总条数 （int Count()）。查询对象类型为：[{typeof(T).FullName}]");
            return _Dal.Queryable().Count();
        }
        #endregion

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

        /// <summary>
        /// 创建一个对象信息
        /// </summary>
        /// <param name="info">新增对象</param>
        /// <param name="logonInfo">登录人信息</param>
        /// <returns></returns>
        public virtual ResultView Create(T info, TokenLogonInfo logonInfo)
        {
            if (info == null)
            {
                ApiException.ThrowBadRequest("新增对象不能为空");
            }
            _logger.LogTrace($"新增一个对象信息 ==> 类型为：[{typeof(T).FullName}]\r\n新增对象：[{info.ToJson()}]");

            try
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
                OnCreatingCheck?.Invoke(info, logonInfo);
                OnSaveCheck?.Invoke(info, logonInfo);
                #endregion

                #region 保存的前置处理
                // 新增的前置操作，可以被重写
                Creating(info, logonInfo);
                // 新增/保存的通用前置操作，可以被重写
                Saving(info, logonInfo);
                // 新增时的前置事件处理
                OnCreating?.Invoke(info, logonInfo);
                OnSaving?.Invoke(info, logonInfo);
                #endregion

                // 持久化数据
                _Dal.InsertObject(info);

                #region 保存后置处理
                // 新增的通用后置操作，可以被重写
                Created(info, logonInfo);
                // 新增/保存的通用后置操作，可以被重写
                Saved(info, logonInfo);
                // 新增后的后置事件处理
                OnCreated?.Invoke(info, logonInfo);
                OnSaved?.Invoke(info, logonInfo);
                #endregion

                // 返回保存结果
                return ResultView.Success(info.IdString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"新增一个对象信息异常 ==> 类型为：[{typeof(T).FullName}]\r\n操作人信息：[{logonInfo.ToJson()}]\r\n新增对象：[{info.ToJson()}]");
                throw;
            }
        }

        /// <summary>
        /// 更新一个对象信息
        /// </summary>
        /// <param name="info">更新对象</param>
        /// <param name="logonInfo">登录人信息</param>
        /// <returns></returns>
        public virtual ResultView Update(T info, TokenLogonInfo logonInfo)
        {
            if (info == null)
            {
                ApiException.ThrowBadRequest("更新对象不能为空");
            }
            _logger.LogTrace($"更新一个对象信息 ==> 类型为：[{typeof(T).FullName}]\r\n操作人信息：[{logonInfo.ToJson()}]\r\n更新对象：[{info.ToJson()}]");

            try
            {
                // 根据ID获取原数据信息
                var old = _SearchById(info.Id);
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
                OnUpdateCheck?.Invoke(info, old, logonInfo);
                OnSaveCheck?.Invoke(info, logonInfo);
                // 判断是否可以执行新增操作，可以被重写
                if (!IsExecuteUpdate(info, old, logonInfo)) return ResultView.Success();
                #endregion

                #region 保存的前置处理        
                // 更新的前置操作，可以被重写
                Updating(info, old, logonInfo);
                // 新增/保存的通用前置操作，可以被重写
                Saving(info, logonInfo);
                // 事件处理
                OnUpdating?.Invoke(info, old, logonInfo);
                OnSaving?.Invoke(info, logonInfo);
                #endregion

                // 持久化数据
                _Dal.UpdateObject(info);

                #region 保存后置处理
                // 更新的后置操作，可以被重写
                Updated(info, old, logonInfo);
                // 新增/保存的通用后置操作，可以被重写
                Saved(info, logonInfo);
                // 新增后的后置事件处理
                OnUpdated?.Invoke(info, old, logonInfo);
                OnSaved?.Invoke(info, logonInfo);
                #endregion

                // 返回保存结果
                return ResultView.Success(info.IdString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新一个对象信息异常 ==> 类型为：[{typeof(T).FullName}]\r\n操作人信息：[{logonInfo.ToJson()}]\r\n更新对象：[{info.ToJson()}]");
                throw;
            }
        }
        #endregion

        #region 删除方法

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
        /// 删除的通用前置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void DeleteRemoving(T info, TokenLogonInfo logonInfo)
        {
        }
        /// <summary>
        /// 逻辑删除的前置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Deleting(T info, TokenLogonInfo logonInfo)
        {
        }
        /// <summary>
        /// 取消逻辑删除的前置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void UnDeleting(T info, TokenLogonInfo logonInfo)
        {
        }
        /// <summary>
        /// 物理删除的前置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Removing(T info, TokenLogonInfo logonInfo)
        {
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
        /// 删除的通用后置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void DeleteRemoved(T info, TokenLogonInfo logonInfo)
        {
        }
        #endregion

        #region 逻辑删除
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public ResultView Delete(string id, TokenLogonInfo logonInfo)
        {
            return Delete(_SearchById(id), logonInfo);
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">要删除的数据id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        public ResultView DeleteById(IdType id, TokenLogonInfo logonInfo)
        {
            // 为了做删除的前后置操作，所以先获取要删除的数据，再做删除处理
            return Delete(_SearchById(id), logonInfo);
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="info">要删除的数据id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        public ResultView Delete(T info, TokenLogonInfo logonInfo)
        {
            if (info == null)
            {
                throw ApiException.EmptyData("无法找到要删除的数据");
            }
            string id = info.Id.ToString();
            _logger.LogTrace($"逻辑删除 ==> 类型为：[{typeof(T).FullName}]\t删除ID为：[{id}]");
            try
            {
                // 判断对应数据是否可以删除.
                DeleteRemoveCheck(info);
                OnDeleteRemoveCheck?.Invoke(info, logonInfo);
                DeleteCheck(info);
                OnDeleteCheck?.Invoke(info, logonInfo);
                // 逻辑删除的前置操作
                DeleteRemoving(info, logonInfo);
                Deleting(info, logonInfo);
                // 逻辑删除前置事件处理
                OnDeleting?.Invoke(info, logonInfo);
                // 设置逻辑删除状态
                info.UpdateDate = DateTime.Now;
                info.UpdaterId = logonInfo.Id;
                info.UpdaterName = logonInfo.Name;
                info.IsDelete = true;

                // 持久化数据
                _Dal.UpdateObject(info);
                // 逻辑删除的后置处理
                Deleted(info, logonInfo);
                DeleteRemoved(info, logonInfo);
                // 逻辑删除后置事件处理
                OnDeleted?.Invoke(info, logonInfo);
                // 返回保存结果
                return ResultView.Success(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"逻辑删除异常 ==> 类型为：[{typeof(T).FullName}]\t删除ID为：[{id}]\r\n删除对象：[{info.ToJson()}]");
                throw;
            }
        }
        #endregion

        #region 物理删除
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public ResultView Remove(string id, TokenLogonInfo logonInfo)
        {
            return Remove(_SearchById(id), logonInfo);
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">要删除的数据id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        public ResultView RemoveById(IdType id, TokenLogonInfo logonInfo)
        {
            // 为了做删除的前后置操作，所以先获取要删除的数据，再做删除处理
            return Remove(_SearchById(id), logonInfo);
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
                throw ApiException.EmptyData("无法找到要删除的数据");
            }
            string id = info.Id.ToString();
            _logger.LogTrace($"物理删除 ==> 类型为：[{typeof(T).FullName}]\t删除ID为：[{id}]");
            try
            {
                // 判断对应数据是否可以删除.
                DeleteRemoveCheck(info);
                OnDeleteRemoveCheck?.Invoke(info, logonInfo);
                RemoveCheck(info);
                OnRemoveCheck?.Invoke(info, logonInfo);
                // 物理删除的前置操作
                DeleteRemoving(info, logonInfo);
                Removing(info, logonInfo);
                // 物理删除时的前置事件处理
                OnRemoving?.Invoke(info, logonInfo);
                // 持久化数据
                _Dal.RemoveObject(info);
                // 物理删除的后置处理
                Removed(info, logonInfo);
                DeleteRemoved(info, logonInfo);
                // 物理删除时的后置事件处理
                OnRemoved?.Invoke(info, logonInfo);
                // 返回保存结果
                return ResultView.Success(info.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"物理删除异常 ==> 类型为：[{typeof(T).FullName}]\t删除ID为：[{id}]]\r\n删除对象为：[{info.ToJson()}]");
                throw;
            }
        }
        #endregion

        #region 还原删除
        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public ResultView UnDelete(string id, TokenLogonInfo logonInfo)
        {
            return UnDelete(_SearchById(id), logonInfo);
        }
        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public virtual ResultView UnDeleteById(IdType id, TokenLogonInfo logonInfo)
        {
            // 为了做还原删除的前后置操作，所以先获取要还原的数据，再做还原处理
            return UnDelete(_SearchById(id), logonInfo);
        }
        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public virtual ResultView UnDelete(T info, TokenLogonInfo logonInfo)
        {
            // 判断id是否有对应数据
            if (info == null)
            {
                throw ApiException.EmptyData("无法找到要还原的数据");
            }
            string id = info.Id.ToString();
            _logger.LogTrace($"还原删除数据 ==> 类型为：[{typeof(T).FullName}]\t还原ID为：[{id}]");
            try
            {
                // 取消逻辑删除的前置操作
                UnDeleting(info, logonInfo);
                // 取消逻辑删除的前置事件处理
                OnUnDeleting?.Invoke(info, logonInfo);
                // 取消删除状态
                info.UpdateDate = DateTime.Now;
                info.UpdaterId = logonInfo.Id;
                info.UpdaterName = logonInfo.Name;
                info.IsDelete = false;

                // 持久化数据
                _Dal.UpdateObject(info);
                // 还原的后置操作
                UnDeleted(info, logonInfo);
                // 取消还原删除的后置事件处理
                OnUnDeleted?.Invoke(info, logonInfo);

                // 返回保存结果
                return ResultView.Success(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"还原删除数据异常 ==> 类型为：[{typeof(T).FullName}]\t还原ID为：[{id}]\r\n还原对象：[{info.ToJson()}]");
                throw;
            }
        }
        #endregion

        #endregion

    }
}
