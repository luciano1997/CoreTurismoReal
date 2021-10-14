using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Class;
using TurismoReal.Models;

namespace TurismoReal.Context.Reserva
{
    public class ReservaContext
    {
        public string ConnectionString { get; set; }

        public ReservaContext(string connectionString) => this.ConnectionString = connectionString;

        private SqlConnection GetConnection() => new SqlConnection(ConnectionString);


        // ******************************** SELECT ALL DEPTOS ************************
        public IList<ReservaViewModel> selectDeptos()
        {

            IList<ReservaViewModel> reservas = new List<ReservaViewModel>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SelectReservas", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reservas.Add(new ReservaViewModel()
                            {
                                id = new Functions().ReaderToValue<int>(reader["id"]), 
                                fechaLlegada = new Functions().ReaderToValue<DateTime>(reader["fecha_llegada"]),
                                fechaSalida = new Functions().ReaderToValue<DateTime>(reader["fecha_salida"]),
                            });
                        }

                    }
                    conn.Close();


                }
            }
            catch (Exception e)
            {
                reservas = new List<ReservaViewModel>();
                reservas.Add(
                    new ReservaViewModel()
                    {
                        retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() }
                    });

            };

            return reservas;
        }

    }
}
