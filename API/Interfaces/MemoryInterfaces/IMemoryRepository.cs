using System;
using API.Entities;

namespace API.Interfaces.MemoryInterfaces
{
    public interface IMemoryRepository 
    {
        Task<Memory> GetMemoryByIdAsync(string id, bool trackChanges); 
        Task<IEnumerable<Memory>> GetAllMemoriesAsync(bool trackChanges); 
        void AddMemory(Memory memory);
        void DeleteMemory(Memory memory);
    }
}