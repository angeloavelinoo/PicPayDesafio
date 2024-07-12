namespace PicPayDesafio.Entities
{
    public class Transicao
    {
        public Transicao(decimal valor, string remetente, string destinatario)
        {
            Valor = valor;
            Remetente = remetente;
            Destinatario = destinatario;
        }

        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string Remetente { get; set; }
        public string Destinatario { get; set; }
        public DateTime DataDoEnvio { get; set; } = DateTime.UtcNow;
    }
}
