namespace Web2_CRUD.Models;

public class Pedido
{
    public int Id { get; set; }
    public string Produto { get; set; } = "";
    public decimal Valor { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNome { get; set; } // só p/ exibição
}
