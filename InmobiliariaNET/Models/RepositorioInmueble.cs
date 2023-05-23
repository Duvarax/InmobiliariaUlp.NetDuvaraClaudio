using MySql.Data.MySqlClient;

namespace PracticaMVC.Models;

public class RepositorioInmueble
{   

   private readonly IConfiguration config;

   public RepositorioInmueble(IConfiguration config)
   {
    this.config = config;
   }

   public List<Inmueble> GetInmuebles()
   {
    List<Inmueble> inmuebles = new List<Inmueble>();
    using (MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
    {
        var query = @"SELECT i.Id, Direccion, Ambientes, Uso, Tipo, Precio,
        PropietarioId, Estado, Nombre, Apellido 
        FROM inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.Id;";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Inmueble inmueble = new Inmueble{
                    Id = reader.GetInt32(0),
                    Direccion = reader.GetString("Direccion"),
                    Ambientes = reader.GetInt32("Ambientes"),
                    Uso = reader.GetString("Uso"),
                    Tipo = reader.GetString("Tipo"),
                    Precio = reader.GetDouble("Precio"),
                    PropietarioId = reader.GetInt32("PropietarioId"),
                    Estado = reader.GetBoolean("Estado"),
                    Duenio = new Propietario
                    {
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
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
   public List<Inmueble> GetInmueblesByDisponibilidad()
   {
    List<Inmueble> inmuebles = new List<Inmueble>();
    using (MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
    {
        var query = @"SELECT i.Id, Direccion, Ambientes, Uso, Tipo, Precio,
        PropietarioId, Estado, Nombre, Apellido 
        FROM inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.Id AND i.Estado = 1;";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Inmueble inmueble = new Inmueble{
                    Id = reader.GetInt32(0),
                    Direccion = reader.GetString("Direccion"),
                    Ambientes = reader.GetInt32("Ambientes"),
                    Uso = reader.GetString("Uso"),
                    Tipo = reader.GetString("Tipo"),
                    Precio = reader.GetDouble("Precio"),
                    PropietarioId = reader.GetInt32("PropietarioId"),
                    Estado = reader.GetBoolean("Estado"),
                    Duenio = new Propietario
                    {
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
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
   public List<Inmueble> GetInmueblesByNoDisponibilidad()
   {
    List<Inmueble> inmuebles = new List<Inmueble>();
    using (MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
    {
        var query = @"SELECT i.Id, Direccion, Ambientes,Uso, Tipo, Precio,
        PropietarioId, Estado, Nombre, Apellido 
        FROM inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.Id AND i.Estado = 0;";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Inmueble inmueble = new Inmueble{
                    Id = reader.GetInt32(0),
                    Direccion = reader.GetString("Direccion"),
                    Ambientes = reader.GetInt32("Ambientes"),
                    Uso = reader.GetString("Uso"),
                    Tipo = reader.GetString("Tipo"),
                    Precio = reader.GetDouble("Precio"),
                    PropietarioId = reader.GetInt32("PropietarioId"),
                    Estado = reader.GetBoolean("Estado"),
                    Duenio = new Propietario
                    {
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
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

   public List<Inmueble> GetInmueblesPorPropietario(int id)
   {
    List<Inmueble> inmuebles = new List<Inmueble>();
    using (MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
    {
        var query = @"SELECT i.Id, Direccion, Ambientes, Uso, Tipo, Precio,
        PropietarioId, Estado, Nombre, Apellido 
        FROM inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.Id AND i.PropietarioId = @id;";
        
        using (var command = new MySqlCommand(query, conn))
        {
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Inmueble inmueble = new Inmueble{
                    Id = reader.GetInt32(0),
                    Direccion = reader.GetString("Direccion"),
                    Ambientes = reader.GetInt32("Ambientes"),
                    Uso = reader.GetString("Uso"),
                    Tipo = reader.GetString("Tipo"),
                    Precio = reader.GetDouble("Precio"),
                    PropietarioId = reader.GetInt32("PropietarioId"),
                    Estado = reader.GetBoolean("Estado"),
                    Duenio = new Propietario
                    {
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
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

   public int agregarInmueble(Inmueble inmueble)
   {
    int res = -1;
    using(MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
    {   
        var query = @"INSERT INTO inmuebles(`Direccion`, `Ambientes`, `Uso`, `Tipo`, Precio, `PropietarioId`, `Estado`) VALUES (@direccion, @ambientes, @uso, @tipo, @precio, @propietarioid, @estado); SELECT LAST_INSERT_ID();
        ;";
        using(var command = new MySqlCommand(query, conn))
        {
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
            command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
            command.Parameters.AddWithValue("@uso", inmueble.Uso);
            command.Parameters.AddWithValue("@tipo", inmueble.Tipo);
            command.Parameters.AddWithValue("@precio", inmueble.Precio);
            command.Parameters.AddWithValue("@propietarioid", inmueble.PropietarioId);
            command.Parameters.AddWithValue("@estado", inmueble.Estado);
            conn.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            inmueble.Id = res;
            Console.Write(res);
            conn.Close();
        }
    }

    return res;
   }

    public Inmueble obtenerInmuebleById(int id)
    {
        Inmueble inmueble = null;
        using(MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
        {
            var query = @$"SELECT i.*, p.Nombre, p.Apellido FROM inmuebles i INNER JOIN propietarios p WHERE i.PropietarioId = p.Id AND i.Id = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    inmueble = new Inmueble{
                    Id = reader.GetInt32(0),
                    Direccion = reader.GetString("Direccion"),
                    Ambientes = reader.GetInt32("Ambientes"),
                    Uso = reader.GetString("Uso"),
                    Tipo = reader.GetString("Tipo"),
                    Precio = reader.GetDouble("Precio"),
                    PropietarioId = reader.GetInt32("PropietarioId"),
                    Estado = reader.GetBoolean("Estado"),
                    Duenio = new Propietario
                    {
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                    }
                    
                };

                }
                conn.Close();
            }
        }
        return inmueble;
    }
    public int eliminarInmuebleById(int id)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
        {
            var query = @$"DELETE FROM inmuebles WHERE {nameof(Inmueble.Id)} = @id";

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

    public int modificarInmueble(Inmueble inmueble)
		{
			int res = -1;
			using (var connection = new MySqlConnection(config["ConnectionStrings:SQL"]))
			{
				string sql = "UPDATE inmuebles SET " +
	"Direccion=@direccion, Ambientes=@ambientes, Uso=@uso, Tipo=@tipo, Imagen=@imagen , Precio=@precio, PropietarioId=@propietarioid, Estado=@estado " + "WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.AddWithValue("@id", inmueble.Id);
					command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                    command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                    command.Parameters.AddWithValue("@uso", inmueble.Uso);
                    command.Parameters.AddWithValue("@tipo", inmueble.Tipo);
                    command.Parameters.AddWithValue("@imagen", inmueble.Imagen);
                    command.Parameters.AddWithValue("@propietarioid", inmueble.PropietarioId);
                    command.Parameters.AddWithValue("@precio", inmueble.Precio);
                    command.Parameters.AddWithValue("@estado", inmueble.Estado);
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}
