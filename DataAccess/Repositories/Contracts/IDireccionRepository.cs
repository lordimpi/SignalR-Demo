using Common.Entities;

namespace DataAccess.Repositories.Contracts
{
    public interface IDireccionRepository
    {
        Task<IEnumerable<Direccion>> GetAllDireccionesAsync();

        Task<Direccion?> GetDireccionByIdAsync(int id);

        Task<IEnumerable<Direccion>> GetDireccionesByUsuarioIdAsync(int usuarioId);

        Task<Direccion> CreateDireccionAsync(Direccion direccion);

        Task<bool> UpdateDireccionAsync(int id, Direccion direccion);

        Task<bool> DeleteDireccionAsync(int id);
    }
}