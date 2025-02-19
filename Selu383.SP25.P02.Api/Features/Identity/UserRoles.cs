using Microsoft.AspNetCore.Identity;

namespace Selu383.SP25.P02.Api.Features.Identity
{
    public class UserRoles : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
