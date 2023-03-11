using Server.API.Server.Domain.Models;
using Server.API.Shared.Domain.Sevices;

namespace Server.API.Server.Domain.Services.Communication
{
    public class UserResponse : BaseResponse<User>
    {
        public UserResponse(User resource) : base(resource)
        {
        }

        public UserResponse(string message) : base(message)
        {
        }
    }
}
