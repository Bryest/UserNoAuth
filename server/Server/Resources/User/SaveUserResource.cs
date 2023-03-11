using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Server.API.Server.Resources.User
{
    public class SaveUserResource
    {
        [Required]
        [SwaggerSchema("User first name")]
        [DefaultValue("Test")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [SwaggerSchema("User last name.")]
        [DefaultValue("Test")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [SwaggerSchema("User email.")]
        [DefaultValue("test@test.com")]
        public string Email { get; set; } = string.Empty ;
        [Required]
        [SwaggerSchema("User password.")]
        [DefaultValue("test123")]
        public string Password { get; set; } = string.Empty ;
        [SwaggerSchema("User image profile.")]
        [DefaultValue("")]
        public IFormFile? ImageProfile { get; set; }
    }
}
