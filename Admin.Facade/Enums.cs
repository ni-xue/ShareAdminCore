using System;
using System.Collections.Generic;
using Tool;
using Tool.Utils;

namespace Admin.Facade
{
    /// <summary>
    /// 路由模式数组
    /// </summary>
    public partial class Rout
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        public Rout()
        {
            Controller = string.Empty;
            Action = string.Empty;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        public Rout(string Controller, string Action)
        {
            this.Controller = Controller;
            this.Action = Action;
            this.Authority = ActionEnum.NOT;
            this.PermissionType = ActionEnum.NOT;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        public Rout(string Controller, string Action, string Authority)
        {
            this.Controller = Controller;
            this.Action = Action;
            this.Authority = Rout.GetActionEnum(Authority);
            this.PermissionType = ActionEnum.NOT;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        public Rout(string Controller, string Action, ActionEnum Authority)
        {
            this.Controller = Controller;
            this.Action = Action;
            this.Authority = Authority;
            this.PermissionType = ActionEnum.NOT;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        public Rout(string Controller, MvcView mvcView)
        {
            this.Controller = Controller;
            this.Action = mvcView.RoleName;
            this.Authority = mvcView.Action;
            this.PermissionType = ActionEnum.NOT;
            this.IsRole = true;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        public Rout(string Controller, string Action, string Authority, string PermissionType)
        {
            this.Controller = Controller;
            this.Action = Action;

            this.Authority = GetActionEnum(Authority, PermissionType, out ActionEnum action);
            this.PermissionType = action;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        public Rout(string Controller, string Action, ActionEnum Authority, ActionEnum PermissionType)
        {
            this.Controller = Controller;
            this.Action = Action;
            this.Authority = Authority;
            this.PermissionType = PermissionType;
        }

        /// <summary>
        /// 路由名
        /// </summary>
        public string Controller { get; init; }
        
        /// <summary>
        /// 路由方法
        /// </summary>
        public string Action { get; init; }

        /// <summary>
        /// 当前权限
        /// </summary>
        public ActionEnum Authority { get; init; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public ActionEnum PermissionType { get; init; }

        /// <summary>
        /// 是子页面？
        /// </summary>
        public bool IsRole { get; init; } = false;


        private static ActionEnum GetActionEnum(string Authority, string PermissionType, out ActionEnum permissionType)
        {
            if (Authority.Equals(PermissionType))
            {
                permissionType = ActionEnum.All;
                return GetActionEnum(Authority);
            }

            string[] authoritys = Authority.Split(',');
            string[] permissions = PermissionType.Split(',');

            if (authoritys.Length > permissions.Length)
            {
                permissionType = GetActionEnum(PermissionType);
                return GetActionEnum(Authority);
            }
            else
            {
                foreach (string Perm in authoritys)
                {
                    if (!TextUtility.InArray(Perm, permissions))
                    {
                        permissionType = GetActionEnum(PermissionType);
                        return GetActionEnum(Authority);
                    }
                }
                permissionType = ActionEnum.All;
                return GetActionEnum(Authority);
            }
        }

        /// <summary>
        /// 权限字符串（可以是多个逗号隔开）
        /// </summary>
        /// <param name="Authority"></param>
        /// <returns></returns>
        public static ActionEnum GetActionEnum(string Authority)
        {
            string[] Authoritys = Authority.Split(',');

            return GetActionEnum(Authoritys);
        }

        /// <summary>
        /// 权限字符串（可以是多个逗号隔开）
        /// </summary>
        /// <param name="Authority"></param>
        /// <returns></returns>
        private static ActionEnum GetActionEnum(string[] Authoritys)
        {
            ActionEnum actionEnum = new ActionEnum();

            foreach (var authority in Authoritys)
            {
                if (string.IsNullOrWhiteSpace(authority))
                {
                    actionEnum |= ActionEnum.NOT;
                }
                else
                {
                    actionEnum |= (ActionEnum)Enum.Parse(typeof(ActionEnum), authority);
                }
            }

            return actionEnum;
        }

        /// <summary>
        /// 根据现有权限验证角色是否拥有权限
        /// </summary>
        /// <param name="Authority">验证页面的权限</param>
        /// <param name="PermissionType">角色的权限</param>
        /// <returns>返回是否拥有权限</returns>
        public static bool IsActionEnum(ActionEnum Authority, ActionEnum PermissionType)
        {
            if ((PermissionType & ActionEnum.NOT).ToVar<int>() != 0)
            {
                return false;
            }
            if ((PermissionType & ActionEnum.All).ToVar<int>() != 0)
            {
                return true;
            }
            return ((Authority & PermissionType).ToVar<int>() != 0);
        }

        /// <summary>
        /// 确定是否 <see cref="T:System.Collections.Generic.List`1" /> 中的每个元素都与指定的谓词所定义的条件相匹配。
        /// </summary>
        /// <param name="match">条件</param>
        /// <param name="routs">List数组</param>
        /// <returns></returns>
        public static bool TrueForAll(Predicate<Rout> match, List<Rout> routs, out Rout rout)
        {
            if (match == null)
            {
                rout = null;
                return false;
            }

            for (int i = 0; i < routs.Count; i++)
            {
                if (match(routs[i]))
                {
                    rout = routs[i];
                    return true;
                }
            }
            rout = null;
            return false;
        }

        /// <summary>
        /// 返回是否存在相同的内容
        /// </summary>
        /// <returns></returns>
        public static bool Contains(List<Rout> routs, Rout rout)
        {
            //无用方法，微软写的有毛病，我已重写。
            //return routs.TrueForAll(r => r.Controller == rout.Controller && r.Action == rout.Action);// ? true : false
            return TrueForAll(r => r.Controller.ToLower() == rout.Controller.ToLower() && r.Action.ToLower() == rout.Action.ToLower(), routs, out Rout m_rout);
        }

        /// <summary>
        /// 返回是否存在相同的内容
        /// </summary>
        /// <returns></returns>
        public static bool Contains(List<Rout> routs, Rout rout, out Rout m_rout)
        {
            //无用方法，微软写的有毛病，我已重写。
            //return routs.TrueForAll(r => r.Controller == rout.Controller && r.Action == rout.Action);// ? true : false
            return TrueForAll(r => r.Controller.ToLower() == rout.Controller.ToLower() && r.Action.ToLower() == rout.Action.ToLower(), routs, out m_rout);
        }

        /// <summary>
        /// 返回是否存在相同的内容
        /// </summary>
        /// <returns></returns>
        public static bool Contains(List<Rout> routs, string Action)
        {
            //无用方法，微软写的有毛病，我已重写。
            //return routs.TrueForAll(r => r.Controller == rout.Controller && r.Action == rout.Action);// ? true : false
            return TrueForAll(r => r.Action.ToLower() == Action.ToLower(), routs, out Rout m_rout);
        }

        /// <summary>
        /// 返回是否存在相同的内容
        /// </summary>
        /// <returns></returns>
        public static bool Contains(List<Rout> routs, string Action, out Rout m_rout)
        {
            //无用方法，微软写的有毛病，我已重写。
            //return routs.TrueForAll(r => r.Controller == rout.Controller && r.Action == rout.Action);// ? true : false
            return TrueForAll(r => r.Action.ToLower() == Action.ToLower(), routs, out m_rout);
        }
    }

    /// <summary>
    /// 统一管理操作枚举
    /// </summary>
    [Flags]
    public enum ActionEnum : short
    {
        /// <summary>
        /// 无任何权限
        /// </summary>
        NOT = 0,
        /// <summary>
        /// 所有
        /// </summary>
        All = 1,
        /// <summary>
        /// 显示
        /// </summary>
        Show = 2,
        /// <summary>
        /// 查看
        /// </summary>
        View = 4,
        /// <summary>
        /// 添加
        /// </summary>
        Add = 8,
        /// <summary>
        /// 修改
        /// </summary>
        Edit = 16,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 32
    }

    ///// <summary>
    ///// 系统导航菜单类别枚举
    ///// </summary>
    //public enum NavigationEnum
    //{
    //    /// <summary>
    //    /// 系统后台菜单
    //    /// </summary>
    //    System,
    //    /// <summary>
    //    /// 会员中心导航
    //    /// </summary>
    //    Users,
    //    /// <summary>
    //    /// 网站主导航
    //    /// </summary>
    //    WebSite
    //}
}



