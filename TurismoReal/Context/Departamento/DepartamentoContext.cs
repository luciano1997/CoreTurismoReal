
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Class;
using TurismoReal.Models;

namespace TurismoReal.Context.Departamento
{
    public class DepartamentoContext
    {

        public string ConnectionString { get; set; }

        public DepartamentoContext(string connectionString) => this.ConnectionString = connectionString;

        private SqlConnection GetConnection() => new SqlConnection(ConnectionString);

        #region Select
        public DepartamentoViewModel selectDeptos()
        {
            DepartamentoViewModel depto = new DepartamentoViewModel();

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SelectAllDepartmentos", conn);
                cmd.CommandType = CommandType.StoredProcedure;
            using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        depto = new DepartamentoViewModel()
                        {

                            id = new Functions().ReaderToValue<int>(reader["id"]),
                            direccion = new Functions().ReaderToValue<string>(reader["direccion"]),
                            cantidad_dormitorios = new Functions().ReaderToValue<int>(reader["cantidad_dormitorios"]),
                            cantidad_baños = new Functions().ReaderToValue<int>(reader["cantidad_baños"]),
                            estado = new Functions().ReaderToValue<string>(reader["estado"]),
                        };
                    }

                }
                conn.Close();

                if (depto.id > 0)
                {
                    depto.retorno = new General.Retorno() { codigo = "ok", mensaje = "" };

                }
                else
                {
                    depto.retorno = new General.Retorno() { codigo = "ok", mensaje = "" };
                }

            }
            return depto;
        }
        #endregion

        #region insert

        public int InsertDepartamento(DepartamentoViewModel model)
        {
            int retorno = 0;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("spInsertDepto", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@direc", model.direccion);
                cmd.Parameters.AddWithValue("@cantidad_baños", model.cantidad_baños);
                cmd.Parameters.AddWithValue("@cantidad_dormitorios", model.cantidad_dormitorios);
                cmd.Parameters.AddWithValue("@estado", model.estado);
                cmd.Parameters.AddWithValue("@descripcion_estado", model.descripcion_estado);
                cmd.Parameters.AddWithValue("@region", model.region);
                cmd.Parameters.AddWithValue("@comuna", model.comuna);
                cmd.Parameters.AddWithValue("@vigente", model.vigente);

                //cmd.Parameters.AddWithValue("Tipo", Functions.UpperString(model.Tipo));
                //cmd.Parameters.AddWithValue("Fecha", Functions.GetHoraOficial());
                //cmd.Parameters.AddWithValue("Vigente", true);


                //parametros de salida
                //cmd.Parameters.Add("lastId", SqlDbType.Int);
                //cmd.Parameters["lastId"].Direction = ParameterDirection.Output;
                //fin parametros salida
                conn.Open();
                cmd.Parameters.Add("@lastId", SqlDbType.Int);
                cmd.Parameters["@lastId"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                retorno = (int)cmd.Parameters["@lastId"].Value;

                

                cmd.ExecuteNonQuery();
                //retorno = Convert.ToInt32(cmd.Parameters["lastId"].Value);
                conn.Close();


                if (retorno > 0)
                    model.retorno = new General.Retorno() { codigo = "ok", mensaje = "Se ha guardado el registro." };
                else
                    model.retorno = new General.Retorno() { codigo = "er", mensaje = "Ha ocurrido un error al intentar guardar el registro." };

            }

            return retorno;
        }
        #endregion





        //public DepartamentoViewModel selectDeptosSQL()
        //    {
        //        DepartamentoViewModel depto = new DepartamentoViewModel();

        //        using (MySqlConnection conn = GetConnection()) 
        //        {
        //            conn.Open();

        //            MySqlCommand cmd = new MySqlCommand("SelectAllDepartmentos", conn);


        //            cmd.CommandType = CommandType.StoredProcedure;

        //            using (var reader = cmd.ExecuteReader())
        //            {

        //                while (reader.Read()) 
        //                {
        //                    depto = new DepartamentoViewModel()
        //                    {

        //                        id = new Functions().ReaderToValue<int>(reader["id"]),
        //                        direccion = new Functions().ReaderToValue<string>(reader["direccion"]),
        //                        nro_dormitorios = new Functions().ReaderToValue<int>(reader["cantidad_dormitorios"]),
        //                        nro_baños = new Functions().ReaderToValue<int>(reader["cantidad_baños"]),
        //                        estado = new Functions().ReaderToValue<string>(reader["estado"]),
        //                    };
        //                }

        //            }
        //            conn.Close();

        //            if (depto.id > 0)
        //            {
        //                depto.retorno = new General.Retorno() { codigo = "ok", mensaje = "" };

        //            }
        //            else
        //            {
        //                depto.retorno = new General.Retorno() { codigo = "ok", mensaje = "" };
        //            }

        //        }
        //        return depto;
        //    }

    }
}
