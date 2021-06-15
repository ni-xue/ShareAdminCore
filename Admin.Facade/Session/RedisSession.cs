using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.Web.Session;
using Tool.Sockets.TcpFrame;

namespace Admin.Facade.Session
{
    public class RedisSession : DiySession
    {
        /// <summary>
        /// 当主无效时使用
        /// </summary>
        public string SpareId { get; set; } = "0000000000";

        /// <summary>
        /// 自定义，用于区分
        /// </summary>
        public new string Id => $"Admin:{base.Id ?? SpareId}";

        private static void IsSuccess(TcpResponse response) 
        {
            if (response.OnTcpFrame != Tool.Sockets.SupportCode.TcpFrameState.Success)
            {
                throw response.Exception ?? new Exception("未知错误！");
            }
        }

        public override void Initialize()
        {
            var api = new ApiPacket(10, 5);
            api.Set("sessionid", Id);
            var response = TcpFrame.Send(api);

            IsSuccess(response);
        }

        public override IEnumerable<string> GetKeys()
        {
            var api = new ApiPacket(10, 0);
            api.Set("sessionid", Id);
            var response = TcpFrame.Send(api);

            IsSuccess(response);

            var _keys = response.Obj == null ? Array.Empty<string>() : response.Obj.Split('|').AsEnumerable();
            return _keys;
        }

        public override bool TryGetValue(string key, out byte[] value)
        {
            var api = new ApiPacket(10, 1);
            api.Set("sessionid", Id);
            api.Set("key", key);
            var response = TcpFrame.Send(api);

            IsSuccess(response);

            value = response.Bytes;

            return value != null;
        }

        public override void Set(string key, byte[] value)
        {
            var api = new ApiPacket(10, 2);
            api.Set("sessionid", Id);
            api.Set("key", key);
            api.Bytes = value;
            var response = TcpFrame.Send(api);

            IsSuccess(response);
        }

        public override void Remove(string key)
        {
            var api = new ApiPacket(10, 3);
            api.Set("sessionid", Id);
            api.Set("key", key);
            var response = TcpFrame.Send(api);

            IsSuccess(response);
        }

        public override void Clear()
        {
            var api = new ApiPacket(10, 4);
            api.Set("sessionid", Id);
            var response = TcpFrame.Send(api);

            IsSuccess(response);
        }
    }
}
