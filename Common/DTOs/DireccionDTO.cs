using Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class DireccionDTO
    {
        public int Id { get; set; }

        public string DireccionEspecifica { get; set; } = string.Empty;

        public string Ciudad { get; set; } = string.Empty;

        public string CodigoPostal { get; set; } = string.Empty;
    }
}