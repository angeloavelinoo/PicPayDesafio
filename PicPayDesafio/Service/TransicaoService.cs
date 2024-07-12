using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PicPayDesafio.Data;
using PicPayDesafio.DTO;
using PicPayDesafio.Entities;
using PicPayDesafio.Repository;
using System.Text;

namespace PicPayDesafio.Service
{
    public class TransicaoService(UsuarioService usuarioService, DataBase context, UsuarioRepository usuarioRepository, HttpClient httpClient, TransicaoRepository transicaoRepository)
    {
        private readonly UsuarioService _usuarioService = usuarioService;
        private readonly UsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly TransicaoRepository _transicaoRepository = transicaoRepository;
        private readonly DataBase _context = context;
        private readonly HttpClient _httpClient = httpClient;


        public async Task<IList<Transicao>> GetAll()
        {
            List<Transicao> trancicoes = await _transicaoRepository.GetAllTransicoes();

            if (trancicoes == null)
                throw new Exception("Nenhuma transição foi encontrada");


            return trancicoes;
        }

        public async Task<IList<Transicao>> GetAllByUser(string email)
        {
            List<Transicao> trancicoes = await _transicaoRepository.GetTransicoesEnviadasByUser(email);

            if (trancicoes == null)
                throw new Exception("Nenhuma transição foi encontrada");


            return trancicoes;
        }

        public async Task<string> SendMoney(decimal valorEnviado, string emailSender, string emailRecebedor)
        {
            try
            {
                Usuario remetente = await _usuarioRepository.Get(emailSender) ?? throw new Exception("Email do remetente inválido");
                Usuario usuarioRecebedor = await _usuarioRepository.Get(emailRecebedor) ?? throw new Exception("Email do destinatário inválido");

                await _usuarioService.ValidacaoDeTransicao(emailSender, valorEnviado);

                //bool autorizado = await AutorizarTransacao(remetente, valorEnviado);
                //if (!autorizado)
                //    throw new Exception("Transação não autorizada");

                remetente.Saldo -= valorEnviado;
                usuarioRecebedor.Saldo += valorEnviado;

                await _usuarioRepository.Update(remetente);
                await _usuarioRepository.Update(usuarioRecebedor);

                Transicao trancicao = new Transicao(valor: valorEnviado, remetente: emailSender, destinatario: emailRecebedor);

                await _transicaoRepository.Add(trancicao);

                return "Valor enviado com sucesso!";

            }
            catch (Exception ex)
            {
                return ("Houve algum erro");
            }

        }

        public async Task<bool> AutorizarTransacao(Usuario usuario, decimal valor)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://util.devi.tools/api/v2/authorize");


            var requestBody = new { usuario, valor };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
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
