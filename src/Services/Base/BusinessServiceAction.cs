using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 服务对应的事件处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusinessServiceAction<T>
    {
        #region SearchById事件处理
        /// <summary>
        /// 查询到Info对象后的事件处理
        /// </summary>
        static public Action<T> OnSearchInfoById;
        #endregion

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

        /// <summary>
        /// 逻辑删除的 删除前检测
        /// </summary>
        static public Action<T, TokenLogonInfo> OnDeleteCheck;
        /// <summary>
        /// 物理删除的 删除前检测
        /// </summary>
        static public Action<T, TokenLogonInfo> OnRemoveCheck;
        /// <summary>
        /// 逻辑与物理删除的 删除前检测
        /// </summary>
        static public Action<T, TokenLogonInfo> OnDeleteRemoveCheck;
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
    }
}
