using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.Web.Session;
using Tool.Sockets.TcpFrame;
using System.Threading;
using Tool.Utils.Data;

namespace Admin.Facade.Session
{
    public class CacheSession : DiySession
    {
        /// <summary>
        /// 当主无效时使用
        /// </summary>
        public string SpareId { get; set; } = "0000000000";

        /// <summary>
        /// 自定义，用于区分
        /// </summary>
        public new string Id => $"Admin:{base.Id ?? SpareId}";

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public const int Second = 20 * 60;

        /// <summary>
        /// 内存存储值
        /// </summary>
        private static readonly LazyConcurrentDictionary<string, (Dictionary<string, byte[]> keys, bool isdate, DateTime date)> cacheSession = new();

        private static Tool.Sockets.SupportCode.KeepAlive keep;

        public static void StartKeep(byte minute)
        {
            if (keep == null)
            {
                keep = new(minute, IsSecond);
            }
        }

        private static void IsSecond()
        {
            foreach (var pair in cacheSession)
            {
                if (pair.Value.isdate && pair.Value.date.AddSeconds(Second) < DateTime.Now)
                {
                    if (cacheSession.Remove(pair.Key, out var val))
                    {
                        val.keys.Clear();
                    }
                }
            }
        }

        public override void Initialize()
        {
            // 此处刷新 过期时间
            if (cacheSession.TryGetValue(Id, out var val))
            {
                val.date = DateTime.Now;
            }
        }

        public override IEnumerable<string> GetKeys()
        {
            if (cacheSession.TryGetValue(Id, out var val))
            {
                return val.keys.Keys;
            }
            return Array.Empty<string>();
        }

        public override bool TryGetValue(string key, out byte[] value)
        {
            if (cacheSession.TryGetValue(Id, out var val) && val.keys.TryGetValue(key, out value))
            {
                return true;
            }
            value = null;
            return false;
        }

        public override void Set(string key, byte[] value)
        {
            if (cacheSession.TryGetValue(Id, out var val))
            {
                if (!val.keys.TryAdd(key, value))
                {
                    val.keys[key] = value;
                }
            }
            else
            {
                cacheSession.TryAdd(Id, (new() { { key, value } }, base.Id != null, DateTime.Now));
            }
        }

        public override void Remove(string key)
        {
            if (cacheSession.TryGetValue(Id, out var val))
            {
                val.keys.Remove(key);
            }
        }

        public override void Clear()
        {
            if (cacheSession.Remove(Id, out var val))
            {
                val.keys.Clear();
            }
        }
    }
}
