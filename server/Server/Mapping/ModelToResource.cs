using AutoMapper;
using Server.API.Server.Domain.Models;
using Server.API.Server.Resources.User;

namespace Server.API.Server.Mapping
{
    public class ModelToResource : Profile
    {
        public ModelToResource()
        {
            CreateMap<User, UserResource>();
        }
    }
}
