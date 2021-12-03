using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Class;
using TurismoReal.Models;
using System.Net.Mail;
using System.Security.Cryptography;

namespace TurismoReal.Context.Usuario
{
    public class UsuarioContext
    {

        public string ConnectionString { get; set; }

        public UsuarioContext(string connectionString) => this.ConnectionString = connectionString;

        private SqlConnection GetConnection() => new SqlConnection(ConnectionString);


        public IList<UsuarioViewModel> selectAllUsuarios()
        {

            IList<UsuarioViewModel> usuarios = new List<UsuarioViewModel>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select * from usuarios", conn);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            usuarios.Add(new UsuarioViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                nombres = new Functions().ReaderToValue<string>(reader["nombres"]),
                                apellidoPaterno = new Functions().ReaderToValue<string>(reader["apellido_paterno"]),
                                apellidoMaterno = new Functions().ReaderToValue<string>(reader["apellido_materno"]),
                                rut = new Functions().ReaderToValue<string>(reader["rut"]),
                                telefono = new Functions().ReaderToValue<int>(reader["telefono"]),
                                correo = new Functions().ReaderToValue<string>(reader["correo"]),
                                tipoUsuario = new Functions().ReaderToValue<string>(reader["tipo_usuario"]),
                                password = new Functions().ReaderToValue<string>(reader["password"]),
                                vigente = new Functions().ReaderToValue<int>(reader["vigente"])
                            });
                        }

                    }
                    conn.Close();


                }
            }
            catch (Exception e)
            {
                usuarios = new List<UsuarioViewModel>();
                usuarios.Add(
                    new UsuarioViewModel()
                    {
                        retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() }
                    });

            };

            return usuarios;
        }



        #region ValidarUsuario

        public int ValidarUsuario(string correo,string password )
        {

            UsuarioViewModel usuario = new UsuarioViewModel();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select * from usuarios Where correo = @correo and  password = @password ", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@password", password);
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            usuario = new UsuarioViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                               // nombres = new Functions().ReaderToValue<string>(reader["nombres"]),
                               // apellidoPaterno = new Functions().ReaderToValue<string>(reader["apellido_paterno"]),
                               // apellidoMaterno = new Functions().ReaderToValue<string>(reader["apellido_materno"]),
                               // telefono = new Functions().ReaderToValue<int>(reader["telefono"]),
                               // rut = new Functions().ReaderToValue<string>(reader["rut"]),
                               // correo = new Functions().ReaderToValue<string>(reader["correo"]),
                               // tipoUsuario = new Functions().ReaderToValue<string>(reader["tipo_usuario"]),
                               //// password = new Functions().ReaderToValue<string>(reader["password"]),

                            };
                        }

                    }
                    conn.Close();
                   

                }
            }
            catch (Exception e)
            {
                usuario = new UsuarioViewModel();
                

            };

            return usuario.id;
        }

        #endregion

        // insert usuario
        public int InsertPassword(int IdUsuario, string Password)
        {
            int retorno = 0;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("spInsertPassword", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserId", IdUsuario);
                using (MD5 md5Hash = MD5.Create())
                {
                    string hashPass = Class.StringCipher.GetFtrMd5Hash(md5Hash, Password);
                    cmd.Parameters.AddWithValue("Nombre", hashPass);
                }
                cmd.Parameters.AddWithValue("Fecha", Functions.GetHoraOficial());
                cmd.Parameters.AddWithValue("Vigente", true);

                

                conn.Open();
                
                retorno = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return retorno;
        }



        // ******************************** SELECT BY ID ************************


        public UsuarioViewModel selectUsuarioById(int id)
        {

            UsuarioViewModel usuario = new UsuarioViewModel();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    

                    SqlCommand cmd = new SqlCommand("select * from usuarios where id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            usuario = new UsuarioViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                nombres = new Functions().ReaderToValue<string>(reader["nombres"]),
                                apellidoPaterno = new Functions().ReaderToValue<string>(reader["apellido_paterno"]),
                                apellidoMaterno = new Functions().ReaderToValue<string>(reader["apellido_materno"]),
                                telefono = new Functions().ReaderToValue<int>(reader["telefono"]),
                                rut = new Functions().ReaderToValue<string>(reader["rut"]),
                                correo = new Functions().ReaderToValue<string>(reader["correo"]),
                                tipoUsuario = new Functions().ReaderToValue<string>(reader["tipo_usuario"]),
                                vigente = new Functions().ReaderToValue<int>(reader["vigente"])

                            };
                        }

                    }
                    conn.Close();
                    if (usuario.id > 0)
                    {
                        usuario.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "" };
                    }
                    else
                    {
                        usuario.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Id no existe" };
                    }

                }
            }
            catch (Exception)
            {
                usuario = null;
            };

            return usuario;
        }



        // ******************************* INSERT *******************

        public int InsertUsuario(UsuarioViewModel usuario)
        {
            int retorno = 0;

            try
            {
                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("INSERT INTO usuarios (nombres ,apellido_paterno,apellido_materno ,rut ,telefono ,correo ,tipo_usuario ,password, vigente)" +
                        " VALUES " +
                        "(@nombres, @apellido_paterno, @apellido_materno, @rut, @telefono, @correo, @tipo_usuario, @password, @vigente); ", conn);



                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("nombres", usuario.nombres);
                    cmd.Parameters.AddWithValue("apellido_materno", usuario.apellidoMaterno);
                    cmd.Parameters.AddWithValue("apellido_paterno", usuario.apellidoPaterno);
                    cmd.Parameters.AddWithValue("rut", usuario.rut);
                    cmd.Parameters.AddWithValue("telefono", usuario.telefono);
                    cmd.Parameters.AddWithValue("correo", usuario.correo);
                    cmd.Parameters.AddWithValue("tipo_usuario", usuario.tipoUsuario);
                    cmd.Parameters.AddWithValue("password", usuario.password);
                    cmd.Parameters.AddWithValue("vigente", usuario.vigente);
                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno > 0)
                    {
                        usuario.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "registro almacenado con exito" };

                    }
                    else
                    {
                        usuario.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Ha ocurrido un error al almacenar el registro" };
                    }
                }
            }
            catch (Exception e)
            {

                retorno = 0;
                usuario.retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() };
            }
            return retorno;
        }



        // ********************** UPDATE *****************************


        #region Update

        public int UpdateUsuarioById(UsuarioViewModel usuario)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("Update usuarios set nombres= @nombres, apellido_paterno = @apellido_paterno,   " +
                                                     "apellido_materno = @apellido_materno, rut = @rut, telefono = @telefono, " +
                                                     "correo = @correo, tipo_usuario = @tipo_usuario where id = @id " , conn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("id", usuario.id);
                    cmd.Parameters.AddWithValue("nombres", usuario.nombres);
                    cmd.Parameters.AddWithValue("apellido_paterno", usuario.apellidoPaterno);
                    cmd.Parameters.AddWithValue("apellido_materno", usuario.apellidoMaterno);
                    cmd.Parameters.AddWithValue("rut", usuario.rut);
                    cmd.Parameters.AddWithValue("telefono", usuario.telefono);
                    cmd.Parameters.AddWithValue("correo", usuario.correo);
                    cmd.Parameters.AddWithValue("tipo_usuario", usuario.tipoUsuario);
                    

                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno.Equals(1))
                    {
                        usuario.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "registro Actualizado con exito" };
                    }
                    else
                    {
                        usuario.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Ha ocurrido un error al almacenar el registro" };
                    }



                }
            }
            catch (Exception e)
            {

                retorno = 0;
                usuario.retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() };
            }
            return retorno;
        }

        public int UpdateUsuarioEstadoById(UsuarioViewModel usuario)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("Update usuarios set vigente=@vigente where id = @id ", conn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("id", usuario.id);
                    cmd.Parameters.AddWithValue("vigente", usuario.vigente);



                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno.Equals(1))
                    {
                        usuario.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "registro Actualizado con exito" };
                    }
                    else
                    {
                        usuario.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Ha ocurrido un error al almacenar el registro" };
                    }



                }
            }
            catch (Exception e)
            {

                retorno = 0;
                usuario.retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() };
            }
            return retorno;
        }


        #endregion

        #region Delete


        public int DeleteUsuarioByCorreo(string correo)
        {
            int retorno = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    SqlCommand cmd = new SqlCommand("delete from usuarios where correo = @correo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("correo", correo);
                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();
                    

                }
            }
            catch (Exception)
            {

                retorno = 0;
            }

            return retorno;
        }


        #endregion

        public void EnviarConfirmacionUsuario()
        {
            string EmailOrigen = "realturismoreal@gmail.com";
            string EmailDestino = "realturismoreal@gmail.com";
            string Contraseña = "tu_pass";
            //string path = @"C:\turuta\burger.png";
           // string path2 = @"C:\turuta\a.jpg";

            MailMessage oMailMessage = new MailMessage(EmailOrigen, EmailDestino, "Usuario Creado", "<b>Bienvendio a turismo Real</b>");
            //oMailMessage.Attachments.Add(new Attachment(path));
            // oMailMessage.Attachments.Add(new Attachment(path2));


            oMailMessage.IsBodyHtml = true;

            SmtpClient oSmtpCliente = new SmtpClient("smtp.gmail.com");
            oSmtpCliente.EnableSsl = true;
            oSmtpCliente.UseDefaultCredentials = false;
            oSmtpCliente.Port = 587;
            oSmtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);

            oSmtpCliente.Send(oMailMessage);
            
            oSmtpCliente.Dispose();
            
        }

    }
}
