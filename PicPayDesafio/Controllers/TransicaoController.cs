using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PicPayDesafio.DTO;
using PicPayDesafio.Entities;
using PicPayDesafio.Service;

namespace PicPayDesafio.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransicaoController : Controller
    {
        private readonly TransicaoService _transicaoService;
        public TransicaoController(TransicaoService transicaoService)
        {
            _transicaoService = transicaoService;
        }
        [HttpPost("EnviarDinheiro")]
        public async Task<IActionResult> EnviarDinheiro(decimal dinheiro, string destinatario)
        {
            return Ok(await _transicaoService.SendMoney(dinheiro, User?.Identity?.Name, destinatario));


        }

        [HttpGet("TodasTransicoes")]
        
        public async Task<IList<Transicao>> Get()
        {
            return await _transicaoService.GetAll();
        }

        [HttpGet("TrancicoesEnviadaPorUsuario")]

        public async Task<IList<Transicao>> GetByUser(string email)
        {
            return await _transicaoService.GetAllByUser(email);
        }
    }
}
