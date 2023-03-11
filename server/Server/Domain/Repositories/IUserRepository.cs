using Server.API.Server.Domain.Models;

namespace Server.API.Server.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> ListAsync();
        Task<User> FindByIdAsync(int id);
        Task<User> FindByEmailAsync(string email);
        Task AddAsync(User user);
        public void Update(User user);
        public void Remove(User user);

    }
}
