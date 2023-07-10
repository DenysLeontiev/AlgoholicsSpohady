using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataTransferObjects
{
    public class SetNewMemoryOwnerDto
    {
        public string NewOwnerId { get; set; }
        public string MemoryId { get; set; }
    }
}