using System.Net;
using Oxide.Core;
using WebSocketSharp;
using WebSocketSharp.Server;
using MessageEventArgs = WebSocketSharp.MessageEventArgs;

namespace Oxide.Ext.Socket
{
    public class SocketListener : WebSocketBehavior
    {
        private readonly Socket Parent;
        private IPAddress Address;

        public SocketListener(Socket parent)
        {
            Parent = parent;
        }

        public void SendMessage(RemoteMessage message) => Sessions.Broadcast(message.ToJson());
        
        protected override void OnClose(CloseEventArgs e)
        {
            var reason = string.IsNullOrEmpty(e.Reason) ? "Unknown" : e.Reason;
            Interface.Oxide.LogInfo($"[Socket] Connection from {Address} closed: {reason} ({e.Code})");
        }

        protected override void OnError(ErrorEventArgs e) => Interface.Oxide.LogException(e.Message, e.Exception);
        
        protected override void OnMessage(MessageEventArgs e) => Parent?.OnMessage(e, Context);
        
        protected override void OnOpen()
        {
            Address = Context.UserEndPoint.Address;
            Interface.Oxide.LogInfo($"[Socket] New connection from {Address}");
        }
    }
}