using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Selu383.SP25.P02.Api.Features.Users

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            // Validate input DTO exists
            if (dto == null)
            {
                return BadRequest("User data is required.");
            }

            // Validate required properties
            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                return BadRequest("UserName is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Password is required.");
            }

            if (dto.Roles == null || !dto.Roles.Any())
            {
                return BadRequest("At least one role must be provided.");
            }

            // Check for duplicate username
            var existingUser = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            // Validate that each provided role exists
            foreach (var role in dto.Roles)
            {
                if (string.IsNullOrWhiteSpace(role) || !await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"Role '{role}' does not exist.");
                }
            }

            // Create the new IdentityUser (Email is optional)
            var user = new IdentityUser
            {
                UserName = dto.UserName
            };

            // Create the user using ASP Identity (password rules enforced here)
            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors);
            }

            // Assign the roles
            var addRoleResult = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!addRoleResult.Succeeded)
            {
                return BadRequest(addRoleResult.Errors);
            }

            // Prepare the response DTO
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = dto.Roles.ToArray()
            };

            return Ok(userDto);
        }
    }
}
