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
            var mesSalida = reserva.fechaSalida.ToString("dd");
            var diaSalida = reserva.fechaSalida.ToString("MM"); // recordar que el mes es el dia en el front
            var anoSalida = reserva.fechaSalida.ToString("yyyy");

            var mesLlegada = reserva.fechaLlegada.ToString("dd");
            var diaLlegada = reserva.fechaLlegada.ToString("MM"); // recordar que el mes es el dia en el front
            var anoLlegada = reserva.fechaLlegada.ToString("yyyy");
            reserva.fechaSalida = new DateTime(Int32.Parse(anoSalida), Int32.Parse(diaSalida), Int32.Parse(mesSalida));
            reserva.fechaLlegada = new DateTime(Int32.Parse(anoLlegada), Int32.Parse(diaLlegada), Int32.Parse(mesLlegada));

            //if (reserva.fechaLlegada < DateTime.Now || reserva.fechaSalida < DateTime.Now)
            //{
            //    return BadRequest(new Retorno()
            //    {
            //        Codigo = "er",
            //        Mensaje = "Fecha debe ser menor a la fecha actual, Verifique las fechas ingresadas"
            //    });
            //}

            var depto = _departamentoContext.selectDeptoById(reserva.departamentoId);
            var dias = 0;
            if ((reserva.fechaSalida - reserva.fechaLlegada).Days == 0)
            {
                dias = 1;
            }
            else
            {
                dias = (reserva.fechaSalida - reserva.fechaLlegada).Days;
            }

            var valorEstadia = dias  * depto.valorArriendo;

            var reservas = 
                _reservaContext.selectIdFechasReservadas(reserva.fechaLlegada.ToString(), reserva.fechaSalida.AddDays(-1).ToString());
            
            var txt = "";
            foreach (var item in reservas )
            {
                txt = txt + item.fechaReservadaId + ",";
            }
            txt = txt.Remove(txt.Length-1);

            var result = _reservaContext.InsertReserva(reserva, valorEstadia, txt);

            

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

        [HttpGet("GetFechasDisponiblesByDepto/{id}/{mes}/{ano}")]
        public ActionResult<List<DepartamentoViewModel>> GetFechasDisponiblesByDepto(int id, int mes,int ano)
        {
            IList<ReservaViewModel> depto = new List<ReservaViewModel>();

            if (ano == 2021 && mes <12)
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "La fecha debe ser Superior o igual a la fecha actual" });
            }

            depto = _reservaContext.selectFechasNoReservadasByIdDepto(id, mes, ano);


            if (depto.Any())
            {
                return Ok(depto);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles" });
            }

        }
        [HttpGet("GetReservasByUserId/{id}")]
        public ActionResult<List<ReservaViewModel>> GetReservasByUserId(int id)
        {
            IList<ReservaViewModel> depto = new List<ReservaViewModel>();

            depto = _reservaContext.selectReservasByUserId(id);



            if (depto.Any())
            {
                return Ok(depto);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "no hay departamentos disponibles" });
            }

        }

        [HttpPut("PutReservaEstadoById")]
        public IActionResult PutReservaEstadoById([FromBody] ReservaViewModel reserva)
        {

            var result = _reservaContext.UpdateReservaEstadoById(reserva);
            if (result > 0 )
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new General.Retorno() { Codigo = "er", Mensaje = "No se pudo actualizar el registro" });
            }
        }

    }
}
