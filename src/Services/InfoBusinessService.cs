using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TianCheng.Model;

namespace TianCheng.BaseService.Services
{
    ///// <summary>
    ///// 基本信息业务处理
    ///// </summary>
    //public class InfoBusinessService<T, IdType> : IInfoBusinessService<T, IdType>
    //     where T : IBusinessModel<IdType>, new()
    //{

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

    //    #region 新增和修改事件处理
    //    /// <summary>
    //    /// 新增前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
    //    /// </summary>
    //    static public Action<T, TokenLogonInfo> OnCreatingCheck;
    //    /// <summary>
    //    /// 新增前置事件
    //    /// </summary>
    //    static public Action<T, TokenLogonInfo> OnCreating;
    //    /// <summary>
    //    /// 新增后置事件
    //    /// </summary>
    //    static public Action<T, TokenLogonInfo> OnCreated;

    //    /// <summary>
    //    /// 更新前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
    //    /// </summary>
    //    static public Action<T, T, TokenLogonInfo> OnUpdateCheck;
    //    /// <summary>
    //    /// 更新前置事件
    //    /// </summary>
    //    static public Action<T, T, TokenLogonInfo> OnUpdating;
    //    /// <summary>
    //    /// 更新后置事件
    //    /// </summary>
    //    static public Action<T, T, TokenLogonInfo> OnUpdated;

    //    /// <summary>
    //    /// 保存前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
    //    /// </summary>
    //    static public Action<T, TokenLogonInfo> OnSaveCheck;
    //    /// <summary>
    //    /// 保存前置事件
    //    /// </summary>
    //    static public Action<T, TokenLogonInfo> OnSaving;
    //    /// <summary>
    //    /// 保存后置事件
    //    /// </summary>
    //    static public Action<T, TokenLogonInfo> OnSaved;
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

    //    T IInfoBusinessService<T, IdType>._SearchById(string id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    ResultView IInfoBusinessService<T, IdType>.Create(T info, TokenLogonInfo logonInfo)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    ResultView IInfoBusinessService<T, IdType>.Update(T info, TokenLogonInfo logonInfo)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IQueryable<T> IInfoBusinessService<T, IdType>.SearchQueryable()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    #endregion
    //}
}
