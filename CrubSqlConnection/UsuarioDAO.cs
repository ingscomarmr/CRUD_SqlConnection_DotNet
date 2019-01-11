using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//para la conexion de base de datos
using System.Data;
using System.Data.SqlClient;

namespace CrubSqlConnection
{
    class UsuarioDAO
    {

        const string STR_CONNECTION = "Server=localhost\\SQLEXPRESS01;Database=ControlEscolarDB;Trusted_Connection=True;";
        //para manejar la conexion a base de datos
        private IDbConnection CreateDBConnection() {
            
            //iniciando la coneccion
            SqlConnection connection = new SqlConnection(STR_CONNECTION);
            connection.Open();
            return connection;
        }

        #region OBTENER LISTA DE USUARIOS
        public List<UsuarioBean> GetUsuario()
        {
            List<UsuarioBean> rList = new List<UsuarioBean>();
            try
            {
                //creamos la conexion para mandar a leer la información
                using (IDbConnection conn = CreateDBConnection()) {
                    //creamos un comando para leer la base de datos
                    using (IDbCommand command = conn.CreateCommand()) {
                        //comando sql
                        command.CommandText = @"SELECT 
                                    u.UsuarioId,u.NombreUsuario,u.Contrasena 
                                FROM dbo.Usuario u WHERE u.BajaLogica = 0";

                        using (IDataReader dataReader = command.ExecuteReader()) {
                            //leemos los datos
                            while (dataReader.Read()) { //mientras sea true existen registros que leer
                                rList.Add(new UsuarioBean() {
                                    Id = dataReader.GetInt32(0),
                                    UsuarioName = dataReader.GetString(1),
                                    Pwd = dataReader.GetString(2),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:{0}", ex.Message);
                throw ex;
            }
            return rList;
        }
        #endregion

        #region GUARDAR USUARIO
        public void SaveUsuario(UsuarioBean u) {
            try
            {
                using (IDbConnection connection = CreateDBConnection()) {
                    using (IDbCommand command = connection.CreateCommand()) {
                        if (u.Id == 0)
                        {
                            //update
                            command.CommandText = @"INSERT INTO dbo.Usuario(NombreUsuario,Contrasena)
                                                    VALUES(@usuario, @pwd)";                            
                        }
                        else {
                            //insert
                            command.CommandText = @"UPDATE dbo.Usuario
                                SET NombreUsuario=@usuario,
                                Contrasena=@pwd
                                WHERE UsuarioId=@id";

                            IDbDataParameter idP = command.CreateParameter();
                            idP.ParameterName = "id";
                            idP.Value = u.Id;
                            command.Parameters.Add(idP);
                        }
                        //agregamos los parametros
                        IDbDataParameter usuarioP = command.CreateParameter();
                        usuarioP.ParameterName = "usuario";
                        usuarioP.Value = u.UsuarioName;
                        command.Parameters.Add(usuarioP);

                        IDbDataParameter pwdP = command.CreateParameter();
                        pwdP.ParameterName = "pwd";
                        pwdP.Value = u.Pwd;
                        command.Parameters.Add(pwdP);

                        //ejecutamos
                       int i = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ELIMINAR USUARIO CON TRASACCION
        public void DeleteUsuario(UsuarioBean u) {
            try
            {
                using (IDbConnection connection = CreateDBConnection()) {
                    using (IDbTransaction transaction = connection.BeginTransaction()) {

                        try
                        {
                            using (IDbCommand command = connection.CreateCommand())
                            {
                                command.Transaction = transaction; //le agregamos la transaccion
                                command.CommandText = "DELETE FROM dbo.RolUsuario WHERE UsuarioId=@id";
                                IDbDataParameter idParam = command.CreateParameter();
                                idParam.ParameterName = "id";
                                idParam.Value = u.Id;
                                command.Parameters.Add(idParam);
                                command.ExecuteNonQuery();
                                //esto ya no requiere de parametro
                                command.CommandText = "DELETE FROM dbo.Usuario WHERE UsuarioId = @id";
                                command.ExecuteNonQuery();
                            }
                            transaction.Commit(); //realiza el commit de la trasaccion
                        }
                        catch (Exception)
                        {
                            transaction.Rollback(); //si falla algo realizamos el rollback
                        }
                       
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
