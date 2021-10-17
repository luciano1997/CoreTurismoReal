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
        
        public int cantidadDormitorios { get; set; }
        public int cantidadBaños { get; set; }
        public int valorArriendo { get; set; }
        public string estado { get; set; }
        public string descripcionEstado { get; set; }
        public string nombreCalle { get; set; }
        public int numeroCalle { get; set; }
        public int numeroDepartamento { get; set; }
        public string Comuna { get; set; }
        public string Region { get; set; }
        public IList<string> imagenes { get; set; }
       
        public int vigente { get; set; }
        
    }

    public class DepartamentoImagen: General
    {
        //internal Retorno retorno;
        public int id { get; set; }
        public int idDepartamento { get; set; }
        public string imagenesUrl { get; set; }

    }
}
