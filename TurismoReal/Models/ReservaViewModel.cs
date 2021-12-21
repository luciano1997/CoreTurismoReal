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
        public string fechaLlegadas { get; set; }
        public string fechaSalidas { get; set; }
        public int ValorReserva { get; set; }
        public int EstadoReserva { get; set; }
        public int departamentoId { get; set; }
        public int usuarioId { get; set; }

        public string fechaReservadaId { get; set; }
        public string[] FechasReservadasId { get; set; }

        public string fechaReserva { get; set; }

    }
}
