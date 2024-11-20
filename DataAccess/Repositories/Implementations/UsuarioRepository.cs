using AutoMapper;
using Common.Entities;
using DataAccess.Data;
using DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UsuarioRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            _dbContext.Usuarios.Add(usuario);
            await _dbContext.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var registrosBorrados = await _dbContext.Usuarios.Where(u => u.Id == id).ExecuteDeleteAsync();
            return registrosBorrados > 0;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _dbContext.Usuarios
                .Include(u => u.Direcciones)
                .ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _dbContext.Usuarios
                .Include(u => u.Direcciones)
                .FirstOrDefaultAsync(u => u.Id == id);
            return usuario;
        }

        public async Task<bool> UpdateUsuarioAsync(int id, Usuario usuario)
        {
            var usuarioExiste = await _dbContext.Usuarios.AnyAsync(u => u.Id == id);
            if (!usuarioExiste)
            {
                return false;
            }

            usuario.Id = id;
            _dbContext.Usuarios.Update(usuario);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}