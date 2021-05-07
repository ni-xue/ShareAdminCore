using System.Collections.Generic;

namespace Admin.Facade.PermissionsMenu
{
    /// <summary>
    /// 头部菜单
    /// </summary>
    public class MenuHead
    {
        /// <summary>
        /// 初始化菜单对象
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Title"></param>
        /// <param name="ImgStyle"></param>
        /// <param name="Sort_Id"></param>
        /// <param name="HideStatus"></param>
        /// <param name="Authority"></param>
        /// <param name="PermissionType"></param>
        /// <param name="ActionId"></param>
        /// <param name="MenuLefts"></param>
        public MenuHead(int Id, string Title, string ImgStyle, int Sort_Id, int HideStatus, string Authority, string PermissionType, int ActionId, List<MenuLeft> MenuLefts)
        {
            this.Id = Id;
            this.Title = Title;
            this.ImgStyle = ImgStyle;
            this.Sort_Id = Sort_Id;
            this.HideStatus = HideStatus;
            this.Authority = Authority;
            this.PermissionType = PermissionType;
            this.ActionId = ActionId;
            this.MenuLefts = MenuLefts;
        }

        /// <summary>
        /// 菜单id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 样式
        /// </summary>
        public string ImgStyle { get; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int Sort_Id { get; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int HideStatus { get; }

        /// <summary>
        /// 当前权限
        /// </summary>
        public string Authority { get; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public string PermissionType { get; }

        /// <summary>
        /// 角色权限Id
        /// </summary>
        public int ActionId { get; }

        /// <summary>
        /// 左菜单对象
        /// </summary>
        public List<MenuLeft> MenuLefts { get; }
    }
}
