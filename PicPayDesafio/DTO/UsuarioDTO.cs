using PicPayDesafio.Entities;

namespace PicPayDesafio.DTO
{
    public class UsuarioDTO
    {
        public UsuarioDTO(Usuario usuario)
        {
            Id = usuario.Id;
            Nome = usuario.Nome;
            Saldo  = usuario.Saldo;
            Role = usuario.Role;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal? Saldo { get; set; }
        public string Role { get; set; }
    }
}
