using Microsoft.EntityFrameworkCore;
using PicPayDesafio.Data;
using PicPayDesafio.Entities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace PicPayDesafio.Repository
{
    public class UsuarioRepository(DataBase dataBase)
    {
        private readonly DataBase _dataBase = dataBase;

        public async Task<Usuario> Get(int id)
        {
            return await _dataBase.Set<Usuario>().Where(x => x.Id == id).SingleAsync();
        }

        public async Task<Usuario> Get(string email)
        {
            return await _dataBase.Set<Usuario>().Where(x => x.Email == email).SingleAsync();
        }

        public async Task<IList<Usuario>> Get()
        {
            return await _dataBase.Set<Usuario>().ToListAsync();
        }

        public async Task<bool> VerificarExistencia(Expression<Func<Usuario, bool>> filtro)
        {
            return await _dataBase.Set<Usuario>().AnyAsync(filtro);
        }

        public async Task Add(Usuario usuario)
        {
            await _dataBase.AddAsync(usuario);
            await _dataBase.SaveChangesAsync();
        }

        public async Task Update(Usuario usuario)
        {
            _dataBase.Update(usuario);
            await _dataBase.SaveChangesAsync();
        } 
    }
}
