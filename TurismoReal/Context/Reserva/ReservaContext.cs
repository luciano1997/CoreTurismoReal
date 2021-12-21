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
        public IList<ReservaViewModel> selectReservas()
        {

            IList<ReservaViewModel> reservas = new List<ReservaViewModel>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT id ,fecha_llegada ,fecha_salida ,fecha_creación ,valor_reserva " +
                        ",estado_reserva ,departamento_id ,usuarios_id FROM reservas ", conn);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reservas.Add(new ReservaViewModel()
                            {
                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                fechaLlegada = new Functions().ReaderToValue<DateTime>(reader["fecha_llegada"]),
                                fechaSalida = new Functions().ReaderToValue<DateTime>(reader["fecha_salida"]),
                                ValorReserva = new Functions().ReaderToValue<int>(reader["valor_reserva"]),
                                EstadoReserva = new Functions().ReaderToValue<int>(reader["estado_reserva"]),
                                departamentoId = new Functions().ReaderToValue<int>(reader["departamento_id"]),
                                usuarioId = new Functions().ReaderToValue<int>(reader["usuario_id"])

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


        public ReservaViewModel selectReservaById(int id)
        {

            ReservaViewModel reserva = new ReservaViewModel();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT id ,fecha_llegada ,fecha_salida ,fecha_creación ,valor_reserva " +
                         ",estado_reserva ,departamento_id ,usuarios_id FROM reservas where id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reserva = new ReservaViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                fechaLlegada = new Functions().ReaderToValue<DateTime>(reader["fecha_llegada"]),
                                fechaSalida = new Functions().ReaderToValue<DateTime>(reader["fecha_salida"]),
                                ValorReserva = new Functions().ReaderToValue<int>(reader["valor_reserva"]),
                                EstadoReserva = new Functions().ReaderToValue<int>(reader["estado_reserva"]),
                                departamentoId = new Functions().ReaderToValue<int>(reader["departamento_id"]),
                                usuarioId = new Functions().ReaderToValue<int>(reader["usuario_id"])

                            };
                        }

                    }
                    conn.Close();
                    if (reserva.id > 0)
                    {
                        reserva.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "" };
                    }
                    else
                    {
                        reserva.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Id no existe" };
                    }

                }
            }
            catch (Exception e)
            {
                reserva = null;
            };

            return reserva;
        }

        public IList<ReservaViewModel> selectReservasByUserId(int id)
        {

            IList<ReservaViewModel> reserva = new List<ReservaViewModel>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT id ,fecha_llegada ,fecha_salida ,valor_reserva " +
                         ",estado_reserva ,departamento_id ,usuarios_id FROM reservas where usuarios_id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reserva.Add(new ReservaViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                fechaLlegada = new Functions().ReaderToValue<DateTime>(reader["fecha_llegada"]),
                                fechaSalida = new Functions().ReaderToValue<DateTime>(reader["fecha_salida"]),
                                ValorReserva = new Functions().ReaderToValue<int>(reader["valor_reserva"]),
                                EstadoReserva = new Functions().ReaderToValue<int>(reader["estado_reserva"]),
                                departamentoId = new Functions().ReaderToValue<int>(reader["departamento_id"]),
                                usuarioId = new Functions().ReaderToValue<int>(reader["usuarios_id"])

                            });
                        }

                    }
                    conn.Close();
                    

                }
            }
            catch (Exception e)
            {
                reserva = null;
            };

            return reserva;
        }
        public IList<ReservaViewModel> selectIdReservadosByDeptoId(int deptoId)
        {

            IList<ReservaViewModel> reserva = new List<ReservaViewModel>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select fecha_reservada_id from reservas where departamento_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", deptoId);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reserva.Add( new ReservaViewModel()
                            {


                                fechaReservadaId = new Functions().ReaderToValue<string>(reader["fecha_reservada_id"])

                            });
                        }

                    }
                    conn.Close();
                   

                }
            }
            catch (Exception e)
            {
                reserva = null;
            };

            return reserva;
        }
        public IList<ReservaViewModel> selectFechasNoReservadasByIdDepto(int id, int mes, int ano)
        {

            IList<ReservaViewModel> reserva = new List<ReservaViewModel>();
            try
            {
                var idReservados = selectIdReservadosByDeptoId(id);
                var txt = "";
                foreach (var item in idReservados)
                {
                    txt = txt + item.fechaReservadaId + ",";
                }
                if (idReservados.Count() == 0)
                {
                    txt = "0";
                }
                else
                {
                    txt = txt.Remove(txt.Length - 1);
                }
                
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    //SqlCommand cmd = new SqlCommand("select id_fecha_reserva, fecha_reserva from  fecha_reservada  full join reservas  on fecha_reserva  between fecha_llegada  and  fecha_salida " +
                    //     " where fecha_llegada is null and MONTH(fecha_reserva) = @mes and YEAR(fecha_reserva) = @ano and reservas.departamento_id = @id_depto;", conn);

                    SqlCommand cmd = new SqlCommand("select *  from fecha_reservada where id_fecha_reserva not in ( " + txt + " ) and MONTH(fecha_reserva) = @mes and YEAR(fecha_reserva) = @ano;", conn);
                    //cmd.Parameters.AddWithValue("@id_depto", id);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@ano", ano);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reserva.Add(new ReservaViewModel()
                            {

                                fechaReservadaId = new Functions().ReaderToValue<string>(reader["id_fecha_reserva"]),
                                fechaReserva = new Functions().ReaderToValue<string>(reader["fecha_reserva"]),

                            });
                        }

                    }
                    conn.Close();
                   

                }
            }
            catch (Exception e)
            {
                reserva = null;
            };

            return reserva;
        }

        public IList<ReservaViewModel> selectIdFechasReservadas(string inicio, string fin)
        {

            IList<ReservaViewModel> reservas = new List<ReservaViewModel>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select id_fecha_reserva from fecha_reservada where fecha_reserva between @inicio and @fin ;", conn);
                    cmd.Parameters.AddWithValue("@inicio", inicio);
                    cmd.Parameters.AddWithValue("@fin", fin);
                    
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reservas.Add(new ReservaViewModel()
                            {

                                fechaReservadaId = new Functions().ReaderToValue<string>(reader["id_fecha_reserva"]),


                            });
                        }

                    }
                    conn.Close();
                   

                }
            }
            catch (Exception e)
            {
                reservas = null;
            };

            return reservas;
        }


        public int InsertReserva(ReservaViewModel reserva, int valor, string txt)
        {
            int retorno = 0;

            try
            {
                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("INSERT INTO reservas (fecha_llegada , fecha_salida , valor_reserva , estado_reserva , departamento_id " +
                                     "  , usuarios_id, fecha_reservada_id) VALUES (@fecha_llegada, @fecha_salida , @valor_reserva ,@estado_reserva ,@departamento_id, @usuarios_id, @fecha_reservada_id); ", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@fecha_llegada", reserva.fechaLlegada);
                    cmd.Parameters.AddWithValue("@fecha_salida", reserva.fechaSalida);
                   
                    cmd.Parameters.AddWithValue("@valor_reserva", valor);
                    cmd.Parameters.AddWithValue("@estado_reserva", 1);
                    cmd.Parameters.AddWithValue("@departamento_id", reserva.departamentoId);
                    cmd.Parameters.AddWithValue("@usuarios_id", reserva.usuarioId);
                    cmd.Parameters.AddWithValue("@fecha_reservada_id", txt);
                    

                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();


                }
            }
            catch (Exception e)
            {

                retorno = 0;

            }
            return retorno;
        }


        public int UpdateReservaById(ReservaViewModel reserva)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("UPDATE reservas    SET fecha_llegada = @fecha_llegada ,fecha_salida = @fecha_salida, fecha_creación = @fecha_creación," +
                        " valor_reserva = @valor_reserva, estado_reserva = @estado_reserva, departamento_id = @departamento_id, usuarios_id = @usuarios_id WHERE id = @id; ", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@fecha_llegada", reserva.fechaLlegada);
                    cmd.Parameters.AddWithValue("@fecha_salida", reserva.fechaSalida);
                   
                    cmd.Parameters.AddWithValue("@valor_reserva", reserva.ValorReserva);
                    cmd.Parameters.AddWithValue("@estado_reserva", reserva.EstadoReserva);
                    cmd.Parameters.AddWithValue("@departamento_id", reserva.departamentoId);
                    cmd.Parameters.AddWithValue("@usuarios_id", reserva.departamentoId);

                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno.Equals(1))
                    {
                        reserva.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "registro almacenado con exito" };
                    }
                    else
                    {
                        reserva.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Ha ocurrido un error al almacenar el registro" };
                    }



                }
            }
            catch (Exception e)
            {

                retorno = 0;
                reserva.retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() };
            }
            return retorno;
        }
        public int UpdateIdReservasById(string ids)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("UPDATE reservas SET fecha_reservada_id = @fecha_reservada_id WHERE id = @id; ", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@fecha_reservada_id", ids);
                    

                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

       
                }
            }
            catch (Exception e)
            {
                retorno = 0;
                
            }
            return retorno;
        }

        public int UpdateReservaEstadoById(ReservaViewModel reserva)
        {
            int retorno = 0;
            try
            {

               
                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("UPDATE reservas SET estado_reserva = @estado_reserva WHERE id = @id; ", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", reserva.id);
                    cmd.Parameters.AddWithValue("@estado_reserva", 2);

                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();


                }
            }
            catch (Exception e)
            {
                retorno = 0;

            }
            return retorno;
        }
    }
}
