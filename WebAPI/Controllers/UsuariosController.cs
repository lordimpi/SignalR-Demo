using Business.Services.Contracts;
using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IOutputCacheStore _outputCacheStore;
        private readonly IHubContext<NotificationHub> _hubContext;
        private const string cacheTag = "Usuarios";

        public UsuariosController(IUsuarioService usuarioService, IOutputCacheStore outputCacheStore, IHubContext<NotificationHub> hubContext)
        {
            _usuarioService = usuarioService;
            _outputCacheStore = outputCacheStore;
            _hubContext = hubContext;
        }

        [HttpGet("obtenerUsuarios", Name = "ObtenerUsuarios")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("obtenerUsuario/{id}", Name = "ObtenerUsuario")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<UsuarioDTO>> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost("crearUsuario", Name = "CrearUsuario")]
        public async Task<ActionResult> CreateUsuarioAsync(UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var result = await _usuarioService.CreateUsuarioAsync(usuarioCreacionDTO);
            if (result == null)
            {
                return BadRequest();
            }

            await _outputCacheStore.EvictByTagAsync(cacheTag, default);
            await _hubContext.Clients.All.SendAsync("ReceiveUserCreation", "Un nuevo usuario ha sido creado.");
            return CreatedAtRoute("ObtenerUsuario", new { id = result.Id }, result);
        }

        [HttpPut("actualizarUsuario/{id}", Name = "ActualizarUsuario")]
        public async Task<ActionResult> UpdateUsuarioAsync(int id, UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var result = await _usuarioService.UpdateUsuarioAsync(id, usuarioCreacionDTO);
            if (!result)
            {
                return BadRequest();
            }
            await _outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        [HttpDelete("eliminarUsuario/{id}", Name = "EliminarUsuario")]
        public async Task<ActionResult> DeleteUsuarioAsync(int id)
        {
            var result = await _usuarioService.DeleteUsuarioAsync(id);
            if (!result)
            {
                return BadRequest();
            }
            await _outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
    }
}