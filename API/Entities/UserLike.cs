using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class UserLike
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string MemoryId { get; set; }
    }
}