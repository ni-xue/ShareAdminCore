/*
 * 版本： 4.5
 * 日期：2020/9/24 17:08:25
 * 
 * 描述：实体类
 * 
 */

using System;
using System.Collections.Generic;

namespace Admin.Entity.WebDB
{

    /// <summary>
    /// 实体类 BaseUser  (属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BaseUser
    {

        #region 常量 

        /// <summary>
        /// 表名
        /// </summary>
        public const string Tablename = "BaseUser";

        /// <summary>
        /// ID，描述:管理员标识
        /// </summary>
        public const string _ID = "ID";

        /// <summary>
        /// BaseName，描述:登录名称
        /// </summary>
        public const string _BaseName = "BaseName";

        /// <summary>
        /// BasePwd，描述:登录密码
        /// </summary>
        public const string _BasePwd = "BasePwd";

        /// <summary>
        /// BaseRank，描述:管理员等级（如：超级管理员）
        /// </summary>
        public const string _BaseRank = "BaseRank";

        /// <summary>
        /// BaseRankId，描述:等级id
        /// </summary>
        public const string _BaseRankId = "BaseRankId";

        /// <summary>
        /// AddTime，描述:创建时间
        /// </summary>
        public const string _AddTime = "AddTime";

        /// <summary>
        /// Nullity，描述:0,正常 1 禁用
        /// </summary>
        public const string _Nullity = "Nullity";

        /// <summary>
        /// IsIdent，描述:0未绑定 1 已绑定
        /// </summary>
        public const string _IsIdent = "IsIdent";

        /// <summary>
        /// LastLoginIP，描述:上次登陆时间
        /// </summary>
        public const string _LastLoginIP = "LastLoginIP";

        /// <summary>
        /// LastLoginDate，描述:上次登陆时间
        /// </summary>
        public const string _LastLoginDate = "LastLoginDate";

        /// <summary>
        /// FinalLoginIP，描述:最后登陆时间
        /// </summary>
        public const string _FinalLoginIP = "FinalLoginIP";

        /// <summary>
        /// FinalLoginDate，描述:最后登陆时间
        /// </summary>
        public const string _FinalLoginDate = "FinalLoginDate";

        #endregion

        #region 私有变量

        private int p_id;
        private string p_basename;
        private string p_basepwd;
        private string p_baserank;
        private int p_baserankid;
        private DateTime p_addtime;
        private int p_nullity;
        private int p_isident;
        private string p_lastloginip;
        private DateTime p_lastlogindate;
        private string p_finalloginip;
        private DateTime p_finallogindate;

        #endregion

        #region 构造函数 

        /// <summary>
        /// 初始化BaseUser
        /// </summary>
        public BaseUser()
        {
            p_id = 0;
            p_basename = string.Empty;
            p_basepwd = string.Empty;
            p_baserank = string.Empty;
            p_baserankid = 0;
            p_addtime = DateTime.Now;
            p_nullity = 0;
            p_isident = 0;
            p_lastloginip = string.Empty;
            p_lastlogindate = DateTime.Now;
            p_finalloginip = string.Empty;
            p_finallogindate = DateTime.Now;
        }

        #endregion

        #region 公共属性 

        /// <summary>
        /// ID，描述:管理员标识
        /// </summary>
        public int ID
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
        /// BaseName，描述:登录名称
        /// </summary>
        public string BaseName
        {
            set
            {
                p_basename = value;
            }
            get
            {
                return p_basename;
            }
        }

        /// <summary>
        /// BasePwd，描述:登录密码
        /// </summary>
        public string BasePwd
        {
            set
            {
                p_basepwd = value;
            }
            get
            {
                return p_basepwd;
            }
        }

        /// <summary>
        /// BaseRank，描述:管理员等级（如：超级管理员）
        /// </summary>
        public string BaseRank
        {
            set
            {
                p_baserank = value;
            }
            get
            {
                return p_baserank;
            }
        }

        /// <summary>
        /// BaseRankId，描述:等级id
        /// </summary>
        public int BaseRankId
        {
            set
            {
                p_baserankid = value;
            }
            get
            {
                return p_baserankid;
            }
        }

        /// <summary>
        /// AddTime，描述:创建时间
        /// </summary>
        public DateTime AddTime
        {
            set
            {
                p_addtime = value;
            }
            get
            {
                return p_addtime;
            }
        }

        /// <summary>
        /// Nullity，描述:0,正常 1 禁用
        /// </summary>
        public int Nullity
        {
            set
            {
                p_nullity = value;
            }
            get
            {
                return p_nullity;
            }
        }

        /// <summary>
        /// IsIdent，描述:0未绑定 1 已绑定
        /// </summary>
        public int IsIdent
        {
            set
            {
                p_isident = value;
            }
            get
            {
                return p_isident;
            }
        }

        /// <summary>
        /// LastLoginIP，描述:上次登陆时间
        /// </summary>
        public string LastLoginIP
        {
            set
            {
                p_lastloginip = value;
            }
            get
            {
                return p_lastloginip;
            }
        }

        /// <summary>
        /// LastLoginDate，描述:上次登陆时间
        /// </summary>
        public DateTime LastLoginDate
        {
            set
            {
                p_lastlogindate = value;
            }
            get
            {
                return p_lastlogindate;
            }
        }

        /// <summary>
        /// FinalLoginIP，描述:最后登陆时间
        /// </summary>
        public string FinalLoginIP
        {
            set
            {
                p_finalloginip = value;
            }
            get
            {
                return p_finalloginip;
            }
        }

        /// <summary>
        /// FinalLoginDate，描述:最后登陆时间
        /// </summary>
        public DateTime FinalLoginDate
        {
            set
            {
                p_finallogindate = value;
            }
            get
            {
                return p_finallogindate;
            }
        }

        #endregion
    }
}

