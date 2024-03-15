using BLL.Servicios.Interfaces;
using Data.Interfaces.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using System.Net;

namespace API.Controllers
{
    [Route("api/especialidad")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadServicio especialidadServicio;
        private ApiResponse response;

        public EspecialidadesController(IEspecialidadServicio especialidadServicio)
        {
            this.especialidadServicio = especialidadServicio;
            response = new ApiResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                response.Resultado = await especialidadServicio.ObtenerTodos();
                response.IsExitoso = true;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.IsExitoso= false;
                response.Mensaje = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(EspecialidadDto especialidadDto)
        {
            try
            {
                await especialidadServicio.Agregar(especialidadDto);
                response.IsExitoso = true;
                response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                response.IsExitoso=! false;
                response.Mensaje = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(EspecialidadDto especialidad)
        {
            try
            {
                await especialidadServicio.Actualizar(especialidad);
                response.IsExitoso = true;
                response.StatusCode = HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.Mensaje = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await especialidadServicio.Remover(id);
                response.IsExitoso = true;
                response.StatusCode = HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.Mensaje = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return Ok(response);
        }
    }
}
