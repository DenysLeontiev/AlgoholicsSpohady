using API.Entities;

namespace API.Interfaces.UserInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id, bool trackChanges); 
        Task<User> GetUserByUsernameAsync(string username, bool trackChanges); 
        Task<User> GetUserByEmailAsync(string email, bool trackChanges); 
        Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges); 
    }
}