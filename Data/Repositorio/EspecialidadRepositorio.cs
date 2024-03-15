using Data.Interfaces.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositorio
{
    public class EspecialidadRepositorio : Repositorio<Especialidad>, IEspecialidadRepositorio
    {
        private readonly ApplicationDbContext db;

        public EspecialidadRepositorio(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task Actualizar(Especialidad especialidad)
        {
            var especialidadBD = await db.Especilidades.FirstOrDefaultAsync(x => x.Id == especialidad.Id);

            if (especialidadBD != null)
            {
                especialidadBD.NombreEspecialidad = especialidad.NombreEspecialidad;
                especialidadBD.Descripcion = especialidad.Descripcion;
                especialidadBD.Estado = especialidad.Estado;
                especialidadBD.FechaActualizacion = especialidad.FechaActualizacion;
            }
        }
    }
}
