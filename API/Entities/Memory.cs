using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Memory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MemoryUrl { get; set; }
        public string MemoryQrCode { get; set; }
        public string OwnerId { get; set; }
        public DateTime DateCreated { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Photo> Photos { get; set; } = new();
        public List<Message> Messages{ get; set; } = new();
    }
}