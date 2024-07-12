using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PicPayDesafio.DTO;
using PicPayDesafio.Service;

namespace PicPayDesafio.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;
        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UsuarioRegisterDTO userDTO)
        {
            await _usuarioService.Add(userDTO);
            return Ok("Usuário Cadastrado com Sucesso!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
        {
        
            return Ok(await _usuarioService.Login(usuarioLogin));
        }

        [HttpGet("GetUsuario")]
        public async Task<UsuarioDTO> Get(int id)
        {
            return await _usuarioService.Get(id);
        }

        [HttpGet("GetUsuarios")]
        public async Task<IList<UsuarioDTO>> Get()
        {
            return await _usuarioService.Get();
        }


    }
}
