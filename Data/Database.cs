using Npgsql;

namespace Web2_CRUD.Data;

public static class Database
{
    // Lê CRUD_CONNECTION_STRING do ambiente; se ausente, monta a partir do .env
    // (mesmo arquivo usado pelo docker-compose). Nunca hardcode credenciais aqui.
    public static readonly string ConnectionString = BuildConnectionString();

    private static string BuildConnectionString()
    {
        var explicitConnStr = Environment.GetEnvironmentVariable("CRUD_CONNECTION_STRING");
        if (explicitConnStr != null) return explicitConnStr;

        LoadDotEnvIfPresent();

        var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var db = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(db))
        {
            throw new InvalidOperationException(
                "Variáveis de banco não configuradas. Copie .env.example para .env e preencha, " +
                "ou defina CRUD_CONNECTION_STRING no ambiente.");
        }

        return $"Host=localhost;Port={port};Database={db};Username={user};Password={password}";
    }

    // Parser minimalista de .env (sem dependência externa): KEY=VALUE por linha, ignora comentários/vazias.
    private static void LoadDotEnvIfPresent()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        if (!File.Exists(path)) return;

        foreach (var line in File.ReadAllLines(path))
        {
            var trimmed = line.Trim();
            if (trimmed.Length == 0 || trimmed.StartsWith('#')) continue;

            var idx = trimmed.IndexOf('=');
            if (idx <= 0) continue;

            var key = trimmed[..idx].Trim();
            var value = trimmed[(idx + 1)..].Trim();
            if (Environment.GetEnvironmentVariable(key) == null)
                Environment.SetEnvironmentVariable(key, value);
        }
    }

    public static void Inicializar()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Cliente (
                Id SERIAL PRIMARY KEY,
                Nome TEXT NOT NULL,
                Email TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Pedido (
                Id SERIAL PRIMARY KEY,
                Produto TEXT NOT NULL,
                Valor NUMERIC(12,2) NOT NULL,
                ClienteId INTEGER NOT NULL REFERENCES Cliente(Id) ON DELETE CASCADE
            );
        ";
        cmd.ExecuteNonQuery();
    }
}
