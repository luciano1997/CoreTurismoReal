using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Context.Usuario;
using TurismoReal.Models;

namespace TurismoReal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller

    {
        private readonly UsuarioContext _usuarioContext;

        public UsuarioController(UsuarioContext usuarioContext)
        {
            _usuarioContext = usuarioContext;
        }

        [HttpGet("GetUsuarios")]
        public ActionResult<List<UsuarioViewModel>> GetUsuarios()
        {
            var usuario = _usuarioContext.selectAllUsuarios();

            if (usuario.Any())
            {
                return Ok(usuario);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles" });
            }

        }
        [HttpGet("ValidarUsuario/{correo}/{password}")]
        public int ValidarUsuario(string correo, string password)
        {
            var usuario = _usuarioContext.ValidarUsuario(correo, password);

            if (usuario > 0)
            {
                return usuario;
            }
            else
            {
                return 0;
            }

        }


        [HttpGet("GetUsuarioById/{id}")]
        public ActionResult<List<UsuarioViewModel>> GetUsuarioById(int id)
        {
            var usuario = _usuarioContext.selectUsuarioById(id);

            if (usuario.id > 0)
            {
                return Ok(usuario);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles" });
            }

        }

        [HttpPost("PostUsuario")]
        public ActionResult PostUsuario([FromBody] UsuarioViewModel usuario)
        {
            var result = _usuarioContext.InsertUsuario(usuario);

            if (result > 0)
            {
                //_usuarioContext.EnviarConfirmacionUsuario();
                return Ok(result);

            }
            else
            {
                return BadRequest(new General.Retorno()
                {
                    Codigo = "er",
                    Mensaje = "error al insertar"
                });
            }


        }

        [HttpPut("PutUsuario")]
        public ActionResult PutUsuario([FromBody] UsuarioViewModel usuario)
        {
            var result = _usuarioContext.UpdateUsuarioById(usuario);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new General.Retorno()
                {
                    Codigo = "er",
                    Mensaje = "error al insertar"
                });
            }


        }
        [HttpPut("PutUsuarioEstadoById")]
        public ActionResult PutUsuarioEstadoById([FromBody] UsuarioViewModel usuario)
        {
            var result = _usuarioContext.UpdateUsuarioEstadoById(usuario);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new General.Retorno()
                {
                    Codigo = "er",
                    Mensaje = "error al insertar"
                });
            }


        }



        [HttpDelete("DeleteUsuarioByCorreo/{correo}")]
        public IActionResult DeleteUsuarioByCorreo(string correo)
        {
            var result = _usuarioContext.DeleteUsuarioByCorreo(correo);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "error al eliminar el registro" });
            }

        }


    }
}
