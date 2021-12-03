using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Context.Departamento;
using TurismoReal.Context.Direccion;
using TurismoReal.Models;
using static TurismoReal.Models.General;

namespace TurismoReal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DepartamentoController : ControllerBase
    {
        private readonly DepartamentoContext _departamentoContext;
        private readonly DireccionContext _direccionContext;

        public DepartamentoController(DepartamentoContext departamentoContext, DireccionContext direccionContext)
        {
            _departamentoContext = departamentoContext;
            _direccionContext = direccionContext;
        }
        
        [HttpGet("GetDepartamentos")]
        public ActionResult<List<DepartamentoViewModel>> GetDepartamentos()
        {
            IList<DepartamentoViewModel> deptos = new List<DepartamentoViewModel>();
            deptos = _departamentoContext.selectDeptos();

            foreach (var item in deptos)
            {
                item.imagenes = _departamentoContext.selectDepartamentoImagenesById(item.id);
                item.direccion = _direccionContext.selectDireccionById(item.id);
            }

            if (deptos.Any())
            {
                return Ok(deptos);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles"});
            }
            
        }

        [HttpGet("GetDepartamentosDisponibles")]
        public ActionResult<List<DepartamentoViewModel>> GetDepartamentosDisponibles()
        {
            IList<DepartamentoViewModel> deptos = new List<DepartamentoViewModel>();
            deptos = _departamentoContext.selectDeptosDisponibles();

            foreach (var item in deptos)
            {
                item.imagenes = _departamentoContext.selectDepartamentoImagenesById(item.id);
                item.direccion = _direccionContext.selectDireccionById(item.id);
            }

            if (deptos.Any())
            {
                return Ok(deptos);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles" });
            }

        }

        [HttpGet("GetDepartamentoById/{id}")]
        public ActionResult<List<DepartamentoViewModel>> GetDepartamentoById(int id)
        {
            DepartamentoViewModel depto = new DepartamentoViewModel();
            
            depto  = _departamentoContext.selectDeptoById(id);

            depto.imagenes = _departamentoContext.selectDepartamentoImagenesById(id);
            depto.direccion = _direccionContext.selectDireccionById(id);

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
            var resultDireccion = _direccionContext.InsertDireccion(departamento);
            if (departamento.imagenes.Any())
            {
                foreach (var item in departamento.imagenes)
                {
                   _departamentoContext.InsertDepartamentoImagenesById(departamento.DepartamentoId, item);
                   
                }
            }
            
           if (result >0 && resultDireccion > 0)
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

        [HttpPost("PostDepartamentoImagen")]
        public ActionResult PostDepartamentoImagen([FromBody] DepartamentoImagen departamento)
        {
            var result = _departamentoContext.InsertDepartamentoImagenesById(departamento.idDepartamento, departamento.imagenesUrl);
            

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new Retorno()
                {
                    Codigo = "er",
                    Mensaje = "error al insertar"
                });
            }


        }
        [HttpPut("PutDepartamentoById")]
        public IActionResult PutDepartamentoById([FromBody] DepartamentoViewModel departamento)
        {
            var result = _departamentoContext.UpdateDepartamentoById(departamento);
            var resultDireccion = _direccionContext.UpdateDireccionDepartamentoById(departamento);

            if (result > 0 && resultDireccion > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "No se pudo actualizar el registro" });
            }
        }
        [HttpPut("PutDepartamentoEstadoById")]
        public IActionResult PutDepartamentoEstadoById([FromBody] DepartamentoViewModel departamento)
        {
            var result = _departamentoContext.UpdateDepartamentoEstadoById(departamento.id, departamento.estado);
           

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
            var result = _departamentoContext.DeleteDepartamentoById(id);

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
