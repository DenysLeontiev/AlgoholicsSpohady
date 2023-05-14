using API.DataTransferObjects;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            User userToCreate = new User
            {
                UserName = userForRegistrationDto.UserName,
                Email = userForRegistrationDto.Email,
            };

            var result = await _userManager.CreateAsync(userToCreate, userForRegistrationDto.Password);

            if(result.Succeeded)
            {
                return Ok(userToCreate);
            }

            return BadRequest(result.Errors);
        }
    }
}