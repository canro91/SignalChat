using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SignalChat.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RegisterViewModel
    {
        [Required]
        [StringLength(256, MinimumLength = 5)]
        [RegularExpression(pattern: "[a-zA-Z_-]+")]
        [JsonProperty(PropertyName = "u")]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        [JsonProperty(PropertyName = "p")]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [Compare(nameof(Password))]
        [JsonProperty(PropertyName = "p2")]
        public string ConfirmPassword { get; set; }
    }
}
