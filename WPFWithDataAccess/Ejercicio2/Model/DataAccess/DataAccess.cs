using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccess
{
    public class DataAccess
    {
        //string databaseConnection = "Data Source=DESKTOP-04QFPV5;Initial Catalog=EscuelaDB;user id=sa;password=Jb941005;";
        string databaseConnection = ConfigurationManager.ConnectionStrings["MyConnectionBD"].ConnectionString;

        public void SaveAlumno(Alumno alumno)
        {
            try
            {
                //buscar si existe el usuario notificar en un modak si no poner exitoso  
                using(SqlConnection connection = new SqlConnection(databaseConnection))
                {
                    SqlCommand command = new SqlCommand("[Escuela].[SaveAlumno]", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter nombreParameter = new SqlParameter();
                    nombreParameter.ParameterName = "Nombre";
                    nombreParameter.Value = alumno.Nombre.Trim(); // Este método devuelve una nueva cadena que no contiene espacios en blanco al principio ni al final de la cadena original

                    SqlParameter apellidosParameter = new SqlParameter();
                    apellidosParameter.ParameterName = "Apellidos";
                    apellidosParameter.Value = alumno.Apellidos.Trim();

                    SqlParameter promedioParameter = new SqlParameter();
                    promedioParameter.ParameterName = "Promedio";
                    promedioParameter.Value = alumno.Promedio;

                    command.Parameters.Add(nombreParameter);
                    command.Parameters.Add(apellidosParameter);
                    command.Parameters.Add(promedioParameter);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public List<Alumno> GetAlumnosList()
        {
            List<Alumno> resultado = new List<Alumno>();
            try
            {
                using (SqlConnection connection = new SqlConnection(databaseConnection))
                {
                    // Ejecutar una consulta
                    SqlCommand command = new SqlCommand("[Escuela].[GetAlumnoList]", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // Abrir la conexión
                    connection.Open();
                    // Ejecutar la consulta y leer los resultados
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //// Acceder a los datos
                        Alumno alumno = new Alumno();
                        alumno.Id = int.Parse(reader["Id"].ToString());
                        alumno.Nombre = reader["Nombre"].ToString();
                        alumno.Apellidos = reader["Apellidos"].ToString();
                        alumno.Promedio = decimal.Parse(reader["Promedio"].ToString());
                        resultado.Add(alumno);
                    }
                    //cierra la conexion 
                    connection.Close();
                }

            }
            catch (Exception ex)
            { //capta el error. 
                Console.WriteLine(ex.ToString());
            }

            return resultado;
        }

        public Alumno SearchAlumno(int id)
        {
            Alumno alumno = new Alumno();
            try
            {
                using (SqlConnection connection = new SqlConnection(databaseConnection)) 
                {
                    // Ejecutar una consulta
                    SqlCommand command = new SqlCommand("[Escuela].[SearchAlumno]", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // Parámetro para el procedimiento almacenado
                    command.Parameters.AddWithValue("@Id", id);

                    /* El método Parameters.AddWithValue se utiliza para agregar parámetros a un comando SQL en ADO.NET.
                    Este método permite especificar los valores de los parámetros de manera más conveniente y segura. */
                    
                    // Abrir la conexión
                    connection.Open();
                    //Comando para leer los datos regreso de la consulta 
                    var reader = command.ExecuteReader();
                  
                    while (reader.Read())
                    {
                        // Recuperar el valor devuelto por el procedimiento almacenado
                        alumno.Id = int.Parse(reader["Id"].ToString());
                        alumno.Nombre = reader["Nombre"].ToString();
                        alumno.Apellidos = reader["Apellidos"].ToString();
                        alumno.Promedio = decimal.Parse(reader["Promedio"].ToString());
                    }
                    reader.Close();

                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return alumno;
        }

        public int SearchFullNameAlumno(int Id,string Nombre, string Apellidos) //busca si ya existe un alumno con ese nombre y apellido 
        {
            int id=0;
            try
            {
                using (SqlConnection connection = new SqlConnection(databaseConnection))
                {
                    // Ejecutar una consulta
                    SqlCommand command = new SqlCommand("[Escuela].[SearchFullNameAlumno]", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // Parámetro para el procedimiento almacenado
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Nombre", Nombre);
                    command.Parameters.AddWithValue("@Apellidos", Apellidos);

                  

                    // Abrir la conexión
                    connection.Open();
                    //Comando para leer los datos regreso de la consulta 
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Recuperar el valor devuelto por el procedimiento almacenado
                        id = int.Parse(reader["Id"].ToString()); //si encuentra devuelve su id & si no devuelve cero 
                    }
                    reader.Close();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

           return id;
        }

        public int SearchFllAlumno(string Nombre, string Apellidos) //busca si ya existe un alumno con ese nombre y apellido 
        {
            int id = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(databaseConnection))
                {
                    // Ejecutar una consulta
                    SqlCommand command = new SqlCommand("[Escuela].[SearchFllAlumno]", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // Parámetro para el procedimiento almacenado
                    command.Parameters.AddWithValue("@Nombre", Nombre);
                    command.Parameters.AddWithValue("@Apellidos", Apellidos);
                    // Abrir la conexión
                    connection.Open();
                    //Comando para leer los datos regreso de la consulta 
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Recuperar el valor devuelto por el procedimiento almacenado
                        id = int.Parse(reader["Id"].ToString()); //si encuentra devuelve su id & si no devuelve cero 
                    }
                    reader.Close();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return id;
        }

        public void UpdateAlumno(Alumno alumno, out int existe, out int repetido) //out parameters para devolver múltiples valores desde un método.
        {
            existe = 0;
            repetido = 0;
            Alumno alumnobuscado = new Alumno();
            try
            {
                alumnobuscado = SearchAlumno(alumno.Id);
                if (!string.IsNullOrEmpty(alumnobuscado.Nombre)) //El método IsNullOrEmpty() toma una cadena como argumento y devuelve true si la cadena es null o tiene una longitud de cero; de lo contrario, devuelve false
                {
                    existe = 1;//si existe 
                    int idrepetido = SearchFullNameAlumno(alumnobuscado.Id,alumnobuscado.Nombre.Trim(),alumnobuscado.Apellidos.Trim());
                    //if (nombrecompleto.Equals(alumno.Nombre + alumno.Apellidos, StringComparison.OrdinalIgnoreCase) == false) //El método Equals() en C# se utiliza para comparar dos objetos y determinar si son iguales o no
                    if(idrepetido==0) //si no esta repetido devuelve cero 
                    {
                        repetido = 0; //no esta repetido 
                        using (SqlConnection connection = new SqlConnection(databaseConnection))
                        {
                            SqlCommand command = new SqlCommand("[Escuela].[UpdateAlumno]", connection);
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            // Parámetros para el procedimiento almacenado
                            command.Parameters.AddWithValue("@Id", alumno.Id);
                            command.Parameters.AddWithValue("@Nombre", alumno.Nombre);
                            command.Parameters.AddWithValue("@Apellidos", alumno.Apellidos);
                            command.Parameters.AddWithValue("@Promedio", alumno.Promedio);
                            connection.Open();
                            int filasAfectadas = command.ExecuteNonQuery();
                            //Console.WriteLine($"Filas afectadas: {filasAfectadas}");
                            connection.Close();
                        }
                    }
                    else { repetido = 1; }
                }
              else { existe = 0; }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           // return (existe, repetido);
            
        }

        public int DeleteAlumno(int Id) {
           int  existe = 0;
            try
            {
                Alumno alumnobuscado = new Alumno();
                alumnobuscado= SearchAlumno(Id);
                if (!string.IsNullOrEmpty(alumnobuscado.Nombre)) //El método IsNullOrEmpty() toma una cadena como argumento y devuelve true si la cadena es null o tiene una longitud de cero; de lo contrario, devuelve false
                {
                    existe = 1;//si existe 
                    using (SqlConnection connection = new SqlConnection(databaseConnection))
                    {
                        SqlCommand command = new SqlCommand("[Escuela].[DeleteAlumno]", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        // Parámetros para el procedimiento almacenado
                        command.Parameters.AddWithValue("@Id", Id);
                        connection.Open();
                        int filasAfectadas = command.ExecuteNonQuery();
                        //Console.WriteLine($"Filas afectadas: {filasAfectadas}");
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return existe;

        }














    }
}
