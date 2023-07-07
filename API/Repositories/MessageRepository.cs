using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Helpers;
using API.Interfaces.MessageInterfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(DataContext context) : base(context)
        {
            
        }

        public void CreateMessage(Message message)
        {
            Create(message);
        }

        public void DeleteMessage(Message message)
        {
            Delete(message);
        }

        public async Task<PagedList<Message>> GetMessagesForMemory(string memoryId, MessageParams messageParams) // messageParams = null
        {
            var memory = await _context.Memories.Include(x => x.Messages)
                                          .Include(x => x.Users)
                                          .FirstOrDefaultAsync(m => m.Id == memoryId);
            var messagesToReturn = memory.Messages.OrderBy(x => x.DateSend);
            return PagedList<Message>.CreateAsync(messagesToReturn, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessagesForMemoryWithNoPagination(string memoryId) // messageParams = null
        {
            // var memory = await _context.Memories.Include(x => x.Messages)
            //                               .Include(x => x.Users)
            //                               .FirstOrDefaultAsync(m => m.Id == memoryId);

            var memory = await _context.Memories.Include(x => x.Messages).FirstOrDefaultAsync(m => m.Id == memoryId);

            var messagesToReturn = memory.Messages.OrderBy(x => x.DateSend).ToList();
            return messagesToReturn;
        }
    }
}