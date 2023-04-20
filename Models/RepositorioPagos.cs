using MySql.Data.MySqlClient;

namespace PracticaMVC.Models;

public class RepositorioPagos
{   
   string ConnectionString = "Server=localhost;User=root;Password=;Database=testmvc;SslMode=none";

   public RepositorioPagos()
   {
    
   }

   public List<Pago> GetPagos()
   {
    List<Pago> Pagos = new List<Pago>();
    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
    {
        var query = @"SELECT p.Id, p.FechaPago, p.Importe, p.ContratoId, fechaInicio FROM pagos p INNER JOIN contratos c WHERE p.ContratoId = c.Id";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Pago pago = new Pago{
                    Id = reader.GetInt32(nameof(Pago.Id)),
                    fechaPago = reader.GetDateTime(nameof(Pago.fechaPago)),
                    Importe = reader.GetDecimal(nameof(Pago.Importe)),
                    ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                    contrato = new Contrato
                    {
                        fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio))
                    }
                    
                };
                Pagos.Add(pago);
               }
            }
        }
        conn.Close();

    }
    return Pagos;
   }

   public List<Pago> GetPagosPorContrato(int id)
   {
    List<Pago> Pagos = new List<Pago>();
    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
    {
        var query = @"SELECT p.Id, p.FechaPago, p.Importe, p.ContratoId, fechaInicio FROM pagos p INNER JOIN contratos c WHERE p.ContratoId = @id AND p.ContratoId = c.Id";
        
        using (var command = new MySqlCommand(query, conn))
        {
            command.Parameters.AddWithValue("@id", id);
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Pago pago = new Pago{
                    Id = reader.GetInt32(nameof(Pago.Id)),
                    fechaPago = reader.GetDateTime(nameof(Pago.fechaPago)),
                    Importe = reader.GetDecimal(nameof(Pago.Importe)),
                    ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                    contrato = new Contrato
                    {
                        fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio))
                    }
                    
                };
                Pagos.Add(pago);
               }
            }
        }
        conn.Close();

    }
    return Pagos;
   }


   public int agregarPago(Pago pago)
   {
    int res = -1;
    using(MySqlConnection conn = new MySqlConnection(ConnectionString))
    {   
        var query = @"INSERT INTO Pagos(`fechaPago`, `Importe`, `ContratoId`) VALUES (@fechapago, @importe, @idcontrato); SELECT LAST_INSERT_ID();
        ;";
        using(var command = new MySqlCommand(query, conn))
        {
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@fechapago", pago.fechaPago);
            command.Parameters.AddWithValue("@importe", pago.Importe);
            command.Parameters.AddWithValue("@idcontrato", pago.ContratoId);
            conn.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            pago.Id = res;
            conn.Close();
        }
    }

    return res;
   }

    public Pago obtenerPagoById(int id)
    {
        Pago? pago = null;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"SELECT p.*, c.fechaInicio FROM pagos p INNER JOIN contratos c WHERE p.{nameof(Pago.Id)} = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    pago = new Pago
                    {
                        Id = reader.GetInt32(nameof(Pago.Id)),
                        fechaPago = reader.GetDateTime(nameof(Pago.fechaPago)),
                        Importe = reader.GetDecimal(nameof(Pago.Importe)),
                        ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                        contrato = new Contrato
                        {
                            fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio))
                        }
                    };

                }
                conn.Close();
            }
        }
        return pago;
    }
    public Pago obtenerPagoByIdDeContrato(int id)
    {
        Pago? pago = null;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"SELECT p.*, c.fechaInicio FROM pagos p INNER JOIN contratos c WHERE ContratoId =  @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    pago = new Pago
                    {
                        Id = reader.GetInt32(nameof(Pago.Id)),
                        fechaPago = reader.GetDateTime(nameof(Pago.fechaPago)),
                        Importe = reader.GetDecimal(nameof(Pago.Importe)),
                        ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                        contrato = new Contrato
                        {
                            fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio))
                        }
                    };

                }
                conn.Close();
            }
        }
        return pago;
    }
    public int eliminarPagoById(int id)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            var query = @$"DELETE FROM pagos WHERE {nameof(Pago.Id)} = @id";

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

    public int modificarPago(Pago Pago)
		{
			int res = -1;
			using (var connection = new MySqlConnection(ConnectionString))
			{
				var sql = @$"UPDATE Pagos SET fechaPago=@fechapago, Importe=@importe, ContratoId=@idcontrato WHERE {nameof(Pago.Id)} = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@fechapago", Pago.fechaPago);
					command.Parameters.AddWithValue("@importe", Pago.Importe);
					command.Parameters.AddWithValue("@idcontrato", Pago.ContratoId);
                    command.Parameters.AddWithValue("@id", Pago.Id);
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}
