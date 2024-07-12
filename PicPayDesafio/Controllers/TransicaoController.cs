using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    }
}
