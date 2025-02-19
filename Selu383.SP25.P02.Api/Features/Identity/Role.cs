using Microsoft.AspNetCore.Identity;


namespace Selu383.SP25.P02.Api.Features.Identity
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRoles> Users { get; set; } = new List<UserRoles>();
    }
}
