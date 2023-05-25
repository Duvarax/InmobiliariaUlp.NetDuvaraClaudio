using MySql.Data.MySqlClient;

namespace PracticaMVC.Models;

public class RepositorioUsuario
{   
   private IConfiguration config;

   public RepositorioUsuario(IConfiguration config)
   {
    this.config = config;
   }

   public List<Usuario> GetUsuarios()
   {
    List<Usuario> usuarios = new List<Usuario>();
    using (MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
    {
        var query = @"SELECT Id, Nombre, Apellido, Email, NombreUsuario, Avatar, Rol FROM usuarios";
        
        using (var command = new MySqlCommand(query, conn))
        {
            conn.Open();

            using (var reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                Usuario usuario = new Usuario{
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Email = reader.GetString("Email"),
                    NombreUsuario = reader.GetString("NombreUsuario"),
                    Avatar = reader.GetString("Avatar"),
                    Rol = reader.GetInt32("Rol")
                };
                usuarios.Add(usuario);
               }
            }
        }
        conn.Close();

    }
    return usuarios;
   }

   public int agregarUsuario(Usuario usuario)
   {
    int res = -1;
    using(MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
    {   
        var query = @"INSERT INTO usuarios(`Nombre`, `Apellido`, `Email`, `NombreUsuario`, `Contraseña`, `Avatar`,`Rol`) VALUES (@Nombre, @Apellido, @Email, @NombreUsuario, @Contraseña, @Avatar, @Rol); SELECT LAST_INSERT_ID();
        ;";
        using(var command = new MySqlCommand(query, conn))
        {
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            command.Parameters.AddWithValue("@Email", usuario.Email);
            command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
            command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
            if(String.IsNullOrEmpty(usuario.Avatar))
            
                command.Parameters.AddWithValue("@Avatar", DBNull.Value);
            else
            command.Parameters.AddWithValue("@Avatar", usuario.Avatar);
            command.Parameters.AddWithValue("@Rol", usuario.Rol);
            conn.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            usuario.Id = res;
            conn.Close();
        }
    }

    return res;
   }

    public Usuario obtenerUsuarioById(int id)
    {
        Usuario? usuario = null;
        using(MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
        {
            var query = @$"SELECT Id, Nombre, Apellido, Email, NombreUsuario, Contraseña, Avatar, Rol FROM usuarios WHERE {nameof(Usuario.Id)} = @id";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Email = reader.GetString("Email"),
                        NombreUsuario = reader.GetString("NombreUsuario"),
                        Contraseña = reader.GetString("Contraseña"),
                        Avatar = reader.GetString("Avatar"),
                        Rol = reader.GetInt32("Rol")
                    };

                }
                conn.Close();
            }
        }
        return usuario;
    }
    public Usuario obtenerUsuarioByEmail(String email)
    {
        Usuario? usuario = null;
        using(MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
        {
            var query = @$"SELECT Id, Nombre, Apellido, Email, NombreUsuario, Contraseña, Avatar, Rol FROM usuarios WHERE {nameof(Usuario.Email)} = @email";

            using(MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.Add("@email", MySqlDbType.String).Value = email;
                command.CommandType = System.Data.CommandType.Text;
                conn.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Email = reader.GetString("Email"),
                        NombreUsuario = reader.GetString("NombreUsuario"),
                        Contraseña = reader.GetString("Contraseña"),
                        Avatar = reader.GetString("Avatar"),
                        Rol = reader.GetInt32("Rol")
                    };

                }
                conn.Close();
            }
        }
        return usuario;
    }
    public int eliminarUsuarioById(int id)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(config["ConnectionStrings:SQL"]))
        {
            var query = @$"DELETE FROM usuarios WHERE {nameof(Usuario.Id)} = @id";

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

    public int modificarUsuario(Usuario Usuario)
		{
			int res = -1;
			using (var connection = new MySqlConnection(config["ConnectionStrings:SQL"]))
			{
				string sql = "UPDATE usuarios SET " +
	"Nombre=@Nombre, Apellido=@Apellido, Email=@Email, NombreUsuario=@NombreUsuario, Contraseña=@Contraseña, Avatar=@Avatar WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@Nombre", Usuario.Nombre);
					command.Parameters.AddWithValue("@Apellido", Usuario.Apellido);
					command.Parameters.AddWithValue("@Email", Usuario.Email);
                    command.Parameters.AddWithValue("@NombreUsuario", Usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@Contraseña", Usuario.Contraseña);
                    command.Parameters.AddWithValue("@Avatar", Usuario.Avatar);
					command.Parameters.AddWithValue("@id", Usuario.Id);
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}
