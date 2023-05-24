using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataTransferObjects
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string MemoryId { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string Text { get; set; }
        public DateTime DateSend { get; set; }
        
    }
}