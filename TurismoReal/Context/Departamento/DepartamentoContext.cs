
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

                    SqlCommand cmd = new SqlCommand("select * from departamento", conn);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            deptos.Add(new DepartamentoViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                cantidadDormitorios = new Functions().ReaderToValue<int>(reader["cantidad_dormitorios"]),
                                cantidadBaños = new Functions().ReaderToValue<int>(reader["cantidad_baños"]),
                                valorArriendo = new Functions().ReaderToValue<int>(reader["cantidad_dormitorios"]),
                                estado = new Functions().ReaderToValue<string>(reader["disp_depto"]),
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
                                cantidadDormitorios = new Functions().ReaderToValue<int>(reader["cantidad_dormitorios"]),
                                cantidadBaños = new Functions().ReaderToValue<int>(reader["cantidad_baños"]),
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

        //***** select departamento imagen ****


        //public string[] selectDepartamentoImagenes(int id)
        //{
        //    string[] imagenesStr;
        //    IList<DepartamentoImagen> imagenes = new List<DepartamentoImagen>();
        //    try
        //    {

        //        using (SqlConnection conn = GetConnection())
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand("select * from departamento_imagen where id_departamento = 5", conn);
        //            cmd.CommandType = CommandType.Text;
        //            using (var reader = cmd.ExecuteReader())
        //            {

        //                while (reader.Read())
        //                {
        //                    imagenes.Add(new DepartamentoImagen()
        //                    {

        //                        id = new Functions().ReaderToValue<int>(reader["id"]),
        //                        idDepartamento = new Functions().ReaderToValue<int>(reader["id_departamento"]),
        //                        imagenesUrl = new Functions().ReaderToValue<string>(reader["imagenUrl"]),
        //                    });
        //                }
        //                for (int i = 0; i < imagenes.Count(); i++)
        //                {
        //                   // imagenesStr.Append(imagenesUrl[i]);
        //                }

        //            }
        //            conn.Close();


        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        imagenes = new List<DepartamentoImagen>();
        //        imagenes.Add(
        //            new DepartamentoImagen()
        //            {
        //                retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() }
        //            });

        //    };

        //    return imagenesStr;
        //}









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

