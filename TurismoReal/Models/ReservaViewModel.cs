using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurismoReal.Models
{
    public class ReservaViewModel: General
    {
        internal Retorno retorno;

        public int id { get; set; }
        public DateTime fechaLlegada { get; set; }
        public DateTime fechaSalida { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int ValorReserva { get; set; }
        public int EstadoReserva { get; set; }
        public int departamentoId { get; set; }
        public int usuariosId { get; set; }
    }
}
