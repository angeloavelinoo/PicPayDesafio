using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PicPayDesafio.Data;
using PicPayDesafio.Entities;
using PicPayDesafio.Repository;
using System.Text;

namespace PicPayDesafio.Service
{
    public class TransicaoService (UsuarioService usuarioService, DataBase context, UsuarioRepository usuarioRepository,HttpClient httpClient)
    {
        private readonly UsuarioService _usuarioService = usuarioService;
        private readonly UsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly DataBase _context = context;
        private readonly HttpClient _httpClient = httpClient;



        public async Task<string> SendMoney(decimal valorEnviado, string emailSender, string emailRecebedor)
        {
            using (var transicao = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Usuario remetente = await _usuarioRepository.Get(emailSender) ?? throw new Exception("Email do remetente inválido");
                    Usuario usuarioRecebedor = await _usuarioRepository.Get(emailRecebedor) ?? throw new Exception("Email do destinatário inválido");

                    await _usuarioService.ValidacaoDeTransicao(emailSender, valorEnviado);

                    bool autorizado = await AutorizarTransacao(remetente, valorEnviado);
                    if (!autorizado)
                        throw new Exception("Transação não autorizada");

                    remetente.Saldo -= valorEnviado;
                    usuarioRecebedor.Saldo += valorEnviado;

                    await _usuarioRepository.Update(remetente);
                    await _usuarioRepository.Update(usuarioRecebedor);

                    await transicao.CommitAsync();

                    return "Valor enviado com sucesso!";

                }
                catch (Exception ex)
                {
                    await transicao.RollbackAsync();
                    return ("Houve algum erro");
                }


            }

        }

        public async Task<bool> AutorizarTransacao(Usuario usuario, decimal valor)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://util.devi.tools/api/v2/authorize");


            var requestBody = new {usuario, valor};

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuthorizationResponse>(responseContent);

                if (result.Status == "success" && result.Data.Authorization == true)
                {
                    return true;
                }
            }

            return false;
        }

        public class AuthorizationResponse
        {
            public string Status { get; set; }
            public AuthorizationData Data { get; set; }
        }

        public class AuthorizationData
        {
            public bool Authorization { get; set; }
        }


    }
}
