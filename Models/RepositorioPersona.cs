using MySql.Data.MySqlClient;

namespace PracticaMVC.Models;

public class RepositorioPersona
{   
   string ConnectionString = "Server=localhost;User=root;Password=;Database=testmvc;SslMode=none";

   public RepositorioPersona()
   {
    
   }

   public List<Persona> GetPersonas()
   {
    List<Persona> personas = new List<Persona>();
    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
    {
        var query = @"SELECT Id, Nombre, Direccion,Telefono, Nacimiento FROM personas";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Persona persona = new Persona{
                    Id = reader.GetInt32(nameof(Persona.Id)),
                    Nombre = reader.GetString(nameof(Persona.Nombre)),
                    Direccion = reader.GetString(nameof(Persona.Direccion)),
                    Telefono = reader.GetString(nameof(Persona.Telefono)),
                    Nacimiento = reader.GetDateTime(nameof(Persona.Nacimiento))
                };
                personas.Add(persona);
               }
            }
        }
        conn.Close();

    }
    return personas;
   }

   public int agregarPersona(Persona persona)
   {
    int res = -1;
    using(MySqlConnection conn = new MySqlConnection(ConnectionString))
    {   
        var query = @"INSERT INTO personas(`Nombre`, `Direccion`, `Telefono`, `Nacimiento`) VALUES (@nombre, @direccion, @telefono, @nacimiento); SELECT LAST_INSERT_ID();
        ;";
        using(var command = new MySqlCommand(query, conn))
        {
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@nombre", persona.Nombre);
            command.Parameters.AddWithValue("@direccion", persona.Direccion);
            command.Parameters.AddWithValue("@telefono", persona.Telefono);
            command.Parameters.AddWithValue("@nacimiento", persona.Nacimiento);
            conn.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            persona.Id = res;
            Console.Write(res);
            conn.Close();
        }
    }

    return res;
   }

    public Persona obtenerPersonaById(int id)
    {
        Persona persona = null;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"SELECT * FROM personas WHERE {nameof(Persona.Id)} = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    persona = new Persona
                    {
                        Id = reader.GetInt32(nameof(Persona.Id)),
                        Direccion = reader.GetString("Direccion"),
                        Telefono = reader.GetString("Telefono"),
                        Nombre = reader.GetString("Nombre"),
                        Nacimiento = reader.GetDateTime("Nacimiento"),

                    };

                }
                conn.Close();
            }
        }
        return persona;
    }
    public int eliminarPersonaById(int id)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"DELETE FROM personas WHERE {nameof(Persona.Id)} = @id";

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

    public int modificarPersona(Persona persona)
		{
			int res = -1;
			using (var connection = new MySqlConnection(ConnectionString))
			{
				string sql = "UPDATE personas SET " +
	"Nombre=@nombre, Direccion=@direccion, Telefono=@telefono, Nacimiento=@nacimiento " + "WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@nombre", persona.Nombre);
					command.Parameters.AddWithValue("@direccion", persona.Direccion);
					command.Parameters.AddWithValue("@Telefono", persona.Telefono);
					command.Parameters.AddWithValue("@Nacimiento", persona.Nacimiento);
					command.Parameters.AddWithValue("@id", persona.Id);
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}
