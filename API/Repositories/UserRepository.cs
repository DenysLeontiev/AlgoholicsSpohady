using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces.UserInterfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).Include(l => l.LikedMemories)
                                              .Include(x => x.Memories)
                                              .ThenInclude(p => p.Photos)
                                              .ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email, bool trackChanges)
        {
            return await _context.Users.Include(l => l.LikedMemories)
                                       .Include(x => x.Memories)
                                       .ThenInclude(p => p.Photos)
                                       .FirstOrDefaultAsync(x => x.Email.Equals(email));

            // return await FindByCondition(u => u.Email.Equals(email), trackChanges);
        }

        public async Task<User> GetUserByIdAsync(string id, bool trackChanges)
        {
            return await _context.Users.Include(l => l.LikedMemories)
                                       .Include(x => x.Memories)
                                       .ThenInclude(p => p.Photos)
                                       .FirstOrDefaultAsync(x => x.Id.Equals(id));

            // return await FindByCondition(u => u.Id.Equals(id), trackChanges);
        }

        public async Task<User> GetUserByUsernameAsync(string username, bool trackChanges)
        {
            return await _context.Users.Include(l => l.LikedMemories)
                                       .Include(x => x.Memories)
                                       .ThenInclude(p => p.Photos)
                                       .FirstOrDefaultAsync(x => x.UserName.Equals(username));

            // return await FindByCondition(u => u.UserName.Equals(username), trackChanges);
        }
    }
}