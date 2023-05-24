using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.ExtensionMethods;
using API.Helpers;
using API.Interfaces.MemoryInterfaces;
using Microsoft.AspNetCore.Mvc;
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

        public PagedList<Memory> GetAllMemoriesAsync([FromQuery] UserParams userParam, bool trackChanges)
        {
            var query = _context.Memories.Include(u => u.Users)
                                         .Include(p => p.Photos)
                                         .AsNoTracking()
                                         .AsQueryable();

            return PagedList<Memory>.CreateAsync(query,
                                                       userParam.PageNumber,
                                                       userParam.PageSize);

            // return await FindAll(trackChanges).Include(x => x.Users)
            //                                   .Include(p => p.Photos)
            //                                   .ToListAsync();
        }

        public async Task<PagedList<Memory>> GetAllMemoriesForUserAsync(UserParams userParams, string id)
        {
            var user = await _context.Users.Include(m => m.Memories)
                                           .ThenInclude(p => p.Photos)
                                           .FirstOrDefaultAsync(x => x.Id.Equals(id));

            var query = user.Memories;
            if (!string.IsNullOrWhiteSpace(userParams.SearchTerm))
            {
                query = query.Where(x => x.Title.Contains(userParams.SearchTerm)).ToList();
            }

            if (userParams.OrderByType == "ASC")
            {
                query = query.OrderBy(x => x.DateCreated).ToList();
            }
            else
            {
                query = query.OrderByDescending(x => x.DateCreated).ToList();
            }

            return PagedList<Memory>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<Memory> GetMemoryByIdAsync(string id, bool trackChanges)
        {
            return await _context.Memories.Include(x => x.Users)
                                          .Include(x => x.Photos)
                                        //   .Include(x => x.Messages) TODO: Do we need that actually?
                                          .FirstOrDefaultAsync(x => x.Id == id);
            // return await FindByCondition(m => m.Id.Equals(id), trackChanges);
        }
    }
}