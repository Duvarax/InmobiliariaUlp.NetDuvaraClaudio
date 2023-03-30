using MySql.Data.MySqlClient;

namespace PracticaMVC.Models;

public class RepositorioInmueble
{   
   string ConnectionString = "Server=localhost;User=root;Password=;Database=testmvc;SslMode=none";

   public RepositorioInmueble()
   {
    
   }

   public List<Inmueble> GetInmuebles()
   {
    List<Inmueble> inmuebles = new List<Inmueble>();
    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
    {
        var query = @"SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Latitud, i.Longitud, 
        i.PropietarioId, p.Nombre, p.Apellido 
        FROM inmuebles i, propietarios p WHERE i.PropietarioId = p.Id";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Inmueble inmueble = new Inmueble{
                    Id = reader.GetInt32(0),
                    Direccion = reader.GetString(1),
                    Ambientes = reader.GetInt32(2),
                    Superficie = reader.GetInt32(3),
                    Latitud = reader.GetDecimal(4),
                    Longitud = reader.GetDecimal(5),
                    PropietarioId = reader.GetInt32(6),
                    Duenio = new Propietario
                    {
                        Id = reader.GetInt32(6),
                        Nombre = reader.GetString(7),
                        Apellido = reader.GetString(8),
                    }
                    
                };
                inmuebles.Add(inmueble);
               }
            }
        }
        conn.Close();

    }
    return inmuebles;
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

    public Inmueble obtenerInmuebleById(int id)
    {
        Inmueble inmueble = null;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"SELECT i.*, p.Nombre, p.Apellido FROM inmuebles i, propietarios p WHERE i.PropietarioId = p.Id AND i.PropietarioId = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    inmueble = new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString("Direccion"),
                        Ambientes = reader.GetInt32("Ambientes"),
                        Superficie = reader.GetInt32("Superficie"),
                        Latitud = reader.GetDecimal("Latitud"),
                        Longitud = reader.GetDecimal("Longitud"),
                        PropietarioId = reader.GetInt32("PropietarioId"),
                        Duenio = new Propietario
                        {
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido")
                        }

                    };

                }
                conn.Close();
            }
        }
        return inmueble;
    }
    public int eliminarInquilinoById(int id)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"DELETE FROM inquilinos WHERE {nameof(Propietario.Id)} = @id";

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
