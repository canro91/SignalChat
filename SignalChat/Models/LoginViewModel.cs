﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SignalChat.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LoginViewModel
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
    }
}
