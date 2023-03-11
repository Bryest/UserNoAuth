using Microsoft.AspNetCore.Mvc;
using Server.API.Security.Domain.Services.Communication;
using Server.API.Security.Domain.Services.Communication.Models;
using Server.API.Server.Domain.Models;
using Server.API.Server.Domain.Services.Communication;

namespace Server.API.Server.Domain.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> ListAsync();
        Task<User> FindByIdAsync(int id);
        Task<User> FindByEmailAsync(string email);
        Task<UserResponse> SaveAsync(User user);
        Task<UserResponse> UpdateAsync(int id, User user);
        Task<UserResponse> DeleteAsync(int id);
    }
}
