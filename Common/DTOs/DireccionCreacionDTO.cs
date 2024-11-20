using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class DireccionCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(300, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string DireccionEspecifica { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Ciudad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string CodigoPostal { get; set; } = string.Empty;

        public int UsuarioId { get; set; }
    }
}