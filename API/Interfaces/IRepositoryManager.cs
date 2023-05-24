using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.MemoryInterfaces;
using API.Interfaces.MessageInterfaces;
using API.Interfaces.UserInterfaces;

namespace API.Interfaces
{
    public interface IRepositoryManager
    {
        IMemoryRepository Memory { get; }
        IUserRepository User { get; }
        IMessageRepository Message { get; }
        Task SaveAsync();
    }
}