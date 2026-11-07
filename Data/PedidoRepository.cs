using Npgsql;
using Web2_CRUD.Models;

namespace Web2_CRUD.Data;

public class PedidoRepository
{
    public void Create(Pedido p)
    {
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Pedido (Produto, Valor, ClienteId) VALUES (@produto, @valor, @clienteId)";
        cmd.Parameters.AddWithValue("produto", p.Produto);
        cmd.Parameters.AddWithValue("valor", p.Valor);
        cmd.Parameters.AddWithValue("clienteId", p.ClienteId);
        cmd.ExecuteNonQuery();
    }

    // JOIN com Cliente para mostrar o nome do cliente (associação Pedido -> Cliente)
    public List<Pedido> ReadAll()
    {
        var lista = new List<Pedido>();
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT p.Id, p.Produto, p.Valor, p.ClienteId, c.Nome
            FROM Pedido p
            JOIN Cliente c ON c.Id = p.ClienteId
            ORDER BY p.Id";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Pedido
            {
                Id = reader.GetInt32(0),
                Produto = reader.GetString(1),
                Valor = reader.GetDecimal(2),
                ClienteId = reader.GetInt32(3),
                ClienteNome = reader.GetString(4)
            });
        }
        return lista;
    }

    public Pedido? ReadById(int id)
    {
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT p.Id, p.Produto, p.Valor, p.ClienteId, c.Nome
            FROM Pedido p
            JOIN Cliente c ON c.Id = p.ClienteId
            WHERE p.Id = @id";
        cmd.Parameters.AddWithValue("id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Pedido
            {
                Id = reader.GetInt32(0),
                Produto = reader.GetString(1),
                Valor = reader.GetDecimal(2),
                ClienteId = reader.GetInt32(3),
                ClienteNome = reader.GetString(4)
            };
        }
        return null;
    }

    public List<Pedido> ReadByClienteId(int clienteId)
    {
        var lista = new List<Pedido>();
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT p.Id, Produto, Valor, ClienteId, Nome FROM Pedido p JOIN Cliente c ON c.Id = ClienteId WHERE ClienteId = @clienteId ORDER BY Id";
        cmd.Parameters.AddWithValue("clienteId", clienteId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Pedido
            {
                Id = reader.GetInt32(0),
                Produto = reader.GetString(1),
                Valor = reader.GetDecimal(2),
                ClienteId = reader.GetInt32(3),
                ClienteNome = reader.GetString(4)
            });
        }
        return lista;
    }

    public void Update(Pedido p)
    {
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Pedido SET Produto = @produto, Valor = @valor, ClienteId = @clienteId WHERE Id = @id";
        cmd.Parameters.AddWithValue("produto", p.Produto);
        cmd.Parameters.AddWithValue("valor", p.Valor);
        cmd.Parameters.AddWithValue("clienteId", p.ClienteId);
        cmd.Parameters.AddWithValue("id", p.Id);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = new NpgsqlConnection(Database.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Pedido WHERE Id = @id";
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }
}
