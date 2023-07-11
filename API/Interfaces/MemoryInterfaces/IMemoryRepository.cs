using System;
using API.Entities;
using API.ExtensionMethods;
using API.Helpers;

namespace API.Interfaces.MemoryInterfaces
{
    public interface IMemoryRepository 
    {
        Task<Memory> GetMemoryByIdAsync(string id, bool trackChanges); 
        PagedList<Memory> GetAllMemoriesAsync(UserParams userParams,bool trackChanges); 
        Task<PagedList<Memory>> GetAllMemoriesForUserAsync(UserParams userParams, string userName);
        Task<PagedList<Memory>> GetLikedMemoriesForUserAsync(UserParams userParams, string userName);
        void AddMemory(Memory memory);
        void DeleteMemory(Memory memory);
    }
}