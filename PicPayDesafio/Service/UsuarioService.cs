using PicPayDesafio.Data;
using PicPayDesafio.DTO;
using PicPayDesafio.Entities;
using PicPayDesafio.Repository;

namespace PicPayDesafio.Service
{
    public class UsuarioService(UsuarioRepository usuarioRepository, DataBase dataBase)
    {
        private readonly UsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly DataBase _context = dataBase;

        public async Task Add(UsuarioRegisterDTO usuarioDTO)
        {
            Usuario usuario = new Usuario(nome: usuarioDTO.FirstName + " " + usuarioDTO.LastName, cPF: usuarioDTO.CPF, email: usuarioDTO.Email, senha: usuarioDTO.Senha, role: usuarioDTO.Role, saldo: usuarioDTO.Saldo);

            if (usuario == null)
                throw new ArgumentNullException(nameof(usuarioDTO));

            if (await _usuarioRepository.VerificarExistencia(x => x.Email == usuarioDTO.Email) || await _usuarioRepository.VerificarExistencia(x => x.CPF == usuarioDTO.CPF))
                throw new InvalidOperationException("Usuário com este e-mail ou CPF já existe.");

            await _usuarioRepository.Add(usuario);
        }

        public async Task<string> Login(UsuarioLogin usuarioDto)
        {
            string token;
            Usuario usuario = await _usuarioRepository.Get(usuarioDto.Email);

            if (usuario?.Senha != null && usuario?.Senha == usuarioDto.Senha)
                return token = TokenService.GenerateToken(usuario);

            return ("Usuario ou senha inválidos");
        }

        public async Task<UsuarioDTO> Get(int id)
        {
            Usuario usuario = await _usuarioRepository.Get(id);

            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            UsuarioDTO usuarioDTO = new UsuarioDTO(usuario);

            return usuarioDTO;
        }


        public async Task<IList<UsuarioDTO>> Get()
        {
            IList<Usuario> usuarios = await _usuarioRepository.Get();

            if (usuarios == null)
                throw new ArgumentNullException(nameof(usuarios));

            List<UsuarioDTO> usuariosDTO = new List<UsuarioDTO>();

            foreach (var usuario in usuarios)
            {
                UsuarioDTO usuarioDTO = new UsuarioDTO(usuario);

                usuariosDTO.Add(usuarioDTO);

            }

            return usuariosDTO;
        }

       public async Task ValidacaoDeTransicao(string emailSender, decimal valor )
        {
            Usuario remetente = await _usuarioRepository.Get(emailSender);

            if (valor > remetente.Saldo || valor < 0)
                throw new Exception("O saldo é invalido");
            
            if(remetente.Role != "UsuarioComum")
                throw new Exception("Você não tem permissão para fazer transações");

        }

        

    }
}
