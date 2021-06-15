using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.Web.Session;

namespace Admin.Facade.Session
{
    /// <summary>
    /// 一个公共的Session壳
    /// </summary>
    public class ShellSession
    {
        public static void InitSession<T>()
        {
            SessionType = typeof(T);
        }

        public static Type SessionType { get; private set; }

        public static DiySession GetSession(string SpareId = "0000000000")
        {
            if (SessionType == typeof(CacheSession))
            {
                return new CacheSession() { SpareId = SpareId };
            }
            else if (SessionType == typeof(RedisSession))
            {
                return new RedisSession() { SpareId = SpareId };
            }
            return default;
        }
    }
}
