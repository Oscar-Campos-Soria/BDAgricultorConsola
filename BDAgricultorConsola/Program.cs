using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data;



namespace BDAgricultorConsola
{
    public class Program
    {
        static string connectionString = "Server=OSCAR;Database=AgricultorDBConsola;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            if (!ValidarUsuario())
            {
                Console.WriteLine("Acceso denegado. Usuario o contraseña incorrectos.");
                return;
            }


            int opcion;

            do
            {
                Console.Clear();

                // Mostrar tablas al iniciar
                Console.WriteLine("\n=== TABLA DE AGRICULTORES ===");
                MostrarAgricultores();

                Console.WriteLine("\n=== TABLA DE CULTIVOS ===");
                MostrarCultivos();

                // Menú Principal
                Console.WriteLine("\n=== MENÚ DE OPCIONES ===");
                Console.WriteLine("1. Agregar Agricultor");
                Console.WriteLine("2. Mostrar Agricultores");
                Console.WriteLine("3. Modificar Agricultor");
                Console.WriteLine("4. Eliminar Agricultor");
                Console.WriteLine("5. Agregar Cultivo");
                Console.WriteLine("6. Mostrar Cultivos");
                Console.WriteLine("7. Modificar Cultivo");
                Console.WriteLine("8. Eliminar Cultivo");
                Console.WriteLine("9. Salir");

                Console.Write("Elige una opción: ");
                opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        AgregarAgricultor();
                        break;
                    case 2:
                        MostrarAgricultores();
                        break;
                    case 3:
                        ModificarAgricultor();
                        break;
                    case 4:
                        EliminarAgricultor();
                        break;
                    case 5:
                        AgregarCultivo();
                        break;
                    case 6:
                        MostrarCultivos();
                        break;
                    case 7:
                        ModificarCultivo();
                        break;
                    case 8:
                        EliminarCultivo();
                        break;
                    case 9:
                        Console.WriteLine("Saliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("Opción inválida, intenta de nuevo.");
                        break;
                }

                Console.WriteLine("\nPresiona una tecla para continuar...");
                Console.ReadKey();

            } while (opcion != 9);
        }

        // ================= CRUD AGRICULTOR =================

        static void AgregarAgricultor()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Edad: ");
            int edad = Convert.ToInt32(Console.ReadLine());

            Console.Write("Experiencia: ");
            string experiencia = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Agricultor (nombre, edad, experiencia) VALUES (@nombre, @edad, @experiencia)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@edad", edad);
                        command.Parameters.AddWithValue("@experiencia", experiencia);
                        command.ExecuteNonQuery();

                        Console.WriteLine("Agricultor agregado correctamente.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void MostrarAgricultores()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT idAgricultor, nombre, edad, experiencia FROM Agricultor";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine("ID | Nombre | Edad | Experiencia");
                        Console.WriteLine("--------------------------------");

                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["idAgricultor"]} | {reader["nombre"]} | {reader["edad"]} | {reader["experiencia"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void ModificarAgricultor()
        {
            Console.Write("ID del agricultor a modificar: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Nuevo Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Nueva Edad: ");
            int edad = Convert.ToInt32(Console.ReadLine());

            Console.Write("Nueva Experiencia: ");
            string experiencia = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Agricultor SET nombre = @nombre, edad = @edad, experiencia = @experiencia WHERE idAgricultor = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@edad", edad);
                        command.Parameters.AddWithValue("@experiencia", experiencia);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                            Console.WriteLine("Agricultor modificado correctamente.");
                        else
                            Console.WriteLine("No se encontró un agricultor con ese ID.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void EliminarAgricultor()
        {
            Console.Write("ID del agricultor a eliminar: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Agricultor WHERE idAgricultor = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                            Console.WriteLine("Agricultor eliminado correctamente.");
                        else
                            Console.WriteLine("No se encontró un agricultor con ese ID.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        // ================= CRUD CULTIVO =================

        static void AgregarCultivo()
        {
            Console.Write("Nombre del Cultivo: ");
            string nombreCultivo = Console.ReadLine();

            Console.Write("Temporada: ");
            string temporada = Console.ReadLine();

            Console.Write("ID del Agricultor: ");
            int idAgricultor = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Cultivo (nombreCultivo, temporada, idAgricultor) VALUES (@nombreCultivo, @temporada, @idAgricultor)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreCultivo", nombreCultivo);
                        command.Parameters.AddWithValue("@temporada", temporada);
                        command.Parameters.AddWithValue("@idAgricultor", idAgricultor);
                        command.ExecuteNonQuery();

                        Console.WriteLine("Cultivo agregado correctamente.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void MostrarCultivos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT idCultivo, nombreCultivo, temporada, idAgricultor FROM Cultivo";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine("ID | Nombre del Cultivo | Temporada | ID Agricultor");
                        Console.WriteLine("---------------------------------------------");

                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["idCultivo"]} | {reader["nombreCultivo"]} | {reader["temporada"]} | {reader["idAgricultor"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void ModificarCultivo()
        {
            Console.Write("ID del Cultivo a modificar: ");
            int idCultivo = Convert.ToInt32(Console.ReadLine());

            Console.Write("Nuevo Nombre del Cultivo: ");
            string nombreCultivo = Console.ReadLine();

            Console.Write("Nueva Temporada: ");
            string temporada = Console.ReadLine();

            Console.Write("Nuevo ID del Agricultor: ");
            int idAgricultor = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Cultivo SET nombreCultivo = @nombreCultivo, temporada = @temporada, idAgricultor = @idAgricultor WHERE idCultivo = @idCultivo";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idCultivo", idCultivo);
                        command.Parameters.AddWithValue("@nombreCultivo", nombreCultivo);
                        command.Parameters.AddWithValue("@temporada", temporada);
                        command.Parameters.AddWithValue("@idAgricultor", idAgricultor);

                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                            Console.WriteLine("Cultivo modificado correctamente.");
                        else
                            Console.WriteLine("No se encontró un cultivo con ese ID.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void EliminarCultivo()
        {
            Console.Write("ID del Cultivo a eliminar: ");
            int idCultivo = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Cultivo WHERE idCultivo = @idCultivo";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idCultivo", idCultivo);

                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                            Console.WriteLine("Cultivo eliminado correctamente.");
                        else
                            Console.WriteLine("No se encontró un cultivo con ese ID.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }


        static byte[] EncriptarContrasena(string contrasena)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(contrasena);
                return sha256.ComputeHash(bytes); // Devuelve el hash como byte[]
            }
        }



        static bool ValidarUsuario()
        {
            Console.Write("Nombre de Usuario: ");
            string nombreUsuario = Console.ReadLine();

            Console.Write("Contraseña: ");
            string contrasena = Console.ReadLine();

            byte[] contrasenaEncriptada = EncriptarContrasena(contrasena); // Devuelve byte[]

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Usuarios WHERE nombreUsuario = @nombreUsuario AND contrasena = @contrasena";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                        command.Parameters.Add("@contrasena", SqlDbType.VarBinary).Value = contrasenaEncriptada; // Agrega como VARBINARY

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            Console.WriteLine("Inicio de sesión exitoso.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Usuario o contraseña incorrectos.");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return false;
                }
            }
        }


    }
}