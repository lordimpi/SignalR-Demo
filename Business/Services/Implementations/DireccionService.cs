using AutoMapper;
using Business.Services.Contracts;
using Common.DTOs;
using Common.Entities;
using DataAccess.Repositories.Contracts;

namespace Business.Services.Implementations
{
    public class DireccionService : IDireccionService
    {
        private readonly IDireccionRepository _direccionRepository;
        private readonly IMapper _mapper;

        public DireccionService(IDireccionRepository direccionRepository, IMapper mapper)
        {
            _direccionRepository = direccionRepository;
            _mapper = mapper;
        }

        public async Task<DireccionDTO> CreateDireccionAsync(DireccionCreacionDTO direccionCreacionDTO)
        {
            var direccion = _mapper.Map<Direccion>(direccionCreacionDTO);
            var direccionCreada = await _direccionRepository.CreateDireccionAsync(direccion);
            return _mapper.Map<DireccionDTO>(direccionCreada);
        }

        public async Task<bool> DeleteDireccionAsync(int id)
        {
            var direccion = await _direccionRepository.GetDireccionByIdAsync(id);
            if (direccion == null)
            {
                return false;
            }

            return await _direccionRepository.DeleteDireccionAsync(id);
        }

        public async Task<IEnumerable<DireccionDTO>> GetAllDireccionesAsync()
        {
            var direcciones = await _direccionRepository.GetAllDireccionesAsync();
            return _mapper.Map<IEnumerable<DireccionDTO>>(direcciones);
        }

        public async Task<DireccionDTO?> GetDireccionByIdAsync(int id)
        {
            var direccion = await _direccionRepository.GetDireccionByIdAsync(id);
            var direccionDTO = _mapper.Map<DireccionDTO>(direccion);
            return direccionDTO;
        }

        public async Task<IEnumerable<DireccionDTO>> GetDireccionesByUserIdAsyn(int userId)
        {
            var direcciones = await _direccionRepository.GetDireccionesByUsuarioIdAsync(userId);
            return _mapper.Map<IEnumerable<DireccionDTO>>(direcciones);
        }

        public async Task<bool> UpdateDireccionAsync(int id, DireccionCreacionDTO direccionCreacionDTO)
        {
            var direccion = await _direccionRepository.GetDireccionByIdAsync(id);
            if (direccion == null)
            {
                return false;
            }

            direccion = _mapper.Map(direccionCreacionDTO, direccion);
            return await _direccionRepository.UpdateDireccionAsync(id, direccion);
        }
    }
}