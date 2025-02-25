using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Selu383.SP25.P02.Api.Features.Users;

namespace Selu383.SP25.P02.Api.Features.Users
{
    
    [ApiController]
    [Route("api/users")]
    
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<IdentityUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            //check if any user data given at all
            if (createUserDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            //check if any roles given
            if (createUserDto.Roles == null || !createUserDto.Roles.Any())
            {
                return BadRequest("At least one role must be provided.");
            }

            //check if given roles exist
            foreach (var role in createUserDto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"Role '{role}' does not exist.");
                }
            }

            //check if exists
            var existingUser = await _userManager.FindByNameAsync(createUserDto.UserName);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            //make new identity User of given name
            var newUser = new IdentityUser
            {
                UserName = createUserDto.UserName
            };
            
            //await creation of user and password and if not successful - return bad request.
            var createUserResult = await _userManager.CreateAsync(newUser, createUserDto.Password);
            if (!createUserResult.Succeeded)
            {
                return BadRequest(createUserResult.Errors);
            }

            //await addition of roles to the new user
            var addRolesResult = await _userManager.AddToRolesAsync(newUser, createUserDto.Roles);
            if (!addRolesResult.Succeeded)
            {
                
                await _userManager.DeleteAsync(newUser);
                return BadRequest(addRolesResult.Errors);
            }

           //make a new userDto to send back with OK 200
            var userDto = new UserDto
            {
                UserName = newUser.UserName,
                Roles = createUserDto.Roles
            };

            return Ok(userDto);
        }
        
    }
    // DTO for creating a user


}
