namespace Web2_CRUD.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Propriedade de navegação: Lista de pedidos vinculados a este cliente
    public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
}