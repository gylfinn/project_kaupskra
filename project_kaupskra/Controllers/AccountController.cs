using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project_kaupskra.DTOs;
using project_kaupskra.Interfaces;
using project_kaupskra.Models;

namespace project_kaupskra.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if(createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded)
                    {                         
                        return Ok(
                            new NewUserDto
                            {
                                Username = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                            );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }

                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            //if (await UserExists(registerDTO.Username)) return BadRequest("Username is taken");

            //var user = new AppUser
            //{
            //    UserName = registerDTO.Username
            //};

            //var result = await _userManager.CreateAsync(user, registerDTO.Password);

            //if (!result.Succeeded) return BadRequest(result.Errors);

            //return new UserDTO
            //{
            //    Username = user.UserName,
            //    Token = "token"
            //};
        }
    }
}
