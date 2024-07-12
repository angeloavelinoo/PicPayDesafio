namespace PicPayDesafio.Entities
{
    public class Transicao
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string Remetente { get; set; }
        public string Destinatario { get; set; }
        public DateTime DataDoEnvio { get; set; } = DateTime.UtcNow;
    }
}
