using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class UsuarioCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 5)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El campo {0} no es una dirección de correo válida.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MinLength(6, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
}