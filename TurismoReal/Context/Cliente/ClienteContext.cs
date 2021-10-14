using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TurismoReal.Class;
using TurismoReal.Models;

namespace TurismoReal.Context.Cliente
{
    public class ClienteContext
    {

        public string ConnectionString { get; set; }

        public ClienteContext(string connectionString) => this.ConnectionString = connectionString;

        private SqlConnection GetConnection() => new SqlConnection(ConnectionString);


        public IList<ClienteViewModel> selectAllClientes()
        {

            IList<ClienteViewModel> clientes = new List<ClienteViewModel>();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("selectClientes", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            clientes.Add(new ClienteViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                nombres = new Functions().ReaderToValue<string>(reader["nombres"]),
                                apellidoPaterno = new Functions().ReaderToValue<string>(reader["apellido_paterno"]),
                                apellidoMaterno = new Functions().ReaderToValue<string>(reader["apellido_materno"]),
                                rut = new Functions().ReaderToValue<string>(reader["rut"]),
                                correo = new Functions().ReaderToValue<string>(reader["correo"])
                            });
                        }

                    }
                    conn.Close();


                }
            }
            catch (Exception e)
            {
                clientes = new List<ClienteViewModel>();
                clientes.Add(
                    new ClienteViewModel()
                    {
                        retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() }
                    });

            };

            return clientes;
        }


        // ******************************** SELECT BY ID ************************


        public ClienteViewModel selectClienteById(int id)
        {

            ClienteViewModel cliente = new ClienteViewModel();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SelectClienteById", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            cliente = new ClienteViewModel()
                            {

                                id = new Functions().ReaderToValue<int>(reader["id"]),
                                nombres = new Functions().ReaderToValue<string>(reader["nombres"]),
                                apellidoPaterno = new Functions().ReaderToValue<string>(reader["apellido_paterno"]),
                                apellidoMaterno = new Functions().ReaderToValue<string>(reader["apellido_materno"]),
                                telefono = new Functions().ReaderToValue<string>(reader["telefono"]),
                                rut = new Functions().ReaderToValue<string>(reader["rut"]),
                                correo = new Functions().ReaderToValue<string>(reader["correo"])
                            };
                        }

                    }
                    conn.Close();
                    if (cliente.id > 0)
                    {
                        cliente.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "" };
                    }
                    else
                    {
                        cliente.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Id no existe" };
                    }

                }
            }
            catch (Exception)
            {
                cliente = null;
            };

            return cliente;
        }



        // ******************************* INSERT *******************

        public int InsertCliente(ClienteViewModel cliente)
        {
            int retorno = 0;

            try
            {
                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("spInsertCliente", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nombres", cliente.nombres);
                    cmd.Parameters.AddWithValue("apellido_materno", cliente.apellidoMaterno);
                    cmd.Parameters.AddWithValue("apellido_paterno", cliente.apellidoPaterno);
                    cmd.Parameters.AddWithValue("rut", cliente.rut);
                    cmd.Parameters.AddWithValue("telefono", cliente.telefono);
                    cmd.Parameters.AddWithValue("correo", cliente.correo);
                    cmd.Parameters.AddWithValue("direccion", cliente.direccion);
                    cmd.Parameters.AddWithValue("vigente", cliente.vigente);
                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno > 0)
                    {
                        cliente.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "registro almacenado con exito" };

                    }
                    else
                    {
                        cliente.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Ha ocurrido un error al almacenar el registro" };
                    }
                }
            }
            catch (Exception e)
            {

                retorno = 0;
                cliente.retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() };
            }
            return retorno;
        }



        // ********************** UPDATE *****************************


        #region Update

        public int UpdateClienteById(ClienteViewModel cliente)
        {
            int retorno = 0;
            try
            {

                using (SqlConnection conn = GetConnection())
                {


                    SqlCommand cmd = new SqlCommand("spUpdateCliente", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", cliente.id);
                    cmd.Parameters.AddWithValue("nombres", cliente.nombres);
                    cmd.Parameters.AddWithValue("apellido_materno", cliente.apellidoMaterno);
                    cmd.Parameters.AddWithValue("apellido_paterno", cliente.apellidoPaterno);
                    cmd.Parameters.AddWithValue("rut", cliente.rut);
                    cmd.Parameters.AddWithValue("telefono", cliente.telefono);
                    cmd.Parameters.AddWithValue("correo", cliente.correo);
                    cmd.Parameters.AddWithValue("direccion", cliente.direccion);
                    cmd.Parameters.AddWithValue("vigente", cliente.vigente);

                    conn.Open();
                    retorno = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (retorno.Equals(1))
                    {
                        cliente.retorno = new General.Retorno() { Codigo = "ok", Mensaje = "registro almacenado con exito" };
                    }
                    else
                    {
                        cliente.retorno = new General.Retorno() { Codigo = "er", Mensaje = "Ha ocurrido un error al almacenar el registro" };
                    }



                }
            }
            catch (Exception e)
            {

                retorno = 0;
                cliente.retorno = new General.Retorno() { Codigo = "ex", Mensaje = e.Message.ToString() };
            }
            return retorno;
        }




        #endregion

        #region Delete


        public int DeleteClienteById(int id)
        {
            int retorno = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    SqlCommand cmd = new SqlCommand("spDeleteCliente", conn);
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
