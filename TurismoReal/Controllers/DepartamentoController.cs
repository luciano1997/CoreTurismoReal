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

        [HttpGet]
        public ActionResult<List<DepartamentoViewModel>> GetDepartamentos()
        {

            var depto = _departamentoContext.selectDeptos();

            if (depto.id > 0)
            {
                return Ok(depto);
            }
            else
            {
                return BadRequest(new General.Retorno() { codigo = "er", mensaje = "no hay departamentos disponibles"});
            }
            
        }
    
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<DepartamentoViewModel>> GetDepartamento(int id)
        //{
        //    throw new NotImplementedException();
          
            
        //}
    
        [HttpPost]
        public ActionResult Post([FromBody] DepartamentoViewModel departamento)
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
                    codigo = "er",
                    mensaje = "error al insertar"
                });
            }
            
            
        }
        [HttpPut]
        public ActionResult Put([FromBody] DepartamentoViewModel departamento)
        {
            throw new NotImplementedException();
        }
        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }


    }

    
}
