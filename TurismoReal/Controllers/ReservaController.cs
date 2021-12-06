using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Models;
using TurismoReal.Context.Reserva;
using TurismoReal.Context.Departamento;
using static TurismoReal.Models.General;

namespace TurismoReal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : Controller
    {

        private readonly ReservaContext _reservaContext;
        private readonly DepartamentoContext _departamentoContext;


        public ReservaController(ReservaContext reservaContext,DepartamentoContext departamentoContext)
        {
            _reservaContext = reservaContext;
            _departamentoContext = departamentoContext;


        }


        [HttpPost("PostReserva")]
        public ActionResult PostReserva([FromBody]ReservaViewModel reserva)
        {
            var depto = _departamentoContext.selectDeptoById(reserva.departamentoId);

            var valorEstadia = (reserva.fechaSalida - reserva.fechaLlegada).Days * depto.valorArriendo;

            var result = _reservaContext.InsertReserva(reserva, valorEstadia);
            

            if (result > 0 )
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
    }
}
