using AutoMapper;
using Common.DTOs;
using Common.Entities;

namespace Common.Profiles
{
    /// <summary>
    /// Clase para configurar los mapeos de AutoMapper.
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            ConfigurarMapeoUsuarios();
            ConfigurarMapeoDirecciones();
        }

        private void ConfigurarMapeoDirecciones()
        {
            CreateMap<Direccion, DireccionDTO>().ReverseMap();
            CreateMap<DireccionCreacionDTO, Direccion>().ReverseMap();
        }

        private void ConfigurarMapeoUsuarios()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<UsuarioCreacionDTO, Usuario>().ReverseMap();
        }
    }
}