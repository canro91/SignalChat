using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SignalChat.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LoginViewModel
    {
        [Required]
        [RegularExpression(pattern: "[a-zA-Z_-]")]
        [JsonProperty(PropertyName = "u")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "p")]
        public string Password { get; set; }
    }
}
