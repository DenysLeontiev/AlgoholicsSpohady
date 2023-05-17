using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataTransferObjects
{
    public class RemoveUserFromMemoryDto
    {
        public string MemoryId { get; set; }
        public string UserName { get; set; }
    }
}