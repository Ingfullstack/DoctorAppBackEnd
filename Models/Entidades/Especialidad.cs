using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class Especialidad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre de la Especialidad es Requerida")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "El Nombre debe ser Minimo 1 Maximo 60 Caracteres")]
        public string NombreEspecialidad { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerida")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "La Descripcion debe ser Minimo 1 Maximo 100 Caracteres")]

        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Estado es Requerido")]
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
