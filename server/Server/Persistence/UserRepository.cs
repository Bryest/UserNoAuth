using Microsoft.EntityFrameworkCore;
using Server.API.Server.Domain.Models;
using Server.API.Server.Domain.Repositories;
using Server.API.Shared.Persistence.Context;
using Server.API.Shared.Persistence.Repositories;

namespace Server.API.Server.Persistence
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u=>u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.User.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.User.Update(user);
        }

        public void Remove(User user)
        {
            _context.User.Remove(user);
        }
    }
}
