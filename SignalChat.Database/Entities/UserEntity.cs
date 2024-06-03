using ServiceStack.DataAnnotations;

namespace SignalChat.Database.Entities
{
    [Alias("User")]
    internal class UserEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        [Unique]
        [StringLength(256)]
        public string? Username { get; set; }

        [Required]
        [StringLength(256)]
        public string? SaltedPassword { get; set; }
    }
}