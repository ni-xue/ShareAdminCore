using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Facade;
using Tool.Web.Api;
using Tool.Web.Routing;

namespace AdminCore.WebUi
{
    public class Admin : MinApi
    {
        protected override IApiOut Initialize(AshxRouteData ashxRoute)
        {
            if (!ashxRoute.HttpContext.Session.IsAvailable) return ApiOut.View("500.html");
            return RoleAction.Initialize(ashxRoute);
        }

        /// <summary>
        /// 后台首页
        /// </summary>
        /// <returns></returns>
        //[AshxRoute("index.html")]
        public async Task<IApiOut> Index() => await ApiOut.ViewAsync("Views/index.html");

        /// <summary>
        /// 后台首页
        /// </summary>
        /// <returns></returns>
        //[AshxRoute("login.html")]
        public async Task<IApiOut> Login() => await ApiOut.ViewAsync("Views/login.html");


        /// <summary>
        /// 默认页面
        /// </summary>
        /// <returns></returns>
        //[AshxRoute("login.html")]
        public async Task<IApiOut> Welcome() => await ApiOut.ViewAsync("Views/welcome.html");


        #region 权限页面

        /// <summary>
        /// 玩家列表
        /// </summary>
        /// <returns></returns>
        public IApiOut AccountList() => ApiOut.View();

        #region 系统管理

        /// <summary>
        /// 菜单管理
        /// </summary>
        /// <returns></returns>
        public async Task<IApiOut> MenuList() => await ApiOut.PathViewAsync("System");

        #region 菜单管理-子页
        /// <summary>
        /// 菜单新增修改
        /// </summary>
        /// <returns></returns>
        [MvcView("MenuList", ActionEnum.Add | ActionEnum.Edit)]
        public async Task<IApiOut> MenuEdil() => await ApiOut.PathViewAsync("System");
        #endregion

        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        public async Task<IApiOut> ManageList() => await ApiOut.PathViewAsync("System");

        #region 用户管理-子页
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        [MvcView("ManageList", ActionEnum.Add | ActionEnum.Edit)]
        public async Task<IApiOut> ManageEdit() => await ApiOut.PathViewAsync("System");
        #endregion

        /// <summary>
        /// 角色管理
        /// </summary>
        /// <returns></returns>
        public async Task<IApiOut> RoleManageList() => await ApiOut.PathViewAsync("System");

        #region 角色管理-子页

        /// <summary>
        /// 角色管理
        /// </summary>
        /// <returns></returns>
        [MvcView("RoleManageList", ActionEnum.Add | ActionEnum.Edit)]
        public async Task<IApiOut> RoleManageEdit() => await ApiOut.PathViewAsync("System");

        #endregion

        /// <summary>
        /// 管理日志
        /// </summary>
        /// <returns></returns>
        public async Task<IApiOut> ManageLogList() => await ApiOut.PathViewAsync("System");

        #endregion

        #endregion

        protected override IApiOut AshxException(AshxException ex)
        {
            Tool.Utils.Log.Fatal("全局错误：", ex);
            return ApiOut.View("404.html");
        }

    }
}
