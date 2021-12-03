using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurismoReal.Models
{
    public class UsuarioViewModel: General
    {
        internal Retorno retorno;

        [Key]
        public int  id { get; set; }
        
        [MaxLength(30)]
        public string nombres { get; set; }
        [MaxLength(20)]
        public string apellidoPaterno { get; set; }
        [MaxLength(20)]
        public string apellidoMaterno { get; set; }
        [MaxLength(13)]
        public string rut { get; set; }
        public int telefono { get; set; }
        [MaxLength(40)]
        public string correo { get; set; }
       
        
        [MaxLength(3)]
        public string tipoUsuario { get; set; }
       
        [MaxLength(20)]
        public string password { get; set; }

        public int vigente { get; set; }
    }
}
