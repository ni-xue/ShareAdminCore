using System;
using System.Collections.Generic;
using System.Reflection;
using Tool;
using Tool.Utils.ActionDelegate;

namespace Admin.Facade
{
    /// <summary>
    /// MVC权限规定
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MvcView : Attribute
    {
        /// <summary>
        /// 为当前视图绑定权限父级
        /// </summary>
        /// <param name="RoleName">父级视图名</param>
        /// <param name="Action">当前页面需要的权限</param>
        /// <param name="Remark">接口备注</param>
        public MvcView(string RoleName, ActionEnum Action, string Remark = "")
        {
            this.RoleName = RoleName;
            this.Action = Action;
            this.Remark = Remark;

        }

        /// <summary>
        /// 为当前视图绑定权限父级
        /// </summary>
        /// <param name="RoleName">父级视图名</param>
        /// <param name="Action">当前页面需要的权限</param>
        /// <param name="IsAction">当前页面是否要验证权限</param>
        public MvcView(string RoleName, ActionEnum Action, bool IsAction)
        {
            this.RoleName = RoleName;
            this.Action = Action;
            this.IsAction = IsAction;
        }

        /// <summary>
        /// 父级视图名
        /// </summary>
        public string RoleName { get; }

        /// <summary>
        /// 当前他需要的权限
        /// </summary>
        public ActionEnum Action { get; }

        /// <summary>
        /// 表示当前权限对象是否需要验证
        /// </summary>
        public bool IsAction { get; } = true;

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; }
    }
}
