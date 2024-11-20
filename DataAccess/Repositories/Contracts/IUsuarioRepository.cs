using Common.Entities;

namespace DataAccess.Repositories.Contracts
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();

        Task<Usuario?> GetUsuarioByIdAsync(int id);

        Task<Usuario> CreateUsuarioAsync(Usuario usuario);

        Task<bool> UpdateUsuarioAsync(int id, Usuario usuario);

        Task<bool> DeleteUsuarioAsync(int id);
    }
}