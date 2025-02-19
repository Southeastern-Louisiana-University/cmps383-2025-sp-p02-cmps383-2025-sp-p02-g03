using Selu383.SP25.P02.Api.Features.Roles;
using System.ComponentModel.DataAnnotations;

namespace Selu383.SP25.P02.Api.Features.Users
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }
        public List<UserRole> UserRoles { get; set; } = new();
    }
}
