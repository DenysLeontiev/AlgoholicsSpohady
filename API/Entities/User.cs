using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class User : IdentityUser
    {
        public List<Memory> Memories { get; set; } = new();
        public List<Memory> LikedMemories { get; set; } = new();
    }
}