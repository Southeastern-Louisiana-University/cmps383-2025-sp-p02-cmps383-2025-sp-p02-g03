using Microsoft.AspNetCore.Identity;


namespace Selu383.SP25.P02.Api.Features.Identity
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRoles> User { get; set; } = new List<UserRoles>();
    }
}
