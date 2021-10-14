using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurismoReal.Models
{
    public class DepartamentoViewModel: General
    {
        internal Retorno retorno;

        public int id { get; set; }
        public string direccion { get; set; }
        public int cantidad_dormitorios { get; set; }
        public int cantidad_baños { get; set; }
        public string estado { get; set; }
        public string descripcion_estado { get; set; }

        public int region { get; set; }
        public int comuna { get; set; }
        public int vigente { get; set; }
        
    }
}
