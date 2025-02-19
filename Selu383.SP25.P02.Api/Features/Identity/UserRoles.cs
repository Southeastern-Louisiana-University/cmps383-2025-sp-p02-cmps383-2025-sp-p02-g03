using Microsoft.AspNetCore.Identity;

namespace Selu383.SP25.P02.Api.Features.Identity
{
    public class UserRoles : IdentityUserRole<int>
    {
        public required User User { get; set; }
        public required Role Role { get; set; }
    }
}
