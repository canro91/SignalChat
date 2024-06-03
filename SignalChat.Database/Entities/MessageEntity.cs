using ServiceStack.DataAnnotations;

namespace SignalChat.Database.Entities
{
    [Alias("Message")]
    public class MessageEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Username { get; set; }

        [Required]
        [StringLength(2048)]
        public string Body { get; set; }

        [Required]
        public DateTimeOffset DeliveredAt { get; set; }
    }
}