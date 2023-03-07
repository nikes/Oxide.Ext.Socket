using System;
using Newtonsoft.Json;
using Oxide.Core;

namespace Oxide.Ext.Socket
{
    [Serializable]
    public class RemoteMessage
    {
        public string Type;
        public string Data;

        public static RemoteMessage Create(string type, string data) => new RemoteMessage
        {
            Type = type,
            Data = data
        };

        public static RemoteMessage GetMessage(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<RemoteMessage>(json);
            }
            catch (JsonReaderException)
            {
                Interface.Oxide.LogError("[Socket] Failed to parse message, incorrect format");
                return null;
            }
        }

        internal string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}