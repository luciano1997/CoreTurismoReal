using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Class;
using TurismoReal.Models;

namespace TurismoReal.Context.Direccion
{
    public class DireccionContext
    {
        public string ConnectionString { get; set; }

        public DireccionContext(string connectionString) => this.ConnectionString = connectionString;

        private SqlConnection GetConnection() => new SqlConnection(ConnectionString);




        #region Select

        public DireccionViewModel selectDireccionById(int id)
        {

            DireccionViewModel dir = new DireccionViewModel();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select * from direccion where departamento_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            dir = new DireccionViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id_direccion"]),
                                NombreCalle  = new Functions().ReaderToValue<string>(reader["nombre_calle"]),
                                NumeroCalle = new Functions().ReaderToValue<string>(reader["numero_calle"]),
                                NumeroDepto = new Functions().ReaderToValue<string>(reader["numero_depto"]),
                                DepartamentoId = new Functions().ReaderToValue<int>(reader["departamento_id"]),
                                ComunaId = new Functions().ReaderToValue<int>(reader["comuna_id"]),
                                

                            };
                        }

                    }
                    conn.Close();
                    if (dir.id > 0)
                    {
                        dir.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "" };
                    }
                    else
                    {
                        dir.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Id no existe" };
                    }

                }
            }
            catch (Exception e)
            {
                dir = null;
            };

            return dir;
        }

        #endregion



        #region Insert

        public int InsertDireccion(DepartamentoViewModel depto)
        {
            int retorno = 0;

            try
            {
                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd1 = new SqlCommand("select max(id) as id from departamento;", conn);
                    cmd1.CommandType = CommandType.Text;
                    conn.Open();
                    using (var reader = cmd1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            depto.direccion.DepartamentoId = new Functions().ReaderToValue<int>(reader["id"]);

                        }    
                    }
                    conn.Close();
                    SqlCommand cmd = new SqlCommand("insert into direccion (nombre_calle, numero_calle, numero_depto, departamento_id, comuna_id) values (@nombre_calle, @numero_calle, @departamento_id, @comuna_id);", conn);
                    
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@nombre_calle", depto.direccion.NombreCalle);
                    cmd.Parameters.AddWithValue("@numero_calle", depto.direccion.NumeroCalle);
                    cmd.Parameters.AddWithValue("@numero_depto", depto.direccion.NumeroDepto);
                    cmd.Parameters.AddWithValue("@departamento_id",depto.direccion.DepartamentoId);
                    cmd.Parameters.AddWithValue("@comuna_id", depto.direccion.ComunaId);
                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno > 0)
                    {
                        depto.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "registro almacenado con exito" };

                    }
                    else
                    {
                        depto.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Ha ocurrido un error al almacenar el registro" };
                    }
                }
            }
            catch (Exception e)
            {

                retorno = 0;
                depto.retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() };
            }
            return retorno;
        }

        #endregion


    }
}
