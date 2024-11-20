using Business.Services.Contracts;
using Business.Services.Implementations;
using Common.DTOs;
using Front.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Front.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public UsuariosController(IHttpService httpService, IHubContext<NotificationHub> hubContext)
        {
            _httpService = httpService;
            this._hubContext = hubContext;
        }

        #region Usuarios

        [HttpGet]
        public async Task<IActionResult> GetUsuariosJson()
        {
            var data = await _httpService.GetAsync<List<UsuarioDTO>>("https://localhost:7263/api/usuarios/obtenerUsuarios");
            return Json(data);
        }

        public async Task<IActionResult> Index()
        {
            var data = await _httpService.GetAsync<List<UsuarioDTO>>("https://localhost:7263/api/usuarios/obtenerUsuarios");
            return View(data);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _httpService.GetAsync<UsuarioDTO>($"https://localhost:7263/api/usuarios/obtenerUsuario/{id}");

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(UsuarioCreacionDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model == null)
                    {
                        return NotFound();
                    }

                    var response = await _httpService.PostAsync<UsuarioCreacionDTO, UsuarioCreacionDTO>("https://localhost:7263/api/usuarios/crearUsuario", model);
                    await _hubContext.Clients.All.SendAsync("ReceiveUserCreation", "Un nuevo usuario ha sido creado.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }
            }

            return View();
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _httpService.GetAsync<UsuarioDTO>($"https://localhost:7263/api/usuarios/obtenerUsuario/{id}");

            if (data == null)
            {
                return NotFound();
            }

            var usuario = new UsuarioCreacionDTO
            {
                Nombre = data.Nombre,
                Email = data.Email,
                Password = data.Password,
            };

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int? id, UsuarioCreacionDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var response = await _httpService.PutAsync<UsuarioCreacionDTO>($"https://localhost:7263/api/usuarios/actualizarUsuario/{id}", model);
                    await _hubContext.Clients.All.SendAsync("ReceiveUserCreation", "Un usuario ha sido modificado.");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }
            }

            return View();
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _httpService.DeleteAsync($"https://localhost:7263/api/usuarios/eliminarUsuario/{id}");
            await _hubContext.Clients.All.SendAsync("ReceiveUserCreation", "Un usuario ha sido eliminado.");

            return RedirectToAction(nameof(Index));
        }

        #endregion Usuarios

        #region Direcciones

        public async Task<IActionResult> getAddressJson(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _httpService.GetAsync<List<DireccionDTO>>($"https://localhost:7263/api/direcciones/obtenerDireccionesPorUsuario/{id}");

            return Json(data);
        }

        public IActionResult CrearDireccion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new DireccionCreacionDTO
            {
                UsuarioId = (int)id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearDireccion(DireccionCreacionDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model == null)
                    {
                        return NotFound();
                    }

                    var response = await _httpService.PostAsync<DireccionCreacionDTO, DireccionCreacionDTO>("https://localhost:7263/api/direcciones/crearDireccion", model);
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", model.UsuarioId, "Una nueva direccion ha sido creada.");

                    return RegresarDetalles(model.UsuarioId);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> EditarDireccion(int? id, int? usuarioId)
        {
            if (id == null || usuarioId == null)
            {
                return NotFound();
            }

            var data = await _httpService.GetAsync<DireccionDTO>($"https://localhost:7263/api/direcciones/obtenerDireccion/{id}");

            if (data == null)
            {
                return NotFound();
            }

            var model = new EditDireccionViewModel
            {
                Id = data.Id,
                Ciudad = data.Ciudad,
                CodigoPostal = data.CodigoPostal,
                DireccionEspecifica = data.DireccionEspecifica,
                UsuarioId = (int)usuarioId,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarDireccion(int? id, EditDireccionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var modelAPI = new DireccionCreacionDTO
                    {
                        Ciudad = model.Ciudad,
                        CodigoPostal = model.CodigoPostal,
                        DireccionEspecifica = model.DireccionEspecifica,
                        UsuarioId = model.UsuarioId,
                    };

                    var response = await _httpService.PutAsync<DireccionCreacionDTO>($"https://localhost:7263/api/direcciones/actualizarDireccion/{id}", modelAPI);
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", model.UsuarioId, "Una direccion ha sido modificada.");

                    return RegresarDetalles(model.UsuarioId);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }
            }

            return View();
        }

        public async Task<IActionResult> EliminarDireccion(int? id, int? usuarioId)
        {
            if (id == null || usuarioId == null)
            {
                return NotFound();
            }

            var data = await _httpService.DeleteAsync($"https://localhost:7263/api/direcciones/eliminarDireccion/{id}");
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", usuarioId, "Una direccion ha sido eliminada.");

            return RegresarDetalles(usuarioId);
        }

        public ActionResult RegresarDetalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Detalles), new { id });
        }

        #endregion Direcciones
    }
}