using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataTransferObjects
{
    public class RemoveUserFromMemoryDto
    {
        [Required]
        public string MemoryId { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}