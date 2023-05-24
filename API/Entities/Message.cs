using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public Memory Memory { get; set; }
        public string MemoryId { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public User Sender { get; set; }
        public string Text { get; set; }
        public DateTime DateSend { get; set; } = DateTime.UtcNow;
    }
}