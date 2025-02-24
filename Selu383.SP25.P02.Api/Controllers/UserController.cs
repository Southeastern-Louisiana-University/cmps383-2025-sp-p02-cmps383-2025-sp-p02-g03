using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP25.P02.Api.Data;
using Selu383.SP25.P02.Api.Features.Identity;
using Selu383.SP25.P02.Api.Features.Users;

namespace Selu383.SP25.P02.Api.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _dataContext;

        public UserController(UserManager<User> userManager, DataContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto dto)
        { 
            if (dto == null || string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Username and password are required");
            }

            bool existingUser = _dataContext.Users.Any(u => u.UserName == dto.UserName);
            if(existingUser)
            {
                return BadRequest("Username exists");
            }

            if (dto.Roles.Any(Role => !_dataContext.Roles.Any(r => r.Name == role)))
            {
                return BadRequest("Roles are required");
            }

            if (dto.Roles.Any(role = !_dataContext.Roles.Any(r => r.Name = role)))
            {
                return BadRequest("Role does not exist");
            }

            var user = new User
            {
                UserName = dto.UserName
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Password doens't meet the requirements");
            }

            foreach(var role in dto.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = user.Roles.Select(r => r.Role.Name).ToArray()
            };

            return Ok(userDto);
        }   
    }
}
