using System.Text;

namespace Admin.Facade
{
    public class AppConfig
    {
        #region 固定值

        public const string SellerKeyId = "KEYID";

        public const string GameKeyId = "GameKey";

        #endregion

        #region 枚举
        /// <summary>
        /// 站点配置KEY
        /// </summary>
        public enum SiteConfigKey
        {

        }


        public enum CodeMode
        {
            /// <summary>
            /// 开发模式（内部测试）
            /// </summary>
            Dev,
            /// <summary>
            /// 演示模式（演示平台）
            /// </summary>
            Demo,
            /// <summary>
            /// 生产模式（客户版本）
            /// </summary>
            Production
        }
        #endregion
    }

    /// <summary>
    /// Sql 条件+排序对象
    /// </summary>
    public class WhereSort
    {
        public string SortKey { get; init; } = "Id";

        public string SortType { get; init; }

        /// <summary>
        /// 索引页
        /// </summary>
        public int Page { get; init; } = 1;

        /// <summary>
        /// 页大小
        /// </summary>
        public int Limit { get; init; } = 50;

        public StringBuilder WhereQuery { get; } = new StringBuilder(" where 1=1 ");

        /// <summary>
        /// 增加更多条件
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public WhereSort Append(string where)
        {
            WhereQuery.Append(' ').Append(where);
            return this;
        }

        /// <summary>
        /// 增加更多条件
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="args">占位符的实际值</param>
        /// <returns></returns>
        public WhereSort AppendFormat(string where, params object[] args)
        {
            WhereQuery.Append(' ').AppendFormat(where, args);
            return this;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(SortType) ? "无排序条件！" : $"ORDER BY {SortKey} {SortType} ";
        }

        public string IsSort(string _default)
        {
            return string.IsNullOrEmpty(SortType) ? _default : $"ORDER BY {SortKey} {SortType} ";
        }

        public string Where()
        {
            return WhereQuery.ToString();
        }
    }
}
