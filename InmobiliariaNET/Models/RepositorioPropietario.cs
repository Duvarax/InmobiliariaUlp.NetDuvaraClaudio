using MySql.Data.MySqlClient;

namespace PracticaMVC.Models;

public class RepositorioPropietario
{   
   string ConnectionString = "Server=localhost;User=root;Password=;Database=testmvc;SslMode=none";

   public RepositorioPropietario()
   {
    
   }

   public List<Propietario> GetPropietarios()
   {
    List<Propietario> Propietarios = new List<Propietario>();
    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
    {
        var query = @"SELECT Id, Nombre, Apellido,Telefono,Dni, Email FROM propietarios";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Propietario propietario = new Propietario{
                    Id = reader.GetInt32(nameof(propietario.Id)),
                    Nombre = reader.GetString(nameof(propietario.Nombre)),
                    Apellido = reader.GetString(nameof(propietario.Apellido)),
                    Telefono = reader.GetString(nameof(propietario.Telefono)),
                    Dni = reader.GetString(nameof(propietario.Dni)),
                    Email = reader.GetString(nameof(propietario.Email)),
                };
                Propietarios.Add(propietario);
               }
            }
        }
        conn.Close();

    }
    return Propietarios;
   }

   public int agregarPropietario(Propietario propietario)
   {
    int res = -1;
    using(MySqlConnection conn = new MySqlConnection(ConnectionString))
    {   
        var query = @"INSERT INTO propietarios(`Nombre`, `Apellido`, `Telefono`, `Dni`, `Email`) VALUES (@nombre, @apellido, @telefono, @dni, @email); SELECT LAST_INSERT_ID();
        ;";
        using(var command = new MySqlCommand(query, conn))
        {
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@nombre", propietario.Nombre);
            command.Parameters.AddWithValue("@apellido", propietario.Apellido);
            command.Parameters.AddWithValue("@telefono", propietario.Telefono);
            command.Parameters.AddWithValue("@dni", propietario.Dni);
            command.Parameters.AddWithValue("@email", propietario.Email);
            conn.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            propietario.Id = res;
            Console.Write(res);
            conn.Close();
        }
    }

    return res;
   }

    public Propietario obtenerPropietarioById(int id)
    {
        Propietario? propietario = null;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"SELECT * FROM propietarios WHERE {nameof(Propietario.Id)} = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    propietario = new Propietario
                    {
                        Id = reader.GetInt32(nameof(Propietario.Id)),
                        Apellido = reader.GetString("Apellido"),
                        Telefono = reader.GetString("Telefono"),
                        Nombre = reader.GetString("Nombre"),
                        Dni = reader.GetString("Dni"),
                        Email = reader.GetString("Email"),
                    };

                }
                conn.Close();
            }
        }
        return propietario;
    }
    public int eliminarPropietarioById(int id)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"DELETE FROM propietarios WHERE {nameof(Propietario.Id)} = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

    public int modificarPropietario(Propietario propietario)
		{
			int res = -1;
			using (var connection = new MySqlConnection(ConnectionString))
			{
				string sql = "UPDATE propietarios SET " +
	"Nombre=@nombre, Apellido=@apellido, Telefono=@telefono, Dni=@dni, Email=@email " + "WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@nombre", propietario.Nombre);
					command.Parameters.AddWithValue("@apellido", propietario.Apellido);
					command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                    command.Parameters.AddWithValue("@email", propietario.Email);
					command.Parameters.AddWithValue("@dni", propietario.Dni);
					command.Parameters.AddWithValue("@id", propietario.Id);
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}
