using System.Collections.Generic;
using System.Text;
using Tool;

namespace Admin.Facade
{
    /// <summary>
    /// 用于操作角色的权限操作
    /// </summary>
    public class UserPermissionOperation
    {
        public int RoleId = 0;

        public StringBuilder Builder = new();

        public readonly List<RoleEntity> Roles;

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="json"></param>
        public UserPermissionOperation(string json, int RoleId)
        {
            Roles = json.JsonList<RoleEntity>();
            this.RoleId = RoleId;

            ActionType();
        }

        /// <summary>
        /// 操作数据集合
        /// </summary>
        public void ActionType()
        {
            foreach (var Role in Roles)
            {
                if (Role.ActionType.Equals("Add"))
                {
                    Builder.AppendFormat(" INSERT INTO [dbo].[AllocationDirectory]([RoleId],[PermissionId],[PermissionType]) VALUES ({0},{1},N'{2}') ", RoleId, Role.PermissionId, Role.PermissionType);
                }
                else if (Role.ActionType.Equals("Edit"))
                {
                    Builder.AppendFormat(" UPDATE [dbo].[AllocationDirectory] SET [RoleId] = {0},[PermissionId] = {1},[PermissionType] = N'{2}' WHERE Id = {3} ", RoleId, Role.PermissionId, Role.PermissionType, Role.Id);
                }
                else if (Role.ActionType.Equals("Delete"))
                {
                    Builder.AppendFormat(" DELETE FROM [dbo].[AllocationDirectory]WHERE Id={0} ", Role.Id);
                }
            }
        }

        /// <summary>
        /// 获取执行的SQL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Builder.ToString();
        }
    }

    /// <summary>
    /// 角色的权限实体
    /// </summary>
    public class RoleEntity
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 页面ID
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// 当前用户的权限
        /// </summary>
        public string PermissionType { get; set; }

        /// <summary>
        /// 当前记录操作方式 Add,Edit,Delete
        /// </summary>
        public string ActionType { get; set; }
    }
}
