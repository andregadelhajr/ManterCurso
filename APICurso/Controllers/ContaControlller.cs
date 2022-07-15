using System.Threading.Tasks;
using APICurso.Dtos;
using APICurso.Models.Identidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APICurso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaControlller : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        public ContaControlller(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            // if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            // if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = "this will be a token", 
                // _tokenService.CreateToken(user),
                NomeExibicao = user.NomeExibicao
            };
        }
    }
}