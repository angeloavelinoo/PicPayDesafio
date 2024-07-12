namespace PicPayDesafio.Entities
{
    public class Usuario
    {
        public Usuario( string nome, string cPF, string email, string senha, string role, decimal? saldo)
        {
            Nome = nome;
            CPF = cPF;
            Email = email;
            Senha = senha;
            Role = role;
            Saldo = saldo;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Role { get; set; }
        public decimal? Saldo { get; set; }
    }
}
