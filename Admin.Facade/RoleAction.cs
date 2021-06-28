using System.Collections.Generic;
using System.Data;
using Tool;
using Tool.Utils.Data;
using Tool.Web;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Admin.Entity;
using Admin.Entity.WebDB;
using Admin.Facade.Session;

namespace Admin.Facade
{
    /// <summary>
    /// 获取角色权限信息
    /// </summary>
    public class RoleAction
    {
        /// <summary>
        /// 是否强制登录
        /// </summary>
        public const bool IsQiangden = false;

        /// <summary>
        /// 系统默认存储器
        /// </summary>
        public static ISession Session => ShellSession.GetSession(); // new RedisSession();// {  Id = "10000000" }

        /// <summary>
        /// 重写初始化
        /// </summary>
        /// <param name="requestContext"></param>
        public static Tool.Web.Api.IApiOut Initialize(Tool.Web.Routing.AshxRouteData ashxRoute)
        {
            var session = ashxRoute.HttpContext.Session;

            var url = string.Concat(ashxRoute.HttpContext.Request.Scheme, "://", ashxRoute.HttpContext.Request.Host);

            if (ashxRoute.HttpContext.Request.Path.Value.StartsWith("/Admin/Index", System.StringComparison.OrdinalIgnoreCase))
            {
                return Tool.Web.Api.ApiOut.Redirect(url + "/");//Risk/index-改地址不能访问-/
            }

            bool m_isOnLine = session.TryGetValue("Admin", out _);

            if (!session.TryGetValue("Routs", out List<Rout> m_shieldRouts)) m_shieldRouts = PermissionsMenu.Menu.Routs;

            Rout m_rout;
            Tool.Web.Api.IApiOut apiOut = null;

            if (ashxRoute.GetAshx.TryGetValue(out MvcView mvcView))
            {
                m_rout = new Rout(ashxRoute.Controller, mvcView);
            }
            else
            {
                m_rout = new Rout(ashxRoute.Controller, ashxRoute.Action);
            }

            if (m_rout.Action.EqualsNotCase("Login") && m_isOnLine)
            {
                apiOut = Tool.Web.Api.ApiOut.Redirect(url + "/");//Risk/index-改地址不能访问-/
            }
            else if (Rout.Contains(m_shieldRouts, m_rout, out Rout r_rout))
            {
                if (m_isOnLine)
                {
                    var rout = m_rout.IsRole ? m_rout : r_rout;
                    if (!Rout.IsActionEnum(rout.Authority, r_rout.PermissionType))
                    {
                        apiOut = Tool.Web.Api.ApiOut.Redirect(url + "/");
                    }
                }
                else
                {
                    apiOut = Tool.Web.Api.ApiOut.Redirect(url + "/Admin/login");
                }
            }

            return apiOut;
        }

        //public static bool IsUserOnline(Microsoft.AspNetCore.Http.ISession session) => session.TryGetValue("Admin", out _);

        public static void Login(ISession session, BaseUser userTicket)
        {
            if (session != null && userTicket != null)
            {
                session.Set("Admin", userTicket);
                var RoleDirID = FacadeManage.AideAdminFacade.GetBaseUserInformation(userTicket.BaseRankId);
                if (RoleDirID.Success)
                {
                    var data = RoleDirID.EntityList[0].ToVar<DataSet>();
                    var menu = PermissionsMenu.Menu.GetMenus(data, out List<Rout> routs, true);
                    session.Set("Routs", routs);
                    session.Set("Menu", menu);

                    bool isedit = false;
                    if (Rout.Contains(routs, "ManageList", out Rout rout))
                    {
                        isedit = Rout.IsActionEnum(ActionEnum.Add | ActionEnum.Edit, rout.PermissionType);//ManageEdit
                    }

                    session.Set("UserInfo", new PermissionsMenu.UserInfo()
                    {
                        Token = session.Id,
                        Id = userTicket.ID,
                        Name = userTicket.BaseName,
                        IsEdit = isedit,
                        TokenUrl = Tool.Utils.AppSettings.Get("ServerUrl")
                    });
                }

                RoleAction.Session.Set(userTicket.BaseName.ToLower(), session.Id);
            }
        }

        public static void Logout(ISession session, BaseUser userTicket)
        {
            if (session != null && userTicket != null)
            {
                session.Clear();
                RoleAction.Session.Remove(userTicket.BaseName.ToLower());
            }
        }

        public static bool Logout(string username)
        {
            if (RoleAction.Session.TryGetValue(username, out string id))
            {
                var _session = ShellSession.GetSession(id);
                _session.Clear();
                RoleAction.Session.Remove(username.ToLower());
                return true;
            }
            return false;
        }

        public static bool IsRepeatLogin(string BaseName)
        {
            if (RoleAction.Session.TryGetValue(BaseName, out string id))
            {
                var _session = ShellSession.GetSession(id); //new RedisSession() { SpareId = id };
                var keys = _session.GetKeys();//.Contains("Admin");
                if (keys.Contains("Admin"))
                {
                    if (!IsQiangden) return false;
                    _session.Clear();
                }
                RoleAction.Session.Remove(BaseName.ToLower());
                return true;
            }
            return true;
        }
    }
}
