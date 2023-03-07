using Oxide.Core;
using Oxide.Core.Configuration;
using Oxide.Core.Libraries;
using WebSocketSharp;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;

namespace Oxide.Ext.Socket
{
    public class Socket : Library
    {
        private readonly OxideConfig.OxideRcon config = Interface.Oxide.Config.Rcon;

        private WebSocketServer _server;
        private SocketListener _listener;

        public Socket()
        {
            _server = new WebSocketServer(44406);
            _server.AddWebSocketService($"/socket/{config.Password}", () => _listener = new SocketListener(this));
        }

        public void Initialize()
        {
            _server.Start();
            Interface.Oxide.LogInfo("Socket Loaded on port 44406");
        }

        public override void Shutdown()
        {
            _server.Stop();
            _server = null;
        }

        internal void OnMessage(MessageEventArgs e, WebSocketContext connection)
        {
            var message = RemoteMessage.GetMessage(e.Data);
            Interface.CallHook($"OnSocket{message.Type}", connection, message.Data);
        }
        
        public void SendMessage(string type, string data) => _listener?.SendMessage(RemoteMessage.Create(type, data));
    }
}