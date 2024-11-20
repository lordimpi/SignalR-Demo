using Business.Services.Contracts;
using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace WebAPI.Controllers
{
    [Route("api/direcciones")]
    [ApiController]
    public class DireccionesController : ControllerBase
    {
        private readonly IDireccionService _direccionService;
        private readonly IOutputCacheStore _outputCacheStore;
        private const string cacheTag = "Direcciones";
        private const string cacheTagU = "Usuarios";

        public DireccionesController(IDireccionService direccionService, IOutputCacheStore outputCacheStore)
        {
            _direccionService = direccionService;
            _outputCacheStore = outputCacheStore;
        }

        [HttpGet("obtenerDirecciones", Name = "ObtenerDirecciones")]
        [OutputCache(Tags = [cacheTag, cacheTagU])]
        public async Task<ActionResult<IEnumerable<DireccionDTO>>> GetAllDireccionesAsync()
        {
            var direcciones = await _direccionService.GetAllDireccionesAsync();
            return Ok(direcciones);
        }

        [HttpGet("obtenerDireccion/{id}", Name = "ObtenerDireccion")]
        [OutputCache(Tags = [cacheTag, cacheTagU])]
        public async Task<ActionResult<DireccionDTO>> GetDireccionByIdAsync(int id)
        {
            var direccion = await _direccionService.GetDireccionByIdAsync(id);
            if (direccion == null)
            {
                return NotFound();
            }

            return Ok(direccion);
        }

        [HttpGet("obtenerDireccionesPorUsuario/{userId}", Name = "ObtenerDireccionesPorUsuario")]
        [OutputCache(Tags = [cacheTag, cacheTagU])]
        public async Task<ActionResult<IEnumerable<DireccionDTO>>> GetDireccionesByUserIdAsync(int userId)
        {
            var direcciones = await _direccionService.GetDireccionesByUserIdAsyn(userId);
            if (direcciones == null)
            {
                return NotFound();
            }

            return Ok(direcciones);
        }

        [HttpPost("crearDireccion", Name = "CrearDireccion")]
        public async Task<ActionResult> CreateDireccionAsync(DireccionCreacionDTO direccionCreacionDTO)
        {
            var result = await _direccionService.CreateDireccionAsync(direccionCreacionDTO);
            if (result == null)
            {
                return BadRequest();
            }

            await ClearCacheAsync([cacheTag, cacheTagU]);

            return CreatedAtRoute("ObtenerDireccion", new { id = result.Id }, result);
        }

        [HttpPut("actualizarDireccion/{id}", Name = "ActualizarDireccion")]
        public async Task<ActionResult> UpdateDireccionAsync(int id, DireccionCreacionDTO direccionCreacionDTO)
        {
            var result = await _direccionService.UpdateDireccionAsync(id, direccionCreacionDTO);
            if (!result)
            {
                return BadRequest();
            }

            await ClearCacheAsync([cacheTag, cacheTagU]);
            return NoContent();
        }

        [HttpDelete("eliminarDireccion/{id}", Name = "EliminarDireccion")]
        public async Task<ActionResult> DeleteDireccionAsync(int id)
        {
            var result = await _direccionService.DeleteDireccionAsync(id);
            if (!result)
            {
                return BadRequest();
            }

            await ClearCacheAsync([cacheTag, cacheTagU]);
            return NoContent();
        }

        private async Task ClearCacheAsync(params string[] tags)
        {
            if (tags == null || tags.Length == 0) return;

            var tasks = tags.Select(tag => _outputCacheStore.EvictByTagAsync(tag, default).AsTask());
            await Task.WhenAll(tasks);
        }
    }
}