using System.ComponentModel.DataAnnotations;

namespace SignalChat.Configuration;

public class Login
{
    [Required]
    public required string Secret { get; set; }
}
