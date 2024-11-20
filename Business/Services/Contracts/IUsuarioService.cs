using Common.DTOs;

namespace Business.Services.Contracts
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync();

        Task<UsuarioDTO?> GetUsuarioByIdAsync(int id);

        Task<UsuarioDTO> CreateUsuarioAsync(UsuarioCreacionDTO usuarioCreacionDTO);

        Task<bool> UpdateUsuarioAsync(int id, UsuarioCreacionDTO usuarioCreacionDTO);

        Task<bool> DeleteUsuarioAsync(int id);
    }
}