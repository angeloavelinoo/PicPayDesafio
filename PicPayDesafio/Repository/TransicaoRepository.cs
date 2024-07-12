using Microsoft.EntityFrameworkCore;
using PicPayDesafio.Data;
using PicPayDesafio.Entities;

namespace PicPayDesafio.Repository
{
    public class TransicaoRepository(DataBase context)
    {
        private readonly DataBase _context = context;


        public async Task<List<Transicao>> GetTransicoesEnviadasByUser(string email)
        {
            return await _context.Set<Transicao>().Where(x => x.Remetente == email).ToListAsync();
        }


        public async Task<List<Transicao>> GetAllTransicoes()
        {
            return await _context.Set<Transicao>().ToListAsync();
        }

        public async Task Add(Transicao transicao)
        {
           await _context.AddAsync(transicao);
           await _context.SaveChangesAsync();
        }


    }
}
