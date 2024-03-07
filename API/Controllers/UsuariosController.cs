using Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entidades;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly ITokenServicio tokenServicio;

        public UsuariosController(ApplicationDbContext db, ITokenServicio tokenServicio)
        {
            this.db = db;
            this.tokenServicio = tokenServicio;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await db.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await db.Usuarios.FindAsync(id);

            if (usuario is null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost("Registro")]
        public async Task<ActionResult<UsuarioDto>> Registro(RegistroDto registroDto)
        {
            if (await UsuarioExiste(registroDto.UserName)) //Validar si existe el usuario
            {
                return BadRequest("UserName ya esta Registrado");
            }

            using var hmac = new HMACSHA512();
            var usuario = new Usuario
            {
                UserName = registroDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registroDto.Password)),
                PasswordSalt = hmac.Key
            };
            await db.Usuarios.AddAsync(usuario);
            await db.SaveChangesAsync();
            return new UsuarioDto
            {
                Username = usuario.UserName,
                Token = tokenServicio.CrearToken(usuario)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await db.Usuarios.SingleOrDefaultAsync(u => u.UserName.ToLower().Trim() == loginDto.Username.ToLower().Trim());
            if (usuario is null) return Unauthorized("Usuario no Valido");
            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != usuario.PasswordHash[i]) return Unauthorized("Password no Valido");
            }
            return new UsuarioDto
            {
                Username = usuario.UserName,
                Token = tokenServicio.CrearToken(usuario)
            };
        }

        private async Task<bool> UsuarioExiste(string username)
        {
            return await db.Usuarios.AnyAsync(u => u.UserName.ToLower().Trim() == username.ToLower().Trim());
        }
    }
}
