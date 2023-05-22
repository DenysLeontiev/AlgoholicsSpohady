using System.Security.Claims;
using API.Data;
using API.DataTransferObjects;
using API.Entities;
using API.Interfaces;
using API.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenHandler tokenHandler,
            DataContext context,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserJwtDto>> Register([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            User userToCreate = new User
            {
                UserName = userForRegistrationDto.Username,
                Email = userForRegistrationDto.Email,
            };

            var result = await _userManager.CreateAsync(userToCreate, userForRegistrationDto.Password);

            if (result.Succeeded)
            {
                var userJwt = new UserJwtDto
                {
                    Username = userToCreate.UserName,
                    Email = userForRegistrationDto.Email,
                    Token = _tokenHandler.CreateToken(userToCreate)
                };
                return Ok(userJwt);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userFromDb = await _userManager.FindByNameAsync(userForLoginDto.Username);

            if (userFromDb == null)
            {
                return NotFound();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(userFromDb, userForLoginDto.Password, false);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var userJwt = new UserJwtDto
            {
                Username = userFromDb.UserName,
                Email = userFromDb.Email,
                Id = userFromDb.Id,
                Token = _tokenHandler.CreateToken(userFromDb)
            };
            return Ok(userJwt);
        }

        [HttpPost("google-login")]
        public async Task<ActionResult> LoginWithGoogle([FromBody] string credential)
        {
            string clientId = _config.GetSection("ClientId").Value;
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { clientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user != null)
            {
                var userJwt = new UserJwtDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Id = user.Id,
                    Token = _tokenHandler.CreateToken(user),
                };
                return Ok(userJwt);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}