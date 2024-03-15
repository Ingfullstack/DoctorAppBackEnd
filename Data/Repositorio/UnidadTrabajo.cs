using Data.Interfaces.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext db;

        public IEspecialidadRepositorio Especialidad { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            this.db = db;
            Especialidad = new EspecialidadRepositorio(db);
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task Guardar()
        {
           await db.SaveChangesAsync();
        }
    }
}
