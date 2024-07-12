using Microsoft.EntityFrameworkCore;
using PicPayDesafio.Entities;

namespace PicPayDesafio.Data
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions<DataBase> options) : base(options) 
        {

        }

        DbSet<Usuario> Usuario { get; set; }
        DbSet<Transicao> Transicao { get; set; }
    }
}
