using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Facade.PermissionsMenu
{
    /// <summary>
    /// 菜单结果对象
    /// </summary>
    public class MenusInfoResult
    {
        public MenusInfoResult()
        {
            UserInfo = new();
            LogoInfo = new();
            HomeInfo = new();
            MenuInfo = new();
        }

        /// <summary>
        /// 权限菜单树
        /// </summary>
        public List<SystemMenu> MenuInfo { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public LogoInfo LogoInfo { get; set; }

        /// <summary>
        /// Home
        /// </summary>
        public HomeInfo HomeInfo { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public UserInfo UserInfo { get; set; }
    }

    /// <summary>
    /// 登录用户的信息
    /// </summary>
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsEdit { get; set; }
    }

    /// <summary>
    /// 登录后的主要内容
    /// </summary>
    public class LogoInfo
    {
        public string Title { get; set; } = "管理系统";
        public string Image { get; set; } = "images/logo.png";
        public string Href { get; set; } = "";
    }

    /// <summary>
    /// 默认显示页
    /// </summary>
    public class HomeInfo
    {
        public string Title { get; set; } = "首页";
        public string Href { get; set; } = "Admin/Welcome";

    }

    /// <summary>
    /// 树结构对象
    /// </summary>
    public class SystemMenu
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public long PId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// 新开Tab方式
        /// </summary>
        public string Target { get; set; } = "_self"; //_blank

        /// <summary>
        /// 菜单图标样式
        /// </summary>
        public string Icon { get; set; } = "";

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<SystemMenu> Child { get; set; }
    }
}
