using Server.API.Shared.Persistence.Context;

namespace Server.API.Shared.Persistence.Repositories
{
    public class BaseRepository
    {
        protected readonly AppDbContext _context;
        public BaseRepository(AppDbContext context) 
        {
            _context = context;
        }
    }
}