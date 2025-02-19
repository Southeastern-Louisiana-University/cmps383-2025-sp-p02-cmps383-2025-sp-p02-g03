using Microsoft.AspNetCore.Identity;

namespace Selu383.SP25.P02.Api.Features.Identity
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<UserRoles> Roles { get; set; } = new List<UserRoles>();
    }
}
