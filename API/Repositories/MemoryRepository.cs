using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces.MemoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class MemoryRepository : RepositoryBase<Memory>, IMemoryRepository
    {
        public MemoryRepository(DataContext context) : base(context)
        {
        }

        public void AddMemory(Memory memory)
        {
            Create(memory);
        }

        public void DeleteMemory(Memory memory)
        {
            Delete(memory);
        }

        public async Task<IEnumerable<Memory>> GetAllMemoriesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).Include(x => x.Users)
                                              .Include(p => p.Photos)
                                              .ToListAsync();
        }


        public async Task<Memory> GetMemoryByIdAsync(string id, bool trackChanges)
        {
            return await _context.Memories.Include(x => x.Users)
                                          .Include(x => x.Photos)
                                          .FirstOrDefaultAsync(x => x.Id == id);
            // return await FindByCondition(m => m.Id.Equals(id), trackChanges);
        }
    }
}