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
        public int cantidadBanos { get; set; }
        public int valorArriendo { get; set; }
        public string descripcion { get; set; }
        public int estado { get; set; }
        public string nombreCalle { get; set; }
        public int numeroCalle { get; set; }
        public int numeroDepartamento { get; set; }
        public string Comuna { get; set; }
        public int ComunaId { get; set; }
        public int DepartamentoId { get; set; }
        public string Region { get; set; }
        public IList<string> imagenes { get; set; }
        public DireccionViewModel direccion { get; set; }





    }

    public class DepartamentoImagen: General
    {
        //internal Retorno retorno;
        public int id { get; set; }
        public int idDepartamento { get; set; }
        public string imagenesUrl { get; set; }

    }
}
