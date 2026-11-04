using Npgsql;
using Web2_CRUD.Models;

namespace Web2_CRUD.Data;

public class ClienteRepository
{
    public void Create(Cliente c)
    {
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Cliente (Nome, Email) VALUES (@nome, @email)";
        cmd.Parameters.AddWithValue("nome", c.Nome);
        cmd.Parameters.AddWithValue("email", c.Email);
        cmd.ExecuteNonQuery();
    }

    public List<Cliente> ReadAll()
    {
        var lista = new List<Cliente>();
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nome, Email FROM Cliente ORDER BY Id";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Cliente
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Email = reader.GetString(2)
            });
        }
        return lista;
    }

    public Cliente? ReadById(int id)
    {
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nome, Email FROM Cliente WHERE Id = @id";
        cmd.Parameters.AddWithValue("id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Cliente { Id = reader.GetInt32(0), Nome = reader.GetString(1), Email = reader.GetString(2) };
        }
        return null;
    }

    public void Update(Cliente c)
    {
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Cliente SET Nome = @nome, Email = @email WHERE Id = @id";
        cmd.Parameters.AddWithValue("nome", c.Nome);
        cmd.Parameters.AddWithValue("email", c.Email);
        cmd.Parameters.AddWithValue("id", c.Id);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Cliente WHERE Id = @id";
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }
}
