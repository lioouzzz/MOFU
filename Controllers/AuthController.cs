using Microsoft.AspNetCore.Mvc;
using MOFU.Dto;
using MOFU.Interfaces;

namespace MOFU.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        public readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto createUser)
        {
            var result = await _authService.Register(createUser);

            if (result == null)
            {
                return BadRequest("欄位不可為空");
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);

            if (result == null)
            {
                return BadRequest("帳號或密碼錯誤");
            }
            return Ok(result);
        }
    }
}
