using MySql.Data.MySqlClient;

namespace PracticaMVC.Models;

public class RepositorioContrato
{   
   string ConnectionString = "Server=localhost;User=root;Password=;Database=testmvc;SslMode=none";

   public RepositorioContrato()
   {
    
   }

   public List<Contrato> GetContratos()
   {
    List<Contrato> contratos = new List<Contrato>();
    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
    {
        var query = @"SELECT c.Id, InquilinoId, InmuebleId, fechaInicio, fechaFinalizacion, c.Precio, i.Nombre, i.Apellido, m.Direccion 
                    FROM contratos c INNER JOIN
                    inquilinos i INNER JOIN
                    inmuebles m 
                    WHERE InmuebleId = m.Id AND InquilinoId = i.Id;";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Contrato contrato = new Contrato{
                    Id = reader.GetInt32(0),
                    InquilinoId = reader.GetInt32(1),
                    InmuebleId = reader.GetInt32(2),
                    fechaInicio = reader.GetDateTime(3),
                    fechaFinalizacion = reader.GetDateTime(4),
                    Precio = reader.GetDouble(5),
                    Inquilino = new Inquilino
                    {
                        Nombre = reader.GetString(6),
                        Apellido = reader.GetString(7)
                    },
                    Inmueble = new Inmueble
                    {
                        Direccion = reader.GetString(8)
                    }
                };
                contratos.Add(contrato);
               }
            }
        }
        conn.Close();

    }
    return contratos;
   }

   public int agregarContrato(Contrato contrato)
   {
    int res = -1;
    using(MySqlConnection conn = new MySqlConnection(ConnectionString))
    {   
        var query = @"INSERT INTO contratos(`InquilinoId`, `InmuebleId`, `fechaInicio`, `fechaFinalizacion`, `Precio`) VALUES (@inquilinoid, @inmuebleid, @fechainicio, @fechaFinalizacion, @precio); SELECT LAST_INSERT_ID();
        ;";
        using(var command = new MySqlCommand(query, conn))
        {
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@inquilinoid", contrato.InquilinoId);
            command.Parameters.AddWithValue("@inmuebleid", contrato.InmuebleId);
            command.Parameters.AddWithValue("@fechainicio", contrato.fechaInicio);
            command.Parameters.AddWithValue("@fechafinalizacion", contrato.fechaFinalizacion);
            command.Parameters.AddWithValue("@precio", contrato.Precio);
            conn.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            contrato.Id = res;
            conn.Close();
        }
    }

    return res;
   }

    public Contrato obtenerContratoById(int id)
    {
        Contrato? contrato = null;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"SELECT c.Id, c.fechaInicio, c.fechaFinalizacion,c.Precio, c.InquilinoId, i.Nombre, i.Apellido, c.InmuebleId, 
            m.Direccion FROM contratos c 
            INNER JOIN inquilinos i 
            INNER JOIN inmuebles m 
            ON c.InquilinoId = i.Id AND c.InmuebleId = m.Id 
            AND c.Id = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    contrato = new Contrato
                    {
                        Id = reader.GetInt32(0),
                        fechaInicio = reader.GetDateTime(1),
                        fechaFinalizacion = reader.GetDateTime(2),
                        Precio = reader.GetDouble(3),
                        InquilinoId = reader.GetInt32(4),
                        Inquilino = new Inquilino
                        {
                            Nombre = reader.GetString(5),
                            Apellido = reader.GetString(6),
                        },
                        InmuebleId = reader.GetInt32(7),
                        Inmueble = new Inmueble
                        {
                            Direccion = reader.GetString(8)
                        }
                        

                    };

                }
                conn.Close();
            }
        }
        return contrato;
    }
    public int eliminarContratoById(int id)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"DELETE FROM contratos WHERE {nameof(Contrato.Id)} = @id";

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

    public int modificarContrato(Contrato contrato)
		{
			int res = -1;
			using (var connection = new MySqlConnection(ConnectionString))
			{
				string sql = "UPDATE contratos SET " +
	"InquilinoId=@inquilinoid, InmuebleId=@inmuebleid, fechaInicio=@fechainicio, fechaFinalizacion=@fechafinalizacion WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@inquilinoid", contrato.InquilinoId);
					command.Parameters.AddWithValue("@inmuebleid", contrato.InmuebleId);
					command.Parameters.AddWithValue("@fechainicio", contrato.fechaInicio);
                    command.Parameters.AddWithValue("@fechafinalizacion", contrato.fechaFinalizacion);
					command.Parameters.AddWithValue("@id", contrato.Id);
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}
