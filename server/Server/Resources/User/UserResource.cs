using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Server.API.Server.Resources.User
{
    public class UserResource
    {
        [SwaggerSchema("User identifier.")]
        [DefaultValue(1)]
        public int Id { get; set; }
        [SwaggerSchema("User first name.")]
        [DefaultValue("Test")]
        public string FirstName { get; set; } = string.Empty;
        [SwaggerSchema("User last name.")]
        [DefaultValue("Test")]
        public string LastName { get; set; } = string.Empty;
        [SwaggerSchema("User email, can be used like an identifier.")]
        [DefaultValue("test@test.com")]
        public string Email { get; set; } = string.Empty;
        [SwaggerSchema("User password.")]
        [DefaultValue("test123")]
        public string Password { get; set; } = string.Empty;
        [SwaggerSchema("User bytes image profile.")]
        [DefaultValue("")]
        public byte[]? ImageProfile { get; set; }
    }
}
