using Microsoft.AspNetCore.Identity;

namespace Selu383.SP25.P02.Api.Features.Users
{
    public class CreateUserDto
    {
       public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string Password { get; set; }
    }
}
