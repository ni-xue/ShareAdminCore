using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tool.Sockets.TcpFrame;

namespace Admin.Facade
{
    public class TcpFrame
    {
        public static ClientFrame AideClientFrame { private set; get; }

        private static ILogger logger;

        private static string ip;

        private static int port;

        /// <summary>
        /// 创建内置服务器
        /// </summary>
        /// <returns></returns>
        public static ClientFrame CreateServer(string ip, int port) 
        {
            AideClientFrame = new ClientFrame(Tool.Sockets.SupportCode.TcpBufferSize.Default, 108, true);
            TcpFrame.ip = ip;
            TcpFrame.port = port;
            return AideClientFrame;
        }

        public static void ConnectClient(ILoggerFactory loggerFactory) 
        {
            logger = loggerFactory.CreateLogger("Client");
            AideClientFrame.SetCompleted((a, b, c) =>
            {
                switch (b)
                {
                    case Tool.Sockets.SupportCode.EnClient.Fail:
                        logger.LogError("服务器：{a}，无法链接！-->{c}", a, c);
                        break;
                    case Tool.Sockets.SupportCode.EnClient.Connect:
                        logger.LogInformation("客户端：{a}，建立！-->{c}", a, c);
                        break;
                    case Tool.Sockets.SupportCode.EnClient.SendMsg:
                        logger.LogDebug("客户端：{a}，读！-->{c}", a, c);
                        break;
                    case Tool.Sockets.SupportCode.EnClient.Receive:
                        logger.LogDebug("客户端：{a}，写！-->{c}", a, c);
                        break;
                    case Tool.Sockets.SupportCode.EnClient.Close:
                        logger.LogWarning("服务器：{a}，已断开！-->{c}", a, c);
                        break;
                    case Tool.Sockets.SupportCode.EnClient.HeartBeat:
                        logger.LogInformation("客户端：{a}，心跳！-->{c}", a, c);
                        break;
                }
            });
            AideClientFrame.ConnectAsync(TcpFrame.ip, TcpFrame.port);

            AideClientFrame.AddKeepAlive(10);
        }

        public static ApiPacket AddApi(byte ClassID, byte ActionID, int Millisecond = 30) => new(ClassID, ActionID, Millisecond);

        public static TcpResponse Send(ApiPacket api) => AideClientFrame.Send(api);

        public static void SendAsync(ApiPacket api, Action<TcpResponse> action) => AideClientFrame.SendAsync(api, action);

        public static TcpResponse SendIpIdea(string IpPort, ApiPacket api) => AideClientFrame.SendIpIdea(IpPort, api);

        public static void SendIpIdeaAsync(string IpPort, ApiPacket api, Action<TcpResponse> action) => AideClientFrame.SendIpIdeaAsync(IpPort, api, action);

        public static void Reconnection() => AideClientFrame.Reconnection();

        public static void Close() => AideClientFrame.Close();
    }
}
