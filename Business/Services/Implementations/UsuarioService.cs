using AutoMapper;
using Business.Services.Contracts;
using Common.DTOs;
using Common.Entities;
using DataAccess.Repositories.Contracts;

namespace Business.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO> CreateUsuarioAsync(UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var usuario = _mapper.Map<Usuario>(usuarioCreacionDTO);
            var usuarioDTO = _mapper.Map<UsuarioDTO>(await _usuarioRepository.CreateUsuarioAsync(usuario));
            return usuarioDTO;
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return false;
            }

            return await _usuarioRepository.DeleteUsuarioAsync(id);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
            return _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
        }

        public async Task<UsuarioDTO?> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
            return usuarioDTO;
        }

        public async Task<bool> UpdateUsuarioAsync(int id, UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return false;
            }

            usuario = _mapper.Map(usuarioCreacionDTO, usuario);

            return await _usuarioRepository.UpdateUsuarioAsync(id, usuario);
        }
    }
}