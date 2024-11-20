using Common.Entities;
using DataAccess.Data;
using DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class DireccionRepository : IDireccionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DireccionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Direccion> CreateDireccionAsync(Direccion direccion)
        {
            _dbContext.Direcciones.Add(direccion);
            await _dbContext.SaveChangesAsync();
            return direccion;
        }

        public async Task<bool> DeleteDireccionAsync(int id)
        {
            var registrosBorrados = await _dbContext.Direcciones.Where(d => d.Id == id).ExecuteDeleteAsync();
            return registrosBorrados > 0;
        }

        public async Task<IEnumerable<Direccion>> GetAllDireccionesAsync()
        {
            return await _dbContext.Direcciones.ToListAsync();
        }

        public Task<Direccion?> GetDireccionByIdAsync(int id)
        {
            var direccion = _dbContext.Direcciones.FirstOrDefaultAsync(d => d.Id == id);
            return direccion;
        }

        public async Task<IEnumerable<Direccion>> GetDireccionesByUsuarioIdAsync(int usuarioId)
        {
            var direcciones = await _dbContext.Direcciones
                .Where(d => d.UsuarioId == usuarioId)
                .ToListAsync();

            return direcciones;
        }

        public async Task<bool> UpdateDireccionAsync(int id, Direccion direccion)
        {
            var direccionExiste = await _dbContext.Direcciones.AnyAsync(d => d.Id == id);
            if (!direccionExiste)
            {
                return false;
            }

            direccion.Id = id;
            _dbContext.Direcciones.Update(direccion);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}