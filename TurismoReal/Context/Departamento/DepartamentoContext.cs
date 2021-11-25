
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

                    SqlCommand cmd = new SqlCommand("select dep.*, dir.nombre_calle, dir.numero_calle, dir.numero_depto, c.nombre_comuna, r.nombre_region from departamento dep" +
                                                    " inner join direccion dir on dep.id = dir.departamento_id" +
                                                    " inner join comuna c on c.id = dir.comuna_id" +
                                                    " inner join region r on r.id = c.region_id" +
                                                    " where dep.disp_depto = 1", conn);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            deptos.Add(new DepartamentoViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                cantidadDormitorios = new Functions().ReaderToValue<int>(reader["cantidad_dormitorios"]),
                                cantidadBanos = new Functions().ReaderToValue<int>(reader["cantidad_banos"]),
                                nombreCalle = new Functions().ReaderToValue<string>(reader["nombre_calle"]),
                                numeroCalle = new Functions().ReaderToValue<int>(reader["numero_calle"]),
                                numeroDepartamento = new Functions().ReaderToValue<int>(reader["numero_calle"]),
                                Comuna = new Functions().ReaderToValue<string>(reader["nombre_comuna"]),
                                Region = new Functions().ReaderToValue<string>(reader["nombre_region"]),
                                valorArriendo = new Functions().ReaderToValue<int>(reader["valor_arriendo"]),
                                estado = new Functions().ReaderToValue<int>(reader["disp_depto"]),
                                descripcion = new Functions().ReaderToValue<string>(reader["descripcion"])
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

                    SqlCommand cmd = new SqlCommand("select dep.*, dir.nombre_calle, dir.numero_calle, dir.numero_depto, c.nombre_comuna, r.nombre_region from departamento dep" + 
                                                    " inner join direccion dir on dep.id = dir.departamento_id" +
                                                    " inner join comuna c on c.id = dir.comuna_id"+
                                                    " inner join region r on r.id = c.region_id where dep.id = @id; ", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            depto = new DepartamentoViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                cantidadDormitorios = new Functions().ReaderToValue<int>(reader["cantidad_dormitorios"]),
                                cantidadBanos = new Functions().ReaderToValue<int>(reader["cantidad_banos"]),
                                nombreCalle = new Functions().ReaderToValue<string>(reader["nombre_calle"]),
                                numeroCalle = new Functions().ReaderToValue<int>(reader["numero_calle"]),
                                numeroDepartamento = new Functions().ReaderToValue<int>(reader["numero_depto"]),
                                Comuna = new Functions().ReaderToValue<string>(reader["nombre_comuna"]),
                                Region = new Functions().ReaderToValue<string>(reader["nombre_region"]),
                                valorArriendo = new Functions().ReaderToValue<int>(reader["valor_arriendo"]),
                                estado = new Functions().ReaderToValue<int>(reader["disp_depto"]),
                                descripcion = new Functions().ReaderToValue<string>(reader["descripcion"])

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
            catch (Exception e)
            {
                depto = null;
            };

            return depto;
        }

        //***** select departamento imagen ****


        public IList<string> selectDepartamentoImagenesById(int id)
        {
           
            IList<string> imagenes = new List<string>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select * from departamento_imagen where departamento_id = @id", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            imagenes.Add( new Functions().ReaderToValue<string>(reader["imagenUrl"]) );
                        }
                        

                    }
                    conn.Close();

                    if (!imagenes.Any())
                    {
                        imagenes.Add("https://a0.muscache.com/im/pictures/2f4ab621-7e4f-40ed-a3a6-e07a2c5f35bf.jpg?im_w=1200");
                    }
                }
            }
            catch (Exception e)
            {
                
                return imagenes;

            };

            return imagenes;
        }









        #endregion

        #region insert
        public int InsertDepartamentoImagenesById(int id, string imagenUrl)
        {
            int retorno = 0;

            try
            {
                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("insert into departamento_imagen (departamento_id, imagenUrl) values (@departamento_id, @imagenUrl);", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@departamento_id", id);
                    cmd.Parameters.AddWithValue("@imagenUrl", imagenUrl);
                 
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

        public int InsertDepartamento(DepartamentoViewModel depto)
        {
            int retorno = 0;

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    

                    SqlCommand cmd = new SqlCommand("insert into departamento (cantidad_dormitorios, cantidad_banos, valor_arriendo, disp_depto, descripcion) values (@dormitorios, @banos, @valor, @disp_depto, @descripcion);", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@dormitorios", depto.estado);
                    cmd.Parameters.AddWithValue("@banos", depto.cantidadBanos);
                    cmd.Parameters.AddWithValue("@valor", depto.valorArriendo);
                    cmd.Parameters.AddWithValue("@disp_depto", depto.estado);
                    cmd.Parameters.AddWithValue("@descripcion", depto.descripcion);
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


                    SqlCommand cmd = new SqlCommand("update departamento set " +
                        " cantidad_dormitorios = @cantidadDormitorios , cantidad_banos = @cantidadBaños, " +
                        " valor_arriendo = @valorArriendo, disp_depto=@estado,  descripcion = @descripcion where id= @id;", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", depto.id);
                    cmd.Parameters.AddWithValue("@cantidadDormitorios", depto.cantidadDormitorios);
                    cmd.Parameters.AddWithValue("@cantidadBaños ", depto.cantidadBanos);
                    cmd.Parameters.AddWithValue("@valorArriendo", depto.valorArriendo);
                    cmd.Parameters.AddWithValue("@estado", depto.estado);
                    cmd.Parameters.AddWithValue("@descripcion", depto.descripcion);
                    
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

        public int UpdateDepartamentoEstadoById(int id, int estado)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("update departamento set disp_depto=@estado where id= @id;", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@estado", estado);
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


        #endregion

        #region Delete


        public int DeleteDepartamentoById(int id)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("delete from departamento where id =@id;", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);

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

        //public int DeleteDepartamento(int id)
        //{
        //    int retorno = 0;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection())
        //        {
        //            SqlCommand cmd = new SqlCommand("delete from departamento where id = @id", conn);
        //            cmd.CommandType = CommandType.Text;
        //            cmd.Parameters.AddWithValue("@id", id);
        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //            conn.Close();


        //        }
        //    }
        //    catch (Exception e )
        //    {

        //        retorno = 0;
        //    }

        //    return retorno;
        //}

        #endregion

    }
}

