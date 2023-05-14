using API.DataTransferObjects;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userFromDb = await _userManager.FindByNameAsync(userForLoginDto.UserName);

            if(userFromDb == null)
            {
                return NotFound();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(userFromDb, userForLoginDto.Password, false);

            if(!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(userFromDb);
        }
    }
}