using API.Data;
using API.Interfaces;
using API.Interfaces.MemoryInterfaces;
using API.Interfaces.UserInterfaces;

namespace API.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DataContext _context;
        private IMemoryRepository _memoryRepository;
        private IUserRepository _userRepository;

        public RepositoryManager(DataContext context)
        {
            _context = context;
        }

        public IMemoryRepository Memory
        {
            get
            {
                if(_memoryRepository == null)
                    _memoryRepository = new MemoryRepository(_context);

                return _memoryRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                if(_userRepository == null)
                    _userRepository = new UserRepository(_context);

                return _userRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}