using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurismoReal.Models
{
    public class DireccionViewModel:General
    {

        internal Retorno retorno;

        public int id { get; set; }
        public string NombreCalle { get; set; }
        public string NumeroCalle { get; set; }
        public string NumeroDepto { get; set; }
        public int DepartamentoId { get; set; }
        public int ComunaId  { get; set; }
        public int ComunaNombre { get; set; }

    }
}
