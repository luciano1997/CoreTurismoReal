using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Context.Departamento;
using TurismoReal.Models;
using static TurismoReal.Models.General;

namespace TurismoReal.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly DepartamentoContext _departamentoContext;

        public DepartamentoController(DepartamentoContext departamentoContext)
        {
            _departamentoContext = departamentoContext;
        }

        [HttpGet("GetDepartamentos")]
        public ActionResult<List<DepartamentoViewModel>> GetDepartamentos()
        {
            var deptos = _departamentoContext.selectDeptos();

            if (deptos.Any())
            {
                return Ok(deptos);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles"});
            }
            
        }

        [HttpGet("GetDepartamentoById/{id}")]
        public ActionResult<List<DepartamentoViewModel>> GetDepartamentoById(int id)
        {
            var depto  = _departamentoContext.selectDeptoById(id);

            if (depto.id > 0)
            {
                return Ok(depto);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles" });
            }

        }


        [HttpPost("PostDepartamento")]
        public ActionResult PostDepartamento([FromBody] DepartamentoViewModel departamento)
        {
            var result = _departamentoContext.InsertDepartamento(departamento);
            
            if (result >0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new Retorno()
                {
                    Codigo = "er",
                    Mensaje = "error al insertar"
                }) ;
            }
            
            
        }
        [HttpPut("DepartamentoById")]
        public IActionResult PutDepartamentoById([FromBody] DepartamentoViewModel departamento)
        {
            var result = _departamentoContext.UpdateDepartamentoById(departamento);

            if (result > 0 )
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "No se pudo actualizar el registro" });
            }
        }

        [HttpDelete("DeleteDepartamentoById/{id}")]
        public IActionResult DeleteDepartamentoById(int id)
        {
            var result = _departamentoContext.DeleteDepartamento(id);

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
