using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurismoReal.Models
{
    public class UsuarioViewModel: General
    {
        internal Retorno retorno;

        public int  id { get; set; }
        public string nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string rut { get; set; }
        public int telefono { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public string tipoUsuario { get; set; }
        public string vigente { get; set; }
        public string password { get; set; }

    }
}
