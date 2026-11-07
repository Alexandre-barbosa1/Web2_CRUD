namespace Web2_CRUD.Models;

public class Pedido
{
    public int Id { get; set; }
    public string Produto { get; set; } = string.Empty;
    public decimal Valor { get; set; }

    // Chave Estrangeira (Foreign Key)
    public int ClienteId { get; set; }

    // Propriedades auxiliares para exibição ou mapeamento do ORM (ex: EF Core / Dapper)
    public string? ClienteNome { get; set; } 
    public Cliente? Cliente { get; set; }
}