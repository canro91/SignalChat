using System;
using Newtonsoft.Json;
using SignalChat.Core.Domain;

namespace SignalChat.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MessageViewModel
    {
        public MessageViewModel(Message t)
        {
            this.Username = t.Username;
            this.Body = t.Body;
            this.DeliveredAt = t.DeliveredAt;
        }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "deliveredAt")]
        public DateTimeOffset DeliveredAt { get; set; }
    }
}
