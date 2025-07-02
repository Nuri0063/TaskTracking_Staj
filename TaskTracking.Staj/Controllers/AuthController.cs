using Microsoft.AspNetCore.Mvc;
using TaskTracking.Staj.Dtos;
using TaskTracking.Staj.Interfaces;
using TaskTracking.Staj.Models;

namespace TaskTracking.Staj.Controllers
{
    [ApiController] 
    [Route("[controller]")] 
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            if (await _authService.UserExists(dto.UserName))
                return BadRequest("Kullanıcı zaten var");

            var user = new User { UserName = dto.UserName };
            await _authService.Register(user, dto.Password);
            return Ok("Kayıt başarılı");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto dto)
        {
            var token = await _authService.Login(dto.UserName, dto.Password);
            if (token == null) return Unauthorized("Kullanıcı adı veya şifre hatalı");

            return Ok(token);
        }
    }
}
