using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.MessageInterfaces
{
    public interface IMessageRepository
    {
        void CreateMessage(Message message);
        void DeleteMessage(Message message);
        Task<PagedList<Message>> GetMessagesForMemory(string memoryId, MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessagesForMemoryWithNoPagination(string memoryId);
    }
}