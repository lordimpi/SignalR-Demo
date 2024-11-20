using Common.DTOs;

namespace Business.Services.Contracts
{
    public interface IDireccionService
    {
        Task<IEnumerable<DireccionDTO>> GetAllDireccionesAsync();

        Task<DireccionDTO?> GetDireccionByIdAsync(int id);

        Task<IEnumerable<DireccionDTO>> GetDireccionesByUserIdAsyn(int userId);

        Task<DireccionDTO> CreateDireccionAsync(DireccionCreacionDTO direccionCreacionDTO);

        Task<bool> UpdateDireccionAsync(int id, DireccionCreacionDTO direccionCreacionDTO);

        Task<bool> DeleteDireccionAsync(int id);
    }
}