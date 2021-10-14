
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
        // ******************************** SELECT ALL DEPTOS ************************
        public IList<DepartamentoViewModel> selectDeptos()
        {

            IList<DepartamentoViewModel> deptos = new List<DepartamentoViewModel>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SelectAllDepartmentos", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            deptos.Add(new DepartamentoViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                direccion = new Functions().ReaderToValue<string>(reader["direccion"]),
                                cantidad_dormitorios = new Functions().ReaderToValue<int>(reader["cantidad_dormitorios"]),
                                cantidad_baños = new Functions().ReaderToValue<int>(reader["cantidad_baños"]),
                                estado = new Functions().ReaderToValue<string>(reader["estado"]),
                            });
                        }

                    }
                    conn.Close();


                }
            }
            catch (Exception e)
            {
                deptos = new List<DepartamentoViewModel>();
                deptos.Add(
                    new DepartamentoViewModel()
                    {
                        retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() }
                    });

            };

            return deptos;
        }

        // ******************************** SELECT BY ID ************************


        public DepartamentoViewModel selectDeptoById(int id)
        {

            DepartamentoViewModel depto = new DepartamentoViewModel();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SelectDepartamentoById", conn);
                    cmd.Parameters.AddWithValue("@id", id);
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
                        depto.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "" };
                    }
                    else
                    {
                        depto.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Id no existe" };
                    }

                }
            }
            catch (Exception)
            {
                depto = null;
            };

            return depto;
        }








        #endregion

        #region insert

        public int InsertDepartamento(DepartamentoViewModel depto)
        {
            int retorno = 0;

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    

                    SqlCommand cmd = new SqlCommand("spInsertDepartamento", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("a", depto.estado);
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

        #region Update

        public int UpdateDepartamentoById(DepartamentoViewModel depto)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("spInsertDepartamento");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", depto.estado);
                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno.Equals(1))
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

        public int UpdateDepartamentoEstadoById(DepartamentoViewModel depto)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("spInsertDepartamento");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", depto.estado);
                    cmd.Parameters.AddWithValue("estado", depto.estado);
                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno.Equals(1))
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

        #region Delete

        public int DeleteDepartamento(int id)
        {
            int retorno = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    SqlCommand cmd = new SqlCommand("spDeleteDepartamento", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    retorno = 1;

                }
            }
            catch (Exception)
            {

                retorno = 0;
            }

            return retorno;
        }

        #endregion

    }
}

