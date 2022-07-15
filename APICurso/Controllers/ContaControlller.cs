using System.Threading.Tasks;
using APICurso.Dtos;
using APICurso.Interface;
using APICurso.Models.Identidade;
using APICurso.Service;
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
        private readonly ITokenService _tokenService;
        public ContaControlller(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService)
        {
            _tokenService = tokenService;
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
                Token = _tokenService.CreateToken(user),
                NomeExibicao = user.NomeExibicao
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            // {
            //     return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new[] {"Este endereço de e-mail já está em uso"}});
            // }

            var user = new Usuario
            {
                NomeExibicao = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            // if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                NomeExibicao = user.NomeExibicao,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }
    }
}