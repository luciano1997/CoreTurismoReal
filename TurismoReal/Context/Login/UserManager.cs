using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TurismoReal.Class;
using TurismoReal.Models;

namespace TurismoReal.Context.Login
{
    public class UserManager
    {
        public string ConnectionString { get; set; }

        public UserManager(string connectionString) => this.ConnectionString = connectionString;
        private SqlConnection GetConnection() => new SqlConnection(ConnectionString);

        public UsuarioViewModel GetUsuarioByNombre(UsuarioViewModel user)
        {
            UsuarioViewModel usuario = new UsuarioViewModel();

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("spGetUsuarioByNombre", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Nombre", user.nombres);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //usuario = Mapping.Usuario.UsuarioMapping.MapeoUsuario(reader);
                    }
                }
                conn.Close();
            }
            return usuario;
        }



        public int IsPasswordUsuario(int idusuario, string pass)
        {
            int ValidPass = 0;
            using (SqlConnection conn = GetConnection())
            {
                //using (MD5 md5Hash = MD5.Create())
                //{
                //    string hashPass = StringCipher.GetFtrMd5Hash(md5Hash, pass);

                //    SqlCommand cmdP = new SqlCommand("spIsPasswordUsuario", conn);
                //    cmdP.CommandType = CommandType.StoredProcedure;
                //    cmdP.Parameters.AddWithValue("IdUsuario", idusuario);
                //    cmdP.Parameters.AddWithValue("PassHash", hashPass);
                //    conn.Open();
                //    using (var reader = cmdP.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            ValidPass = Convert.ToInt32(reader["id"]);
                //        }
                //    }
                //    conn.Close();
                //}
            }
            return ValidPass;
        }

        public int GetIdTiendaToken(int IdCentro, string key)
        {
            int ValidPass = 0;
            using (SqlConnection conn = GetConnection())
            {
                SqlCommand cmdPass = new SqlCommand("spGetIdTiendaToken", conn);
                cmdPass.CommandType = CommandType.StoredProcedure;
                cmdPass.Parameters.AddWithValue("IdCentro", IdCentro);
                cmdPass.Parameters.AddWithValue("ApiKey", key);
                conn.Open();
                using (var reader = cmdPass.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ValidPass = Convert.ToInt32(reader["id"]);
                    }
                }

                conn.Close();
            }
            return ValidPass;
        }




    }
}
