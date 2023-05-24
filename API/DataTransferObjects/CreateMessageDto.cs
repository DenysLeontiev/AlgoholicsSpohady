using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataTransferObjects
{
    public class CreateMessageDto
    {
        public string MemoryId { get; set; }
        public string MessageText { get; set; }
    }
}