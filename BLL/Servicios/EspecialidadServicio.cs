using AutoMapper;
using BLL.Servicios.Interfaces;
using Data;
using Data.Interfaces.IRepositorio;
using Models.DTOs;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Servicios
{
    public class EspecialidadServicio : IEspecialidadServicio
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly IMapper mapper;

        public EspecialidadServicio(IUnidadTrabajo unidadTrabajo, IMapper mapper)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.mapper = mapper;
        }
        public async Task Actualizar(EspecialidadDto modeloDto)
        {
            try
            {
                var especialidadBD = await unidadTrabajo.Especialidad.ObtenerPrimero(x => x.Id == modeloDto.Id);

                if (especialidadBD == null)
                {
                    throw new TaskCanceledException("La especialidad no existe");
                }
                especialidadBD.NombreEspecialidad = modeloDto.NombreEspecialidad;
                especialidadBD.Descripcion = modeloDto.Descripcion;
                especialidadBD.Estado = modeloDto.Estado == 1 ? true : false;
                await unidadTrabajo.Especialidad.Actualizar(especialidadBD);
                await unidadTrabajo.Guardar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<EspecialidadDto> Agregar(EspecialidadDto modeloDto)
        {
            try
            {
                Especialidad especialidad = new Especialidad
                {
                    NombreEspecialidad = modeloDto.NombreEspecialidad,
                    Descripcion = modeloDto.Descripcion,
                    Estado = modeloDto.Estado == 1 ? true : false,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                };
                await unidadTrabajo.Especialidad.Agregar(especialidad);
                await unidadTrabajo.Guardar();
                if (especialidad.Id == 0)
                {
                    throw new TaskCanceledException("La Especialidad no se Pudo Crear");
                }
                return mapper.Map<EspecialidadDto>(especialidad);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<EspecialidadDto>> ObtenerTodos()
        {
            try
            {
                var lista = await unidadTrabajo.Especialidad.ObtenerTodos();
                return mapper.Map<IEnumerable<EspecialidadDto>>(lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Remover(int id)
        {
            try
            {
                var especialidadBD = await unidadTrabajo.Especialidad.ObtenerPrimero(x => x.Id == id);

                if (especialidadBD == null)
                {
                    throw new TaskCanceledException("La especialidad no existe");
                }
                 unidadTrabajo.Especialidad.Remover(especialidadBD);
                await unidadTrabajo.Guardar();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
