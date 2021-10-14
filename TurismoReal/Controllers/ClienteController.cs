using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Context.Cliente;
using TurismoReal.Models;

namespace TurismoReal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : Controller

    {
        private readonly ClienteContext _clienteContext;

        public ClienteController(ClienteContext clienteContext)
        {
            _clienteContext = clienteContext;
        }

        [HttpGet("GetClientes")]
        public ActionResult<List<ClienteViewModel>> GetClientes()
        {
            var cliente = _clienteContext.selectAllClientes();

            if (cliente.Any())
            {
                return Ok(cliente);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles" });
            }

        }

        [HttpGet("GetClienteById/{id}")]
        public ActionResult<List<ClienteViewModel>> GetClienteById(int id)
        {
            var cliente = _clienteContext.selectClienteById(id);

            if (cliente.id > 0)
            {
                return Ok(cliente);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles" });
            }

        }

        [HttpPost("PostCliente")]
        public ActionResult PostCliente([FromBody] ClienteViewModel cliente)
        {
            var result = _clienteContext.InsertCliente(cliente);

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

        [HttpDelete("DeleteClienteById/{id}")]
        public IActionResult DeleteClienteById(int id)
        {
            var result = _clienteContext.DeleteClienteById(id);

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
