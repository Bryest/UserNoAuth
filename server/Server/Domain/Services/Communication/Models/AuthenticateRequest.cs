using System.ComponentModel.DataAnnotations;

namespace Server.API.Security.Domain.Services.Communication.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
