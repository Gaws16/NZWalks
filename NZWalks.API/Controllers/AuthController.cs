using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> _userManager, ITokenRepository _tokenRepository)
        {
            userManager = _userManager;
            tokenRepository = _tokenRepository;
        }
        //POST: api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
           
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDTO.Password);

            if (identityResult.Succeeded)
            {
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {

                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("Registered succesfully!");
                    }
                }
            }
            return BadRequest("Something went wrong!");
        }

        //POST: api/auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user != null)
            {
                var loginResult = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (loginResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var token = tokenRepository.GenerateJSONWebToken(user, roles);
                   var responseToken = new LoginResponseDTO { JwtToken = token };
                    return Ok(responseToken);
                }
            }
            return BadRequest("Invalid login attempt");
           
        }
    }
}


