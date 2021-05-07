/*
 * 版本： 4.5
 * 日期：2018/11/20 11:19:14
 * 
 * 描述：实体类
 * 
 */

using System;

namespace Admin.Entity.WebDB
{

    /// <summary>
    /// 实体类 AllocationDirectory  (属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class AllocationDirectory
    {

        #region 常量 

        /// <summary>
        /// 表名
        /// </summary>
        public const string Tablename = "AllocationDirectory";

        /// <summary>
        /// Id，描述:权限分配标识
        /// </summary>
        public const string _Id = "Id";

        /// <summary>
        /// RoleId，描述:角色Id关联
        /// </summary>
        public const string _RoleId = "RoleId";

        /// <summary>
        /// PermissionId，描述:关联权限目录Id
        /// </summary>
        public const string _PermissionId = "PermissionId";

        /// <summary>
        /// PermissionType，描述:权限类型
        /// </summary>
        public const string _PermissionType = "PermissionType";

        #endregion

        #region 私有变量

        private int p_id;
        private int p_roleid;
        private int p_permissionid;
        private string p_permissiontype;

        #endregion

        #region 构造函数 

        /// <summary>
        /// 初始化AllocationDirectory
        /// </summary>
        public AllocationDirectory()
        {
            p_id = 0;
            p_roleid = 0;
            p_permissionid = 0;
            p_permissiontype = string.Empty;
        }

        #endregion

        #region 公共属性 

        /// <summary>
        /// Id，描述:权限分配标识
        /// </summary>
        public int Id
        {
            set
            {
                p_id = value;
            }
            get
            {
                return p_id;
            }
        }

        /// <summary>
        /// RoleId，描述:角色Id关联
        /// </summary>
        public int RoleId
        {
            set
            {
                p_roleid = value;
            }
            get
            {
                return p_roleid;
            }
        }

        /// <summary>
        /// PermissionId，描述:关联权限目录Id
        /// </summary>
        public int PermissionId
        {
            set
            {
                p_permissionid = value;
            }
            get
            {
                return p_permissionid;
            }
        }

        /// <summary>
        /// PermissionType，描述:权限类型
        /// </summary>
        public string PermissionType
        {
            set
            {
                p_permissiontype = value;
            }
            get
            {
                return p_permissiontype;
            }
        }

        #endregion
    }
}

