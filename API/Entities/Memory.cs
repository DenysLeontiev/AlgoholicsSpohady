using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Memory
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MemoryUrl { get; set; }
        public string MemoryQrCode { get; set; }
        public DateTime DateCreated { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Photo> Photos { get; set; } = new();
    }
}