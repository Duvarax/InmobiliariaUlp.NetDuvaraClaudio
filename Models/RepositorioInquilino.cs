using MySql.Data.MySqlClient;

namespace PracticaMVC.Models;

public class RepositorioInquilino
{   
   string ConnectionString = "Server=localhost;User=root;Password=;Database=testmvc;SslMode=none";

   public RepositorioInquilino()
   {
    
   }

   public List<Inquilino> GetInquilinos()
   {
    List<Inquilino> inquilinos = new List<Inquilino>();
    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
    {
        var query = @"SELECT Id, Nombre, Apellido,Telefono,Dni, Email FROM inquilinos";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Inquilino inquilino = new Inquilino{
                    Id = reader.GetInt32(nameof(inquilino.Id)),
                    Nombre = reader.GetString(nameof(inquilino.Nombre)),
                    Apellido = reader.GetString(nameof(inquilino.Apellido)),
                    Telefono = reader.GetString(nameof(inquilino.Telefono)),
                    Dni = reader.GetString(nameof(inquilino.Dni)),
                    Email = reader.GetString(nameof(inquilino.Email))
                };
                inquilinos.Add(inquilino);
               }
            }
        }
        conn.Close();

    }
    return inquilinos;
   }

   public int agregarInquilino(Inquilino inquilino)
   {
    int res = -1;
    using(MySqlConnection conn = new MySqlConnection(ConnectionString))
    {   
        var query = @"INSERT INTO inquilinos(`Nombre`, `Apellido`, `Telefono`, `Dni`, `Email`) VALUES (@nombre, @apellido, @telefono, @dni, @email); SELECT LAST_INSERT_ID();
        ;";
        using(var command = new MySqlCommand(query, conn))
        {
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
            command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
            command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
            command.Parameters.AddWithValue("@dni", inquilino.Dni);
            command.Parameters.AddWithValue("@email", inquilino.Email);
            conn.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            inquilino.Id = res;
            Console.Write(res);
            conn.Close();
        }
    }

    return res;
   }

    public Inquilino obtenerInquilinoById(int id)
    {
        Inquilino inquilino = null;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"SELECT * FROM inquilinos WHERE {nameof(Inquilino.Id)} = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    inquilino = new Inquilino
                    {
                        Id = reader.GetInt32(nameof(Inquilino.Id)),
                        Apellido = reader.GetString("Apellido"),
                        Telefono = reader.GetString("Telefono"),
                        Nombre = reader.GetString("Nombre"),
                        Dni = reader.GetString("Dni"),
                        Email = reader.GetString("Email")

                    };

                }
                conn.Close();
            }
        }
        return inquilino;
    }
    public int eliminarInquilinoById(int id)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"DELETE FROM inquilinos WHERE {nameof(Persona.Id)} = @id";

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

    public int modificarInquilino(Inquilino inquilino)
		{
			int res = -1;
			using (var connection = new MySqlConnection(ConnectionString))
			{
				string sql = "UPDATE inquilinos SET " +
	"Nombre=@nombre, Apellido=@apellido, Telefono=@telefono, Dni=@dni, Email=@email " + "WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
					command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
					command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                    command.Parameters.AddWithValue("@email", inquilino.Email);
					command.Parameters.AddWithValue("@dni", inquilino.Dni);
					command.Parameters.AddWithValue("@id", inquilino.Id);
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}
